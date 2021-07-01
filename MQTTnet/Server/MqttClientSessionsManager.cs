// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttClientSessionsManager
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Runtime.CompilerServices;
using MQTTnet.Adapter;
using MQTTnet.Diagnostics;
using MQTTnet.Exceptions;
using MQTTnet.Formatter;
using MQTTnet.Internal;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MQTTnet.Server.Status;

namespace MQTTnet.Server
{
    public sealed class MqttClientSessionsManager : IDisposable
    {
        private readonly AsyncQueue<MqttEnqueuedApplicationMessage> _messageQueue =
            new AsyncQueue<MqttEnqueuedApplicationMessage>();

        private readonly AsyncLock _createConnectionGate = new AsyncLock();

        private readonly ConcurrentDictionary<string, MqttClientConnection> _connections =
            new ConcurrentDictionary<string, MqttClientConnection>();

        private readonly ConcurrentDictionary<string, MqttClientSession> _sessions =
            new ConcurrentDictionary<string, MqttClientSession>();

        private readonly IDictionary<object, object> _serverSessionItems = new ConcurrentDictionary<object, object>();
        private readonly CancellationToken _cancellationToken;
        private readonly MqttServerEventDispatcher _eventDispatcher;
        private readonly IMqttRetainedMessagesManager _retainedMessagesManager;
        private readonly IMqttServerOptions _options;
        private readonly IMqttNetScopedLogger _logger;
        private readonly IMqttNetLogger _rootLogger;

        public MqttClientSessionsManager(
            IMqttServerOptions options,
            IMqttRetainedMessagesManager retainedMessagesManager,
            CancellationToken cancellationToken,
            MqttServerEventDispatcher eventDispatcher,
            IMqttNetLogger logger)
        {
            _cancellationToken = cancellationToken;
            _logger = logger != null
                ? logger.CreateScopedLogger(nameof(MqttClientSessionsManager))
                : throw new ArgumentNullException(nameof(logger));
            _rootLogger = logger;
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _retainedMessagesManager = retainedMessagesManager ??
                                       throw new ArgumentNullException(nameof(retainedMessagesManager));
        }

        public void Start() => TaskExtension
            .Run(() => TryProcessQueuedApplicationMessagesAsync(_cancellationToken).Wait(), _cancellationToken)
            .Forget(_logger);

        public async Task StopAsync()
        {
            foreach (var clientConnection in _connections.Values)
                await clientConnection.StopAsync().ConfigureAwait(false);
        }

        public Task HandleClientConnectionAsync(IMqttChannelAdapter clientAdapter) => clientAdapter != null
            ? HandleClientConnectionAsync(clientAdapter, _cancellationToken)
            : throw new ArgumentNullException(nameof(clientAdapter));

        public Task<IList<IMqttClientStatus>> GetClientStatusAsync()
        {
            var mqttClientStatusList = new List<IMqttClientStatus>();
            foreach (var connection in _connections.Values)
            {
                var status1 = new MqttClientStatus(connection);
                connection.FillStatus(status1);
                var status2 = new MqttSessionStatus(connection.Session, this);
                connection.Session.FillStatus(status2);
                status1.Session = status2;
                mqttClientStatusList.Add(status1);
            }

            return TaskExtension.FromResult<IList<IMqttClientStatus>>(mqttClientStatusList);
        }

        public Task<IList<IMqttSessionStatus>> GetSessionStatusAsync()
        {
            var mqttSessionStatusList = new List<IMqttSessionStatus>();
            foreach (var session in _sessions.Values)
            {
                var status = new MqttSessionStatus(session, this);
                session.FillStatus(status);
                mqttSessionStatusList.Add(status);
            }

            return TaskExtension.FromResult<IList<IMqttSessionStatus>>(mqttSessionStatusList);
        }

        public void DispatchApplicationMessage(
            MqttApplicationMessage applicationMessage,
            MqttClientConnection sender)
        {
            if (applicationMessage == null)
                throw new ArgumentNullException(nameof(applicationMessage));
            _messageQueue.Enqueue(new MqttEnqueuedApplicationMessage(applicationMessage, sender));
        }

        public Task SubscribeAsync(string clientId, ICollection<MqttTopicFilter> topicFilters)
        {
            if (clientId == null)
                throw new ArgumentNullException(nameof(clientId));
            if (topicFilters == null)
                throw new ArgumentNullException(nameof(topicFilters));
            MqttClientSession mqttClientSession;
            if (!_sessions.TryGetValue(clientId, out mqttClientSession))
                throw new InvalidOperationException("Client session '" + clientId + "' is unknown.");
            return mqttClientSession.SubscribeAsync(topicFilters);
        }

