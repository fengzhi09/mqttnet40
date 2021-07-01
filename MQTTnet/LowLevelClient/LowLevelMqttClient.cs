// Decompiled with JetBrains decompiler
// Type: MQTTnet.LowLevelClient.LowLevelMqttClient
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Adapter;
using MQTTnet.Client.Options;
using MQTTnet.Diagnostics;
using MQTTnet.Packets;

namespace MQTTnet.LowLevelClient
{
    public sealed class LowLevelMqttClient : ILowLevelMqttClient, IDisposable
    {
        private readonly IMqttNetScopedLogger _logger;
        private readonly IMqttClientAdapterFactory _clientAdapterFactory;
        private IMqttChannelAdapter _adapter;
        private IMqttClientOptions _options;

        public LowLevelMqttClient(IMqttClientAdapterFactory clientAdapterFactory, IMqttNetLogger logger)
        {
            _clientAdapterFactory =
                clientAdapterFactory ?? throw new ArgumentNullException(nameof(clientAdapterFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger.CreateScopedLogger(nameof(LowLevelMqttClient));
        }

        private bool IsConnected => _adapter != null;

        public async Task ConnectAsync(
            IMqttClientOptions options,
            CancellationToken cancellationToken)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (_adapter != null)
                throw new InvalidOperationException(
                    "Low level MQTT client is already connected. Disconnect first before connecting again.");
            var newAdapter = _clientAdapterFactory.CreateClientAdapter(options);
            try
            {
                _logger.Verbose(
                    $"Trying to connect with server '{options.ChannelOptions}' (Timeout={options.CommunicationTimeout}).");
                await newAdapter.ConnectAsync(options.CommunicationTimeout, cancellationToken).ConfigureAwait(false);
                _logger.Verbose("Connection with server established.");
                _options = options;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Connect failed.");
                _adapter?.Dispose();
                throw;
            }

            _adapter = newAdapter;
            newAdapter = null;
        }

        public async Task DisconnectAsync(CancellationToken cancellationToken)
        {
            if (_adapter == null)
                return;
            await SafeDisconnect(cancellationToken).ConfigureAwait(false);
            _adapter = null;
        }

        public async Task SendAsync(MqttBasePacket packet, CancellationToken cancellationToken)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));
            if (_adapter == null)
                throw new InvalidOperationException("Low level MQTT client is not connected.");
            try
            {
                await _adapter.SendPacketAsync(packet, _options.CommunicationTimeout, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SafeDisconnect(cancellationToken).ConfigureAwait(false);
                throw ex;
            }
        }

        public async Task<MqttBasePacket> ReceiveAsync(
            CancellationToken cancellationToken)
        {
            if (_adapter == null)
            {
                throw new InvalidOperationException("Low level MQTT client is not connected.");
            }

            try
            {
                return await _adapter.ReceivePacketAsync(_options.CommunicationTimeout, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception)
            {
                await SafeDisconnect(cancellationToken).ConfigureAwait(false);
                throw;
            }
        }

        public void Dispose() => _adapter?.Dispose();

        private async Task SafeDisconnect(CancellationToken cancellationToken)
        {
            try
            {
                await _adapter.DisconnectAsync(_options.CommunicationTimeout, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while disconnecting.");
            }
            finally
            {
                _adapter.Dispose();
            }
        }
    }
}