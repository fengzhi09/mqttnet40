// Decompiled with JetBrains decompiler
// Type: MQTTnet.PacketDispatcher.MqttPacketDispatcher
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Concurrent;
using MQTTnet.Exceptions;
using MQTTnet.Packets;

namespace MQTTnet.PacketDispatcher
{
  public sealed class MqttPacketDispatcher
  {
    private readonly ConcurrentDictionary<Tuple<ushort, Type>, IMqttPacketAwaiter> _awaiters = new ConcurrentDictionary<Tuple<ushort, Type>, IMqttPacketAwaiter>();

    public void Dispatch(Exception exception)
    {
      foreach (var awaiter in _awaiters)
        awaiter.Value.Fail(exception);
      _awaiters.Clear();
    }

    public void Dispatch(MqttBasePacket packet)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      if (packet is MqttDisconnectPacket disconnectPacket)
      {
        foreach (var awaiter in _awaiters)
          awaiter.Value.Fail(new MqttUnexpectedDisconnectReceivedException(disconnectPacket));
      }
      else
      {
        ushort num = 0;
        if (packet is IMqttPacketWithIdentifier packetWithIdentifier2 && packetWithIdentifier2.PacketIdentifier.HasValue)
          num = packetWithIdentifier2.PacketIdentifier.Value;
        var type = packet.GetType();
        IMqttPacketAwaiter mqttPacketAwaiter;
        if (!_awaiters.TryRemove(new Tuple<ushort, Type>(num, type), out mqttPacketAwaiter))
          throw new MqttProtocolViolationException(string.Format("Received packet '{0}' at an unexpected time.", packet));
        mqttPacketAwaiter.Complete(packet);
      }
    }

    public void Reset()
    {
      foreach (var awaiter in _awaiters)
        awaiter.Value.Cancel();
      _awaiters.Clear();
    }

    public MqttPacketAwaiter<TResponsePacket> AddAwaiter<TResponsePacket>(
      ushort? identifier)
      where TResponsePacket : MqttBasePacket
    {
      if (!identifier.HasValue)
        identifier = 0;
      var mqttPacketAwaiter = new MqttPacketAwaiter<TResponsePacket>(identifier, this);
      var key = new Tuple<ushort, Type>(identifier.Value, typeof (TResponsePacket));
      if (!_awaiters.TryAdd(key, mqttPacketAwaiter))
        throw new InvalidOperationException(string.Format("The packet dispatcher already has an awaiter for packet of type '{0}' with identifier {1}.", key.Item2.Name, key.Item1));
      return mqttPacketAwaiter;
    }

    public void RemoveAwaiter<TResponsePacket>(ushort? identifier) where TResponsePacket : MqttBasePacket
    {
      if (!identifier.HasValue)
        identifier = 0;
      _awaiters.TryRemove(new Tuple<ushort, Type>(identifier.Value, typeof (TResponsePacket)), out var _);
    }
  }
}