        public Task UnsubscribeAsync(string clientId, IEnumerable<string> topicFilters)
        {
            if (clientId == null)
                throw new ArgumentNullException(nameof(clientId));
            if (topicFilters == null)
                throw new ArgumentNullException(nameof(topicFilters));
            MqttClientSession mqttClientSession;
            if (!_sessions.TryGetValue(clientId, out mqttClientSession))
                throw new InvalidOperationException("Client session '" + clientId + "' is unknown.");
            return mqttClientSession.UnsubscribeAsync(topicFilters);
        }

        public async Task DeleteSessionAsync(string clientId)
        {
            MqttClientConnection clientConnection;
            if (_connections.TryGetValue(clientId, out clientConnection))
                await clientConnection.StopAsync().ConfigureAwait(false);
            _sessions.TryRemove(clientId, out var _);
            _logger.Verbose("Session for client '{0}' deleted.", (object) clientId);
        }

        public void Dispose() => _messageQueue?.Dispose();

        private async Task TryProcessQueuedApplicationMessagesAsync(
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await TryProcessNextQueuedApplicationMessageAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Verbose("Canceled while processing queued application messages. reason:" + ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Unhandled exception while processing queued application messages.");
                }
            }
        }

        private async Task TryProcessNextQueuedApplicationMessageAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                var queueDequeueResult = await _messageQueue.TryDequeueAsync(cancellationToken).ConfigureAwait(false);
                if (!queueDequeueResult.IsSuccess)
                    return;
                var applicationMessage1 = queueDequeueResult.Item;
                var sender = applicationMessage1.Sender;
                var applicationMessage = applicationMessage1.ApplicationMessage;
                var interceptorContext =
                    await InterceptApplicationMessageAsync(sender, applicationMessage).ConfigureAwait(false);
                if (interceptorContext != null)
                {
                    if (interceptorContext.CloseConnection && sender != null)
                        await sender.StopAsync().ConfigureAwait(false);
                    if (interceptorContext.ApplicationMessage == null || !interceptorContext.AcceptPublish)
                        return;
                    applicationMessage = interceptorContext.ApplicationMessage;
                }

                await _eventDispatcher.SafeNotifyApplicationMessageReceivedAsync(sender?.ClientId, applicationMessage)
                    .ConfigureAwait(false);
                if (applicationMessage.Retain)
                    await _retainedMessagesManager.HandleMessageAsync(sender?.ClientId, applicationMessage)
                        .ConfigureAwait(false);
                var num = 0;
                foreach (var mqttClientSession in _sessions.Values)
                {
                    if (mqttClientSession.EnqueueApplicationMessage(applicationMessage, sender?.ClientId, false))
                        ++num;
                }

                if (num == 0)
                {
                    var messageInterceptor = _options.UndeliveredMessageInterceptor;
                    if (messageInterceptor == null)
                        return;
                    await messageInterceptor.InterceptApplicationMessagePublishAsync(
                        new MqttApplicationMessageInterceptorContext(sender?.ClientId, sender?.Session?.Items,
                            applicationMessage));
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.Verbose("Canceled while processing next queued application messages. reason:" + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception while processing next queued application message.");
            }
        }

