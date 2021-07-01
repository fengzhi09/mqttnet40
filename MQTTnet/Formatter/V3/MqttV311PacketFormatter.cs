// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.V3.MqttV311PacketFormatter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Formatter.V3
{
  public class MqttV311PacketFormatter : MqttV310PacketFormatter
  {
    public MqttV311PacketFormatter(IMqttPacketWriter packetWriter)
      : base(packetWriter)
    {
    }

    protected override byte EncodeConnectPacket(
      MqttConnectPacket packet,
      IMqttPacketWriter packetWriter)
    {
      ValidateConnectPacket(packet);
      packetWriter.WriteWithLengthPrefix("MQTT");
      packetWriter.Write(4);
      byte num = 0;
      if (packet.CleanSession)
        num |= 2;
      if (packet.WillMessage != null)
      {
        num = (byte) ((byte) (num | 4U) | (uint) (byte) ((uint) (byte) packet.WillMessage.QualityOfServiceLevel << 3));
        if (packet.WillMessage.Retain)
          num |= 32;
      }
      if (packet.Password != null && packet.Username == null)
        throw new MqttProtocolViolationException("If the User Name Flag is set to 0, the Password Flag MUST be set to 0 [MQTT-3.1.2-22].");
      if (packet.Password != null)
        num |= 64;
      if (packet.Username != null)
        num |= 128;
      packetWriter.Write(num);
      packetWriter.Write(packet.KeepAlivePeriod);
      packetWriter.WriteWithLengthPrefix(packet.ClientId);
      if (packet.WillMessage != null)
      {
        packetWriter.WriteWithLengthPrefix(packet.WillMessage.Topic);
        packetWriter.WriteWithLengthPrefix(packet.WillMessage.Payload);
      }
      if (packet.Username != null)
        packetWriter.WriteWithLengthPrefix(packet.Username);
      if (packet.Password != null)
        packetWriter.WriteWithLengthPrefix(packet.Password);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.Connect);
    }

    protected override byte EncodeConnAckPacket(
      MqttConnAckPacket packet,
      IMqttPacketWriter packetWriter)
    {
      byte num = 0;
      if (packet.IsSessionPresent)
        num |= 1;
      packetWriter.Write(num);
      packetWriter.Write((byte) packet.ReturnCode.Value);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.ConnAck);
    }

    protected override MqttBasePacket DecodeConnAckPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      return new MqttConnAckPacket
      {
        IsSessionPresent = ((body.ReadByte() & 1) > 0),
        ReturnCode = (MqttConnectReturnCode) body.ReadByte()
      };
    }
  }
}
