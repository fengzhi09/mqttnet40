// Decompiled with JetBrains decompiler
// Type: MQTTnet.Implementations.MqttTcpServerAdapter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Adapter;
using MQTTnet.Diagnostics;
using MQTTnet.Server;

namespace MQTTnet.Implementations
{
  public sealed class MqttTcpServerAdapter : IMqttServerAdapter, IDisposable
  {
    private readonly List<MqttTcpServerListener> _listeners = new List<MqttTcpServerListener>();
    private readonly IMqttNetScopedLogger _logger;
    private readonly IMqttNetLogger _rootLogger;
    private CancellationTokenSource _cancellationTokenSource;

    public MqttTcpServerAdapter(IMqttNetLogger logger)
    {
      _rootLogger = logger ?? throw new ArgumentNullException(nameof (logger));
      _logger = logger.CreateScopedLogger(nameof (MqttTcpServerAdapter));
    }

    public Func<IMqttChannelAdapter, Task> ClientHandler { get; set; }

    public bool TreatSocketOpeningErrorAsWarning { get; set; }

    public Task StartAsync(IMqttServerOptions options)
    {
      _cancellationTokenSource = _cancellationTokenSource == null ? new CancellationTokenSource() : throw new InvalidOperationException("Server is already started.");
      if (options.DefaultEndpointOptions.IsEnabled)
        RegisterListeners(options.DefaultEndpointOptions, null, _cancellationTokenSource.Token);
      var tlsEndpointOptions = options.TlsEndpointOptions;
      if ((tlsEndpointOptions != null ? (tlsEndpointOptions.IsEnabled ? 1 : 0) : 0) != 0)
      {
        if (options.TlsEndpointOptions.CertificateProvider == null)
          throw new ArgumentException("TLS certificate is not set.");
        var certificate = options.TlsEndpointOptions.CertificateProvider.GetCertificate();
        if (!certificate.HasPrivateKey)
          throw new InvalidOperationException("The certificate for TLS encryption must contain the private key.");
        RegisterListeners(options.TlsEndpointOptions, certificate, _cancellationTokenSource.Token);
      }
      return PlatformAbstractionLayer.CompletedTask;
    }

    public Task StopAsync()
    {
      Cleanup();
      return PlatformAbstractionLayer.CompletedTask;
    }

    public void Dispose() => Cleanup();

    private void Cleanup()
    {
      _cancellationTokenSource?.Cancel(false);
      _cancellationTokenSource?.Dispose();
      _cancellationTokenSource = null;
      foreach (var listener in _listeners)
        listener.Dispose();
      _listeners.Clear();
    }

    private void RegisterListeners(
      MqttServerTcpEndpointBaseOptions options,
      X509Certificate2 tlsCertificate,
      CancellationToken cancellationToken)
    {
      if (!options.BoundInterNetworkAddress.Equals(IPAddress.None))
      {
        var tcpServerListener = new MqttTcpServerListener(AddressFamily.InterNetwork, options, tlsCertificate, _rootLogger)
        {
          ClientHandler = OnClientAcceptedAsync
        };
        if (tcpServerListener.Start(TreatSocketOpeningErrorAsWarning, cancellationToken))
          _listeners.Add(tcpServerListener);
      }
      if (options.BoundInterNetworkV6Address.Equals(IPAddress.None))
        return;
      var tcpServerListener1 = new MqttTcpServerListener(AddressFamily.InterNetworkV6, options, tlsCertificate, _rootLogger)
      {
        ClientHandler = OnClientAcceptedAsync
      };
      if (!tcpServerListener1.Start(TreatSocketOpeningErrorAsWarning, cancellationToken))
        return;
      _listeners.Add(tcpServerListener1);
    }

    private Task OnClientAcceptedAsync(IMqttChannelAdapter channelAdapter)
    {
      var clientHandler = ClientHandler;
      return clientHandler == null ? TaskExtension.FromResult(0) : clientHandler(channelAdapter);
    }
  }
}
