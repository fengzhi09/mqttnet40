// Decompiled with JetBrains decompiler
// Type: MQTTnet.PacketDispatcher.MqttPacketAwaiter`1
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Exceptions;
using MQTTnet.Packets;

namespace MQTTnet.PacketDispatcher
{
    public sealed class MqttPacketAwaiter<TPacket> : IMqttPacketAwaiter, IDisposable
        where TPacket : MqttBasePacket
    {
        private readonly TaskCompletionSource<MqttBasePacket> _taskCompletionSource;
        private readonly ushort? _packetIdentifier;
        private readonly MqttPacketDispatcher _owningPacketDispatcher;

        public MqttPacketAwaiter(ushort? packetIdentifier, MqttPacketDispatcher owningPacketDispatcher)
        {
            _packetIdentifier = packetIdentifier;
            _owningPacketDispatcher = owningPacketDispatcher ??
                                           throw new ArgumentNullException(nameof(owningPacketDispatcher));
            _taskCompletionSource = new TaskCompletionSource<MqttBasePacket>();
        }

        public async Task<TPacket> WaitOneAsync(TimeSpan timeout)
        {
            using (var timeoutToken = new CancellationTokenSource())
            {
                timeoutToken.CancelAfter(timeout);
                timeoutToken.Token.Register(() => Fail(new MqttCommunicationTimedOutException()));

                var packet = await _taskCompletionSource.Task.ConfigureAwait(false);
                return (TPacket) packet;
            }
        }

        public void Complete(MqttBasePacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));
            _taskCompletionSource.TrySetResult(packet);
        }

        public void Fail(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));
            _taskCompletionSource.TrySetException(exception);
        }

        public void Cancel() => _taskCompletionSource.TrySetCanceled();

        public void Dispose() => _owningPacketDispatcher.RemoveAwaiter<TPacket>(_packetIdentifier);
    }
}