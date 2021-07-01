// Decompiled with JetBrains decompiler
// Type: MQTTnet.Implementations.CrossPlatformSocket
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTnet.Implementations
{
    public sealed class CrossPlatformSocket : IDisposable
    {
        private readonly Socket _socket;
        private NetworkStream _networkStream;

        public CrossPlatformSocket(AddressFamily addressFamily) =>
            _socket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);

        public CrossPlatformSocket()
        {
            _socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            DualMode = true;
        }

        public CrossPlatformSocket(Socket socket)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _networkStream = new NetworkStream(socket, true);
        }

        public bool NoDelay
        {
            get => (int) _socket.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug) > 0;
            set => _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
        }

        public bool DualMode
        {
            get
            {
                if (_socket.AddressFamily != AddressFamily.InterNetworkV6)
                    throw new NotSupportedException("net_invalidversion");
                return (int) _socket.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only) == 0;
            }
            set
            {
                if (_socket.AddressFamily != AddressFamily.InterNetworkV6)
                    throw new NotSupportedException("net_invalidversion");
                _socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, value ? 0 : 1);
            }
        }

        public int ReceiveBufferSize
        {
            get => _socket.ReceiveBufferSize;
            set => _socket.ReceiveBufferSize = value;
        }

        public int SendBufferSize
        {
            get => _socket.SendBufferSize;
            set => _socket.SendBufferSize = value;
        }

        public int SendTimeout
        {
            get => _socket.SendTimeout;
            set => _socket.SendTimeout = value;
        }

        public EndPoint RemoteEndPoint => _socket.RemoteEndPoint;

        public bool ReuseAddress
        {
            get => (uint) (int) _socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress) >
                   0U;
            set => _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, value ? 1 : 0);
        }

        public async Task<CrossPlatformSocket> AcceptAsync()
        {
            try
            {
                return new CrossPlatformSocket(await Task.Factory
                    .FromAsync(_socket.BeginAccept, _socket.EndAccept, null).ConfigureAwait(false));
            }
            catch (ObjectDisposedException)
            {
                // This will happen when _socket.EndAccept gets called by Task library but the socket is already disposed.
                return null;
            }
        }

        public void Bind(EndPoint localEndPoint)
        {
            if (localEndPoint == null)
                throw new ArgumentNullException(nameof(localEndPoint));
            _socket.Bind(localEndPoint);
        }

        public void Listen(int connectionBacklog) => _socket.Listen(connectionBacklog);

        public async Task ConnectAsync(string host, int port, CancellationToken cancellationToken)
        {
            if (host is null) throw new ArgumentNullException(nameof(host));
            try
            {
                _networkStream?.Dispose();
                using (cancellationToken.Register(() => _socket.Dispose()))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Factory.FromAsync(_socket.BeginConnect, _socket.EndConnect, host, port, null)
                        .ConfigureAwait(false);
                    _networkStream = new NetworkStream(_socket, true);
                }
            }
            catch (ObjectDisposedException)
            {
                // This will happen when _socket.EndConnect gets called by Task library but the socket is already disposed.
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public NetworkStream GetStream() => _networkStream ?? throw new IOException("The socket is not connected.");

        public void Dispose()
        {
            _networkStream?.Dispose();
            _socket?.Dispose();
        }
    }
}