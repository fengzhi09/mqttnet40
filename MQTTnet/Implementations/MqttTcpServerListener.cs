// Decompiled with JetBrains decompiler
// Type: MQTTnet.Implementations.MqttTcpServerListener
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Adapter;
using MQTTnet.Diagnostics;
using MQTTnet.Formatter;
using MQTTnet.Internal;
using MQTTnet.Server;

namespace MQTTnet.Implementations
{
    public sealed class MqttTcpServerListener : IDisposable
    {
        private readonly IMqttNetScopedLogger _logger;
        private readonly IMqttNetLogger _rootLogger;
        private readonly AddressFamily _addressFamily;
        private readonly MqttServerTcpEndpointBaseOptions _options;
        private readonly MqttServerTlsTcpEndpointOptions _tlsOptions;
        private readonly X509Certificate2 _tlsCertificate;
        private CrossPlatformSocket _socket;
        private IPEndPoint _localEndPoint;

        public MqttTcpServerListener(
            AddressFamily addressFamily,
            MqttServerTcpEndpointBaseOptions options,
            X509Certificate2 tlsCertificate,
            IMqttNetLogger logger)
        {
            _addressFamily = addressFamily;
            _options = options;
            _tlsCertificate = tlsCertificate;
            _rootLogger = logger;
            _logger = logger.CreateScopedLogger(nameof(MqttTcpServerListener));
            if (!(_options is MqttServerTlsTcpEndpointOptions options1))
                return;
            _tlsOptions = options1;
        }

        public Func<IMqttChannelAdapter, Task> ClientHandler { get; set; }

        public bool Start(bool treatErrorsAsWarning, CancellationToken cancellationToken)
        {
            try
            {
                var address = _options.BoundInterNetworkAddress;
                if (_addressFamily == AddressFamily.InterNetworkV6)
                    address = _options.BoundInterNetworkV6Address;
                _localEndPoint = new IPEndPoint(address, _options.Port);
                _logger.Info(string.Format("Starting TCP listener for {0} TLS={1}.", _localEndPoint,
                    _tlsCertificate != null));
                _socket = new CrossPlatformSocket(_addressFamily);
                if (_options.ReuseAddress)
                    _socket.ReuseAddress = true;
                if (_options.NoDelay)
                    _socket.NoDelay = true;
                _socket.Bind(_localEndPoint);
                _socket.Listen(_options.ConnectionBacklog);
                TaskExtension.Run(() => AcceptClientConnectionsAsync(cancellationToken).Wait(), cancellationToken)
                    .Forget(_logger);
                return true;
            }
            catch (Exception ex)
            {
                if (!treatErrorsAsWarning)
                {
                    throw;
                }

                _logger.Warning(ex, "Error while creating listener socket for local end point '{0}'.",
                    (object) _localEndPoint);
                return false;
            }
        }

        public void Dispose() => _socket?.Dispose();

        private async Task AcceptClientConnectionsAsync(CancellationToken cancellationToken)
        {
            var tcpServerListener1 = this;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var tcpServerListener = tcpServerListener1;
                    var clientSocket =
                        await tcpServerListener1._socket.AcceptAsync().ConfigureAwait(false);
                    if (clientSocket != null)
                        TaskExtension
                            .Run(() => tcpServerListener.TryHandleClientConnectionAsync(clientSocket).Wait(),
                                cancellationToken).Forget(tcpServerListener1._logger);
                }
                catch (Exception ex)
                {
                    if (!(ex is SocketException socketException2) ||
                        socketException2.SocketErrorCode != SocketError.ConnectionAborted &&
                        socketException2.SocketErrorCode != SocketError.OperationAborted)
                    {
                        tcpServerListener1._logger.Error(ex,
                            string.Format("Error while accepting connection at TCP listener {0} TLS={1}.",
                                tcpServerListener1._localEndPoint, tcpServerListener1._tlsCertificate != null));
                        await TaskExtension.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task TryHandleClientConnectionAsync(CrossPlatformSocket clientSocket)
        {
            Stream stream = null;
            string remoteEndPoint = null;
            try
            {
                remoteEndPoint = clientSocket.RemoteEndPoint.ToString();
                _logger.Verbose("Client '{0}' accepted by TCP listener '{1}, {2}'.", (object) remoteEndPoint,
                    (object) _localEndPoint,
                    _addressFamily == AddressFamily.InterNetwork ? (object) "ipv4" : (object) "ipv6");
                clientSocket.NoDelay = _options.NoDelay;
                stream = clientSocket.GetStream();
                X509Certificate2 clientCertificate = null;
                if (_tlsCertificate != null)
                {
                    var sslStream = new SslStream(stream, false,
                        _tlsOptions.RemoteCertificateValidationCallback);
                    Task.Factory.FromAsync(
                            (callback, state) => sslStream.BeginAuthenticateAsServer(_tlsCertificate,
                                _tlsOptions.ClientCertificateRequired, _tlsOptions.SslProtocol,
                                _tlsOptions.CheckCertificateRevocation, callback, state),
                            sslStream.EndAuthenticateAsServer, null)
                        .Wait();
                    stream = sslStream;
                    clientCertificate = sslStream.RemoteCertificate as X509Certificate2;

                    if (clientCertificate == null && sslStream.RemoteCertificate != null)
                    {
                        clientCertificate =
                            new X509Certificate2(sslStream.RemoteCertificate.Export(X509ContentType.Cert));
                    }
                }

                var clientHandler = ClientHandler;
                if (clientHandler == null)
                {
                    stream = null;
                    remoteEndPoint = null;
                }
                else
                {
                    using (var clientAdapter = new MqttChannelAdapter(
                        new MqttTcpChannel(stream, remoteEndPoint, clientCertificate),
                        new MqttPacketFormatterAdapter(new MqttPacketWriter()), _rootLogger))
                        await clientHandler(clientAdapter).ConfigureAwait(false);
                    stream = null;
                    remoteEndPoint = null;
                }
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ObjectDisposedException _:
                        stream = null;
                        remoteEndPoint = null;
                        return;
                    case SocketException socketException1:
                        if (socketException1.SocketErrorCode == SocketError.OperationAborted)
                        {
                            stream = null;
                            remoteEndPoint = null;
                            return;
                        }

                        break;
                }

                _logger.Error(ex, "Error while handling client connection.");
                stream = null;
                remoteEndPoint = null;
            }
            finally
            {
                try
                {
                    stream?.Dispose();
                    clientSocket?.Dispose();
                    _logger.Verbose("Client '{0}' disconnected at TCP listener '{1}, {2}'.",
                        (object) remoteEndPoint, (object) _localEndPoint,
                        _addressFamily == AddressFamily.InterNetwork ? (object) "ipv4" : (object) "ipv6");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error while cleaning up client connection");
                }
            }
        }
    }
}