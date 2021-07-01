// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.MqttPacketFormatterAdapter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using MQTTnet.Adapter;
using MQTTnet.Exceptions;
using MQTTnet.Formatter.V3;
using MQTTnet.Formatter.V5;
using MQTTnet.Packets;

namespace MQTTnet.Formatter
{
  public class MqttPacketFormatterAdapter
  {
    private IMqttPacketFormatter _formatter;

    public MqttPacketFormatterAdapter(MqttProtocolVersion protocolVersion)
      : this(protocolVersion, new MqttPacketWriter())
    {
    }

    public MqttPacketFormatterAdapter(MqttProtocolVersion protocolVersion, IMqttPacketWriter writer)
      : this(writer)
    {
      UseProtocolVersion(protocolVersion);
    }

    public MqttPacketFormatterAdapter(IMqttPacketWriter writer) => Writer = writer;

    public MqttProtocolVersion ProtocolVersion { get; private set; }

    public IMqttDataConverter DataConverter
    {
      get
      {
        ThrowIfFormatterNotSet();
        return _formatter.DataConverter;
      }
    }

    public IMqttPacketWriter Writer { get; }

    public ArraySegment<byte> Encode(MqttBasePacket packet)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      ThrowIfFormatterNotSet();
      return _formatter.Encode(packet);
    }

    public MqttBasePacket Decode(ReceivedMqttPacket receivedMqttPacket)
    {
      if (receivedMqttPacket == null)
        throw new ArgumentNullException(nameof (receivedMqttPacket));
      ThrowIfFormatterNotSet();
      return _formatter.Decode(receivedMqttPacket);
    }

    public void FreeBuffer() => _formatter?.FreeBuffer();

    public void DetectProtocolVersion(ReceivedMqttPacket receivedMqttPacket)
    {
      var protocolVersion = ParseProtocolVersion(receivedMqttPacket);
      receivedMqttPacket.Body.Seek(0);
      UseProtocolVersion(protocolVersion);
    }

    private void UseProtocolVersion(MqttProtocolVersion protocolVersion)
    {
      ProtocolVersion = protocolVersion != MqttProtocolVersion.Unknown ? protocolVersion : throw new InvalidOperationException("MQTT protocol version is invalid.");
      _formatter = GetMqttPacketFormatter(protocolVersion, Writer);
    }

    public static IMqttPacketFormatter GetMqttPacketFormatter(
      MqttProtocolVersion protocolVersion,
      IMqttPacketWriter writer)
    {
      switch (protocolVersion)
      {
        case MqttProtocolVersion.Unknown:
          throw new InvalidOperationException("MQTT protocol version is invalid.");
        case MqttProtocolVersion.V310:
          return new MqttV310PacketFormatter(writer);
        case MqttProtocolVersion.V311:
          return new MqttV311PacketFormatter(writer);
        case MqttProtocolVersion.V500:
          return new MqttV500PacketFormatter(writer);
        default:
          throw new NotSupportedException();
      }
    }

    private MqttProtocolVersion ParseProtocolVersion(
      ReceivedMqttPacket receivedMqttPacket)
    {
      if (receivedMqttPacket == null)
        throw new ArgumentNullException(nameof (receivedMqttPacket));
      if (receivedMqttPacket.Body.Length < 7)
        throw new MqttProtocolViolationException("CONNECT packet must have at least 7 bytes.");
      var str = receivedMqttPacket.Body.ReadStringWithLengthPrefix();
      var num = receivedMqttPacket.Body.ReadByte();
      if (str == "MQTT")
      {
        if (num == 5)
          return MqttProtocolVersion.V500;
        if (num == 4)
          return MqttProtocolVersion.V311;
        throw new MqttProtocolViolationException(string.Format("Protocol level '{0}' not supported.", num));
      }
      if (!(str == "MQIsdp"))
        throw new MqttProtocolViolationException("Protocol '" + str + "' not supported.");
      if (num == 3)
        return MqttProtocolVersion.V310;
      throw new MqttProtocolViolationException(string.Format("Protocol level '{0}' not supported.", num));
    }

    private void ThrowIfFormatterNotSet()
    {
      if (_formatter == null)
        throw new InvalidOperationException("Protocol version not set or detected.");
    }
  }
}