        private async Task HandleClientConnectionAsync(
            IMqttChannelAdapter channelAdapter,
            CancellationToken cancellationToken)
        {
            try
            {
                MqttConnectPacket connectPacket;
                try
                {
                    var firstPacket = await channelAdapter
                        .ReceivePacketAsync(_options.DefaultCommunicationTimeout, cancellationToken)
                        .ConfigureAwait(false);
                    connectPacket = firstPacket as MqttConnectPacket;
                    if (connectPacket == null)
                    {
                        _logger.Warning(null,
                            "The first packet from client '{0}' was no 'CONNECT' packet [MQTT-3.1.0-1].",
                            channelAdapter.Endpoint);
                        return;
                    }
                }
                catch (MqttCommunicationTimedOutException ex)
                {
                    _logger.Warning(null, "Client '{0}' connected but did not sent a CONNECT packet. Because of {1}",
                        (object) channelAdapter.Endpoint, ex);
                    return;
                }

                var connectionValidatorContext =
                    await ValidateConnectionAsync(connectPacket, channelAdapter).ConfigureAwait(false);
                if (connectionValidatorContext.ReasonCode != MqttConnectReasonCode.Success)
                {
                    // Send failure response here without preparing a session. The result for a successful connect
                    // will be sent from the session itself.
                    var connAckPacket =
                        channelAdapter.PacketFormatterAdapter.DataConverter.CreateConnAckPacket(
                            connectionValidatorContext);
                    await channelAdapter
                        .SendPacketAsync(connAckPacket, _options.DefaultCommunicationTimeout, cancellationToken)
                        .ConfigureAwait(false);

                    return;
                }


                await (await CreateClientConnectionAsync(connectPacket, connectionValidatorContext, channelAdapter)
                    .ConfigureAwait(false)).RunAsync().ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }

        public async Task CleanUpClient(
            string clientId,
            IMqttChannelAdapter channelAdapter,
            MqttClientDisconnectType disconnectType)
        {
            ConfiguredTaskAwaitable configuredTaskAwaitable;
            if (clientId != null)
            {
                _connections.TryRemove(clientId, out var _);
                if (!_options.EnablePersistentSessions)
                {
                    configuredTaskAwaitable = DeleteSessionAsync(clientId).ConfigureAwait(false);
                    await configuredTaskAwaitable;
                }
            }

            configuredTaskAwaitable = SafeCleanupChannelAsync(channelAdapter).ConfigureAwait(false);
            await configuredTaskAwaitable;
            if (clientId == null)
                return;
            configuredTaskAwaitable = _eventDispatcher.SafeNotifyClientDisconnectedAsync(clientId, disconnectType)
                .ConfigureAwait(false);
            await configuredTaskAwaitable;
        }

        private async Task<MqttConnectionValidatorContext> ValidateConnectionAsync(
            MqttConnectPacket connectPacket,
            IMqttChannelAdapter channelAdapter)
        {
            var context = new MqttConnectionValidatorContext(connectPacket, channelAdapter,
                new ConcurrentDictionary<object, object>());
            var connectionValidator = _options.ConnectionValidator;
            if (connectionValidator == null)
            {
                context.ReasonCode = MqttConnectReasonCode.Success;
                return context;
            }

            await connectionValidator.ValidateConnectionAsync(context).ConfigureAwait(false);
            if (string.IsNullOrEmpty(connectPacket.ClientId) &&
                channelAdapter.PacketFormatterAdapter.ProtocolVersion == MqttProtocolVersion.V500)
                connectPacket.ClientId = context.AssignedClientIdentifier;
            if (string.IsNullOrEmpty(connectPacket.ClientId))
                context.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
            return context;
        }

        private async Task<MqttClientConnection> CreateClientConnectionAsync(
            MqttConnectPacket connectPacket,
            MqttConnectionValidatorContext connectionValidatorContext,
            IMqttChannelAdapter channelAdapter)
        {
            MqttClientConnection connection;
            MqttClientConnection existingConnection = null;
            using (await _createConnectionGate.WaitAsync(_cancellationToken).ConfigureAwait(false))
            {
                var session = _sessions.AddOrUpdate(
                    connectPacket.ClientId,
                    key =>
                    {
                        _logger.Verbose("Created a new session for client '{0}'.", key);
                        return new MqttClientSession(key, connectionValidatorContext.SessionItems, _eventDispatcher,
                            _options, _retainedMessagesManager, _rootLogger);
                    },
                    (key, existingSession) =>
                    {
                        if (connectPacket.CleanSession)
                        {
                            _logger.Verbose("Deleting existing session of client '{0}'.", connectPacket.ClientId);
                            return new MqttClientSession(key, connectionValidatorContext.SessionItems, _eventDispatcher,
                                _options, _retainedMessagesManager, _rootLogger);
                        }

                        _logger.Verbose("Reusing existing session of client '{0}'.", connectPacket.ClientId);
                        return existingSession;
                    });

                connection = new MqttClientConnection(connectPacket, channelAdapter, session,
                    connectionValidatorContext, _options, this, _retainedMessagesManager, _rootLogger);

                _connections.AddOrUpdate(
                    connectPacket.ClientId,
                    key => connection,
                    (key, tempExistingConnection) =>
                    {
                        existingConnection = tempExistingConnection;
                        return connection;
                    });
            }

            // Disconnect the client outside of the lock so that new clients can still connect while a single
            // one is being disconnected.
            if (existingConnection != null)
            {
                await existingConnection.StopAsync(true).ConfigureAwait(false);
            }

            return connection;
        }

        private async Task<MqttApplicationMessageInterceptorContext> InterceptApplicationMessageAsync(
            MqttClientConnection senderConnection,
            MqttApplicationMessage applicationMessage)
        {
            var messageInterceptor = _options.ApplicationMessageInterceptor;
            if (messageInterceptor == null)
                return null;
            string clientId;
            IDictionary<object, object> sessionItems;
            if (senderConnection == null)
            {
                clientId = _options.ClientId;
                sessionItems = _serverSessionItems;
            }
            else
            {
                clientId = senderConnection.ClientId;
                sessionItems = senderConnection.Session.Items;
            }

            var interceptorContext =
                new MqttApplicationMessageInterceptorContext(clientId, sessionItems, applicationMessage);
            await messageInterceptor.InterceptApplicationMessagePublishAsync(interceptorContext).ConfigureAwait(false);
            return interceptorContext;
        }

        private async Task SafeCleanupChannelAsync(IMqttChannelAdapter channelAdapter)
        {
            try
            {
                await channelAdapter.DisconnectAsync(_options.DefaultCommunicationTimeout, CancellationToken.None)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while disconnecting client channel.");
            }
        }
    }
}