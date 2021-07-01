// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttClientKeepAliveMonitor
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Runtime.CompilerServices;
using MQTTnet.Diagnostics;
using MQTTnet.Internal;

namespace MQTTnet.Server
{
    public sealed class MqttClientKeepAliveMonitor
    {
        private readonly Stopwatch _lastPacketReceivedTracker = new Stopwatch();
        private readonly string _clientId;
        private readonly Func<Task> _keepAliveElapsedCallback;
        private readonly IMqttNetScopedLogger _logger;
        private bool _isPaused;

        public MqttClientKeepAliveMonitor(
            string clientId,
            Func<Task> keepAliveElapsedCallback,
            IMqttNetLogger logger)
        {
            _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            _keepAliveElapsedCallback = keepAliveElapsedCallback ??
                                        throw new ArgumentNullException(nameof(keepAliveElapsedCallback));
            _logger = logger != null
                ? logger.CreateScopedLogger(nameof(MqttClientKeepAliveMonitor))
                : throw new ArgumentNullException(nameof(logger));
        }

        public void Start(int keepAlivePeriod, CancellationToken cancellationToken)
        {
            if (keepAlivePeriod == 0)
                return;
            TaskExtension.Run(() => RunAsync(keepAlivePeriod, cancellationToken).Wait(), cancellationToken)
                .Forget(_logger);
        }

        public void Pause() => _isPaused = true;

        public void Resume() => _isPaused = false;

        public void PacketReceived() => _lastPacketReceivedTracker.Restart();

        private async Task RunAsync(int keepAlivePeriod, CancellationToken cancellationToken)
        {
            try
            {
                _lastPacketReceivedTracker.Restart();
                while (!cancellationToken.IsCancellationRequested)
                {
                    ConfiguredTaskAwaitable configuredTaskAwaitable;
                    if (!_isPaused && _lastPacketReceivedTracker.Elapsed.TotalSeconds >= keepAlivePeriod * 1.5)
                    {
                        _logger.Warning(null, "Client '{0}': Did not receive any packet or keep alive signal.",
                            (object) _clientId);
                        configuredTaskAwaitable = _keepAliveElapsedCallback().ConfigureAwait(false);
                        await configuredTaskAwaitable;
                        break;
                    }

                    configuredTaskAwaitable = TaskExtension
                        .Delay(TimeSpan.FromSeconds(keepAlivePeriod * 0.5), cancellationToken).ConfigureAwait(false);
                    await configuredTaskAwaitable;
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.Verbose("Client '{0}': Canceled, because :{1}.", (object) _clientId, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Client '{0}': Failed checking keep alive timeouts.", (object) _clientId);
            }
            finally
            {
                _logger.Verbose("Client '{0}': Stopped checking keep alive timeout.", (object) _clientId);
            }
        }
    }
}