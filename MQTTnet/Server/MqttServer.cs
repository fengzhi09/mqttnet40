// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServer
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Adapter;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Receiving;
using MQTTnet.Diagnostics;
using MQTTnet.Exceptions;
using MQTTnet.Implementations;
using MQTTnet.Internal;
using MQTTnet.Protocol;
using MQTTnet.Server.Status;

namespace MQTTnet.Server
{
    public class MqttServer :
        Disposable,
        IMqttServer,
        IApplicationMessageReceiver,
        IApplicationMessagePublisher,
        IDisposable
    {
        private readonly MqttServerEventDispatcher _eventDispatcher;
        private readonly ICollection<IMqttServerAdapter> _adapters;
        private readonly IMqttNetLogger _rootLogger;
        private readonly IMqttNetScopedLogger _logger;
        private MqttClientSessionsManager _clientSessionsManager;
        private IMqttRetainedMessagesManager _retainedMessagesManager;
        private CancellationTokenSource _cancellationTokenSource;

        public MqttServer(IEnumerable<IMqttServerAdapter> adapters, IMqttNetLogger logger)
        {
            _adapters = adapters != null
                ? (ICollection<IMqttServerAdapter>) adapters.ToList()
                : throw new ArgumentNullException(nameof(adapters));
            _logger = logger != null
                ? logger.CreateScopedLogger(nameof(MqttServer))
                : throw new ArgumentNullException(nameof(logger));
            _rootLogger = logger;
            _eventDispatcher = new MqttServerEventDispatcher(logger);
        }

        public bool IsStarted => _cancellationTokenSource != null;

        public IMqttServerStartedHandler StartedHandler { get; set; }

        public IMqttServerStoppedHandler StoppedHandler { get; set; }

        public IMqttServerClientConnectedHandler ClientConnectedHandler
        {
            get => _eventDispatcher.ClientConnectedHandler;
            set => _eventDispatcher.ClientConnectedHandler = value;
        }

        public IMqttServerClientDisconnectedHandler ClientDisconnectedHandler
        {
            get => _eventDispatcher.ClientDisconnectedHandler;
            set => _eventDispatcher.ClientDisconnectedHandler = value;
        }

        public IMqttServerClientSubscribedTopicHandler ClientSubscribedTopicHandler
        {
            get => _eventDispatcher.ClientSubscribedTopicHandler;
            set => _eventDispatcher.ClientSubscribedTopicHandler = value;
        }

        public IMqttServerClientUnsubscribedTopicHandler ClientUnsubscribedTopicHandler
        {
            get => _eventDispatcher.ClientUnsubscribedTopicHandler;
            set => _eventDispatcher.ClientUnsubscribedTopicHandler = value;
        }

        public IMqttApplicationMessageReceivedHandler ApplicationMessageReceivedHandler
        {
            get => _eventDispatcher.ApplicationMessageReceivedHandler;
            set => _eventDispatcher.ApplicationMessageReceivedHandler = value;
        }

        public IMqttServerOptions Options { get; private set; }

        public Task<IList<IMqttClientStatus>> GetClientStatusAsync()
        {
            ThrowIfDisposed();
            ThrowIfNotStarted();
            return _clientSessionsManager.GetClientStatusAsync();
        }

        public Task<IList<IMqttSessionStatus>> GetSessionStatusAsync()
        {
            ThrowIfDisposed();
            ThrowIfNotStarted();
            return _clientSessionsManager.GetSessionStatusAsync();
        }

        public Task<IList<MqttApplicationMessage>> GetRetainedApplicationMessagesAsync()
        {
            ThrowIfDisposed();
            ThrowIfNotStarted();
            return _retainedMessagesManager.GetMessagesAsync();
        }

        public Task ClearRetainedApplicationMessagesAsync()
        {
            ThrowIfDisposed();
            ThrowIfNotStarted();
            return _retainedMessagesManager?.ClearMessagesAsync() ?? PlatformAbstractionLayer.CompletedTask;
        }

        public Task SubscribeAsync(string clientId, ICollection<MqttTopicFilter> topicFilters)
        {
            if (clientId == null)
                throw new ArgumentNullException(nameof(clientId));
            if (topicFilters == null)
                throw new ArgumentNullException(nameof(topicFilters));
            ThrowIfDisposed();
            ThrowIfNotStarted();
            return _clientSessionsManager.SubscribeAsync(clientId, topicFilters);
        }

        public Task UnsubscribeAsync(string clientId, ICollection<string> topicFilters)
        {
            if (clientId == null)
                throw new ArgumentNullException(nameof(clientId));
            if (topicFilters == null)
                throw new ArgumentNullException(nameof(topicFilters));
            ThrowIfDisposed();
            ThrowIfNotStarted();
            return _clientSessionsManager.UnsubscribeAsync(clientId, topicFilters);
        }

        public Task<MqttClientPublishResult> PublishAsync(
            MqttApplicationMessage applicationMessage,
            CancellationToken cancellationToken)
        {
            if (applicationMessage == null)
                throw new ArgumentNullException(nameof(applicationMessage));
            ThrowIfDisposed();
            MqttTopicValidator.ThrowIfInvalid(applicationMessage.Topic);
            ThrowIfNotStarted();
            _clientSessionsManager.DispatchApplicationMessage(applicationMessage, null);
            return TaskExtension.FromResult(new MqttClientPublishResult());
        }

        public async Task StartAsync(IMqttServerOptions options)
        {
            var mqttServer1 = this;
            mqttServer1.ThrowIfDisposed();
            mqttServer1.ThrowIfStarted();
            var mqttServer2 = mqttServer1;
            mqttServer2.Options = options ?? throw new ArgumentNullException(nameof(options));
            mqttServer1._cancellationTokenSource = new CancellationTokenSource();
            var mqttServer3 = mqttServer1;
            // ISSUE: explicit non-virtual call
            mqttServer3._retainedMessagesManager = (mqttServer1.Options).RetainedMessagesManager ??
                                                   throw new MqttConfigurationException(
                                                       "options.RetainedMessagesManager should not be null.");
            // ISSUE: explicit non-virtual call
            var configuredTaskAwaitable = mqttServer1._retainedMessagesManager
                .Start((mqttServer1.Options), mqttServer1._rootLogger).ConfigureAwait(false);
            await configuredTaskAwaitable;
            configuredTaskAwaitable = mqttServer1._retainedMessagesManager.LoadMessagesAsync().ConfigureAwait(false);
            await configuredTaskAwaitable;
            // ISSUE: explicit non-virtual call
            mqttServer1._clientSessionsManager = new MqttClientSessionsManager((mqttServer1.Options),
                mqttServer1._retainedMessagesManager, mqttServer1._cancellationTokenSource.Token,
                mqttServer1._eventDispatcher, mqttServer1._rootLogger);
            mqttServer1._clientSessionsManager.Start();
            foreach (var adapter in mqttServer1._adapters)
            {
                adapter.ClientHandler = mqttServer1.OnHandleClient;
                // ISSUE: explicit non-virtual call
                configuredTaskAwaitable = adapter.StartAsync((mqttServer1.Options)).ConfigureAwait(false);
                await configuredTaskAwaitable;
            }

            mqttServer1._logger.Info("Started.");
            // ISSUE: explicit non-virtual call
            IMqttServerStartedHandler startedHandler = (mqttServer1.StartedHandler);
            if (startedHandler == null)
                return;
            await startedHandler.HandleServerStartedAsync(EventArgs.Empty).ConfigureAwait(false);
        }

        public async Task StopAsync()
        {
            try
            {
                if (_cancellationTokenSource == null)
                    return;
                await _clientSessionsManager.StopAsync().ConfigureAwait(false);
                _cancellationTokenSource.Cancel(false);
                foreach (var adapter in _adapters)
                {
                    adapter.ClientHandler = null;
                    await adapter.StopAsync().ConfigureAwait(false);
                }

                _logger.Info("Stopped.");
            }
            finally
            {
                _clientSessionsManager?.Dispose();
                _clientSessionsManager = null;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _retainedMessagesManager = null;
            }

            var stoppedHandler = StoppedHandler;
            if (stoppedHandler == null)
                return;
            await stoppedHandler.HandleServerStoppedAsync(EventArgs.Empty).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopAsync().GetAwaiter().GetResult();
                foreach (IDisposable adapter in _adapters)
                    adapter.Dispose();
            }

            base.Dispose(disposing);
        }

        private Task OnHandleClient(IMqttChannelAdapter channelAdapter) =>
            _clientSessionsManager.HandleClientConnectionAsync(channelAdapter);

        private void ThrowIfStarted()
        {
            if (_cancellationTokenSource != null)
                throw new InvalidOperationException("The MQTT server is already started.");
        }

        private void ThrowIfNotStarted()
        {
            if (_cancellationTokenSource == null)
                throw new InvalidOperationException("The MQTT server is not started.");
        }
    }
}