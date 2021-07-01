// Decompiled with JetBrains decompiler
// Type: MQTTnet.Implementations.MqttTcpChannel
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Channel;
using MQTTnet.Client.Options;
using MQTTnet.Exceptions;

namespace MQTTnet.Implementations
{
    public sealed class MqttTcpChannel : IMqttChannel
    {
        private readonly IMqttClientOptions _clientOptions;
        private readonly MqttClientTcpOptions _options;
        private Stream _stream;

        public MqttTcpChannel(IMqttClientOptions clientOptions)
        {
            _clientOptions = clientOptions ?? throw new ArgumentNullException(nameof(clientOptions));
            _options = (MqttClientTcpOptions) clientOptions.ChannelOptions;
            var channelOptions = clientOptions.ChannelOptions;
            int num;
            if (channelOptions == null)
            {
                num = 0;
            }
            else
            {
                var useTls = channelOptions.TlsOptions?.UseTls;
                var flag = true;
                num = useTls.GetValueOrDefault() == flag & useTls.HasValue ? 1 : 0;
            }

            IsSecureConnection = num != 0;
        }

        public MqttTcpChannel(Stream stream, string endpoint, X509Certificate2 clientCertificate)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            Endpoint = endpoint;
            IsSecureConnection = stream is SslStream;
            ClientCertificate = clientCertificate;
        }

        public string Endpoint { get; private set; }

        public bool IsSecureConnection { get; }

        public X509Certificate2 ClientCertificate { get; }

        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            CrossPlatformSocket socket = null;
            try
            {
                socket = _options.AddressFamily == AddressFamily.Unspecified
                    ? new CrossPlatformSocket()
                    : new CrossPlatformSocket(_options.AddressFamily);

                socket.ReceiveBufferSize = _options.BufferSize;
                socket.SendBufferSize = _options.BufferSize;
                socket.SendTimeout = (int) _clientOptions.CommunicationTimeout.TotalMilliseconds;
                socket.NoDelay = _options.NoDelay;

                if (_options.DualMode.HasValue)
                {
                    // It is important to avoid setting the flag if no specific value is set by the user
                    // because on IPv4 only networks the setter will always throw an exception. Regardless
                    // of the actual value.
                    socket.DualMode = _options.DualMode.Value;
                }

                await socket.ConnectAsync(_options.Server, _options.GetPort(), cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                var networkStream = socket.GetStream();
                if (_options.TlsOptions?.UseTls == true)
                {
                    var sslStream = new SslStream(networkStream, false, InternalUserCertificateValidationCallback);
                    try
                    {
                        // ReSharper disable once HeapView.CanAvoidClosure
                        await Task.Factory.FromAsync(
                            (callback, state) => sslStream.BeginAuthenticateAsClient(_options.Server,
                                LoadCertificates(), _options.TlsOptions.SslProtocol,
                                !_options.TlsOptions.IgnoreCertificateRevocationErrors, callback, state),
                            sslStream.EndAuthenticateAsClient, null);
                    }
                    catch
                    {
                        sslStream.Dispose();
                    }

                    _stream = sslStream;
                }
                else
                {
                    _stream = networkStream;
                }

                Endpoint = socket.RemoteEndPoint?.ToString();
            }
            catch (Exception)
            {
                socket?.Dispose();
                throw;
            }
        }

        public Task DisconnectAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return TaskExtension.FromResult(0);
        }

        public async Task<int> ReadAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            cancellationToken.ThrowIfCancellationRequested();
            using (cancellationToken.Register(Dispose))
            {
                var stream = _stream;
                if (stream == null)
                    return 0;
                return await stream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task WriteAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                // Workaround for: https://github.com/dotnet/corefx/issues/24430

                using (var tokenRegistration = cancellationToken.Register(Dispose))
                {
                    var stream = _stream;

                    if (stream == null)
                    {
                        throw new MqttCommunicationException("The TCP connection is closed.");
                    }

                    await stream.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (ObjectDisposedException)
            {
                throw new MqttCommunicationException("The TCP connection is closed.");
            }
            catch (IOException exception)
            {
                if (exception.InnerException is SocketException socketException)
                {
                    throw socketException;
                }

                throw;
            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _stream = null;
        }

        private bool InternalUserCertificateValidationCallback(
            object sender,
            X509Certificate x509Certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            #region OBSOLETE

#pragma warning disable CS0618 // Type or member is obsolete
            var certificateValidationCallback = _options?.TlsOptions?.CertificateValidationCallback;
#pragma warning restore CS0618 // Type or member is obsolete
            if (certificateValidationCallback != null)
            {
                return certificateValidationCallback(x509Certificate, chain, sslPolicyErrors, _clientOptions);
            }

            #endregion

            var validationHandler = _options?.TlsOptions?.CertificateValidationHandler;
            if (validationHandler != null)
            {
                var validationCallbackContext = new MqttClientCertificateValidationCallbackContext
                {
                    Certificate = x509Certificate,
                    Chain = chain,
                    SslPolicyErrors = sslPolicyErrors,
                    ClientOptions = _options
                };
                return validationHandler(validationCallbackContext);
            }

            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            if (chain.ChainStatus.Any(c =>
                c.Status == X509ChainStatusFlags.RevocationStatusUnknown || c.Status == X509ChainStatusFlags.Revoked ||
                c.Status == X509ChainStatusFlags.OfflineRevocation))
            {
                var options = _options;
                int num;
                if (options == null)
                {
                    num = 1;
                }
                else
                {
                    // ISSUE: explicit non-virtual call
                    var revocationErrors = (options.TlsOptions)?.IgnoreCertificateRevocationErrors;
                    var flag = true;
                    num = !(revocationErrors.GetValueOrDefault() == flag & revocationErrors.HasValue) ? 1 : 0;
                }

                if (num != 0)
                    return false;
            }

            if (chain.ChainStatus.Any(c => c.Status == X509ChainStatusFlags.PartialChain))
            {
                var options = _options;
                int num;
                if (options == null)
                {
                    num = 1;
                }
                else
                {
                    // ISSUE: explicit non-virtual call
                    var certificateChainErrors = (options.TlsOptions)?.IgnoreCertificateChainErrors;
                    var flag = true;
                    num = !(certificateChainErrors.GetValueOrDefault() == flag & certificateChainErrors.HasValue)
                        ? 1
                        : 0;
                }

                if (num != 0)
                    return false;
            }

            var options1 = _options;
            if (options1 == null)
                return false;
            // ISSUE: explicit non-virtual call
            var untrustedCertificates = (options1.TlsOptions)?.AllowUntrustedCertificates;
            var flag1 = true;
            return untrustedCertificates.GetValueOrDefault() == flag1 & untrustedCertificates.HasValue;
        }

        private X509CertificateCollection LoadCertificates()
        {
            var certificateCollection = new X509CertificateCollection();
            if (_options.TlsOptions.Certificates == null)
                return certificateCollection;
            foreach (var certificate in _options.TlsOptions.Certificates)
                certificateCollection.Add(certificate);
            return certificateCollection;
        }
    }
}