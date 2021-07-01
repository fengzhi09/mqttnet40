// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.V3.MqttV310PacketFormatter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Linq;
using MQTTnet.Adapter;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Formatter.V3
{
  public class MqttV310PacketFormatter : IMqttPacketFormatter
  {
    private const int FixedHeaderSize = 1;
    private static readonly MqttPingReqPacket PingReqPacket = new MqttPingReqPacket();
    private static readonly MqttPingRespPacket PingRespPacket = new MqttPingRespPacket();
    private static readonly MqttDisconnectPacket DisconnectPacket = new MqttDisconnectPacket();
    private readonly IMqttPacketWriter _packetWriter;

    public MqttV310PacketFormatter(IMqttPacketWriter packetWriter) => _packetWriter = packetWriter;

    public IMqttDataConverter DataConverter { get; } = (IMqttDataConverter) new MqttV310DataConverter();

    public ArraySegment<byte> Encode(MqttBasePacket packet)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      _packetWriter.Reset(5);
      _packetWriter.Seek(5);
      var num1 = EncodePacket(packet, _packetWriter);
      var num2 = (uint) (_packetWriter.Length - 5);
      var offset = 5 - (1 + MqttPacketWriter.GetLengthOfVariableInteger(num2));
      _packetWriter.Seek(offset);
      _packetWriter.Write(num1);
      _packetWriter.WriteVariableLengthInteger(num2);
      return new ArraySegment<byte>(_packetWriter.GetBuffer(), offset, _packetWriter.Length - offset);
    }

    public MqttBasePacket Decode(ReceivedMqttPacket receivedMqttPacket)
    {
      if (receivedMqttPacket == null)
        throw new ArgumentNullException(nameof (receivedMqttPacket));
      var num = receivedMqttPacket.FixedHeader >> 4;
      if (num < 1 || num > 14)
        throw new MqttProtocolViolationException(string.Format("The packet type is invalid ({0}).", num));
      switch (num)
      {
        case 1:
          return DecodeConnectPacket(receivedMqttPacket.Body);
        case 2:
          return DecodeConnAckPacket(receivedMqttPacket.Body);
        case 3:
          return DecodePublishPacket(receivedMqttPacket);
        case 4:
          return DecodePubAckPacket(receivedMqttPacket.Body);
        case 5:
          return DecodePubRecPacket(receivedMqttPacket.Body);
        case 6:
          return DecodePubRelPacket(receivedMqttPacket.Body);
        case 7:
          return DecodePubCompPacket(receivedMqttPacket.Body);
        case 8:
          return DecodeSubscribePacket(receivedMqttPacket.Body);
        case 9:
          return DecodeSubAckPacket(receivedMqttPacket.Body);
        case 10:
          return DecodeUnsubscribePacket(receivedMqttPacket.Body);
        case 11:
          return DecodeUnsubAckPacket(receivedMqttPacket.Body);
        case 12:
          return PingReqPacket;
        case 13:
          return PingRespPacket;
        case 14:
          return DisconnectPacket;
        default:
          throw new MqttProtocolViolationException(string.Format("Packet type ({0}) not supported.", num));
      }
    }

    public void FreeBuffer() => _packetWriter.FreeBuffer();

    private byte EncodePacket(MqttBasePacket packet, IMqttPacketWriter packetWriter)
    {
      switch (packet)
      {
        case MqttConnectPacket packet1:
          return EncodeConnectPacket(packet1, packetWriter);
        case MqttConnAckPacket packet2:
          return EncodeConnAckPacket(packet2, packetWriter);
        case MqttDisconnectPacket _:
          return EncodeEmptyPacket(MqttControlPacketType.Disconnect);
        case MqttPingReqPacket _:
          return EncodeEmptyPacket(MqttControlPacketType.PingReq);
        case MqttPingRespPacket _:
          return EncodeEmptyPacket(MqttControlPacketType.PingResp);
        case MqttPublishPacket packet3:
          return EncodePublishPacket(packet3, packetWriter);
        case MqttPubAckPacket packet4:
          return EncodePubAckPacket(packet4, packetWriter);
        case MqttPubRecPacket packet5:
          return EncodePubRecPacket(packet5, packetWriter);
        case MqttPubRelPacket packet6:
          return EncodePubRelPacket(packet6, packetWriter);
        case MqttPubCompPacket packet7:
          return EncodePubCompPacket(packet7, packetWriter);
        case MqttSubscribePacket packet8:
          return EncodeSubscribePacket(packet8, packetWriter);
        case MqttSubAckPacket packet9:
          return EncodeSubAckPacket(packet9, packetWriter);
        case MqttUnsubscribePacket packet10:
          return EncodeUnsubscribePacket(packet10, packetWriter);
        case MqttUnsubAckPacket packet11:
          return EncodeUnsubAckPacket(packet11, packetWriter);
        default:
          throw new MqttProtocolViolationException("Packet type invalid.");
      }
    }

    private static MqttBasePacket DecodeUnsubAckPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      return new MqttUnsubAckPacket
      {
        PacketIdentifier = body.ReadTwoByteInteger()
      };
    }

    private static MqttBasePacket DecodePubCompPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttPubCompPacket = new MqttPubCompPacket();
      mqttPubCompPacket.PacketIdentifier = body.ReadTwoByteInteger();
      return mqttPubCompPacket;
    }

    private static MqttBasePacket DecodePubRelPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttPubRelPacket = new MqttPubRelPacket();
      mqttPubRelPacket.PacketIdentifier = body.ReadTwoByteInteger();
      return mqttPubRelPacket;
    }

    private static MqttBasePacket DecodePubRecPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttPubRecPacket = new MqttPubRecPacket();
      mqttPubRecPacket.PacketIdentifier = body.ReadTwoByteInteger();
      return mqttPubRecPacket;
    }

    private static MqttBasePacket DecodePubAckPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttPubAckPacket = new MqttPubAckPacket();
      mqttPubAckPacket.PacketIdentifier = body.ReadTwoByteInteger();
      return mqttPubAckPacket;
    }

    private static MqttBasePacket DecodeUnsubscribePacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var unsubscribePacket = new MqttUnsubscribePacket
      {
        PacketIdentifier = body.ReadTwoByteInteger()
      };
      while (!body.EndOfStream)
        unsubscribePacket.TopicFilters.Add(body.ReadStringWithLengthPrefix());
      return unsubscribePacket;
    }

    private static MqttBasePacket DecodeSubscribePacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttSubscribePacket = new MqttSubscribePacket
      {
        PacketIdentifier = body.ReadTwoByteInteger()
      };
      while (!body.EndOfStream)
      {
        var mqttTopicFilter = new MqttTopicFilter
        {
          Topic = body.ReadStringWithLengthPrefix(),
          QualityOfServiceLevel = (MqttQualityOfServiceLevel) body.ReadByte()
        };
        mqttSubscribePacket.TopicFilters.Add(mqttTopicFilter);
      }
      return mqttSubscribePacket;
    }

    private static MqttBasePacket DecodePublishPacket(
      ReceivedMqttPacket receivedMqttPacket)
    {
      ThrowIfBodyIsEmpty(receivedMqttPacket.Body);
      var flag1 = (receivedMqttPacket.FixedHeader & 1) > 0;
      var qualityOfServiceLevel = (MqttQualityOfServiceLevel) (receivedMqttPacket.FixedHeader >> 1 & 3);
      var flag2 = (receivedMqttPacket.FixedHeader & 8) > 0;
      var str = receivedMqttPacket.Body.ReadStringWithLengthPrefix();
      var nullable = new ushort?();
      if (qualityOfServiceLevel > MqttQualityOfServiceLevel.AtMostOnce)
        nullable = receivedMqttPacket.Body.ReadTwoByteInteger();
      var mqttPublishPacket1 = new MqttPublishPacket();
      mqttPublishPacket1.PacketIdentifier = nullable;
      mqttPublishPacket1.Retain = flag1;
      mqttPublishPacket1.Topic = str;
      mqttPublishPacket1.QualityOfServiceLevel = qualityOfServiceLevel;
      mqttPublishPacket1.Dup = flag2;
      var mqttPublishPacket2 = mqttPublishPacket1;
      if (!receivedMqttPacket.Body.EndOfStream)
        mqttPublishPacket2.Payload = receivedMqttPacket.Body.ReadRemainingData();
      return mqttPublishPacket2;
    }

    private MqttBasePacket DecodeConnectPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var str = body.ReadStringWithLengthPrefix();
      var num1 = body.ReadByte();
      if (str != "MQTT" && str != "MQIsdp")
        throw new MqttProtocolViolationException("MQTT protocol name do not match MQTT v3.");
      if (num1 != 3 && num1 != 4)
        throw new MqttProtocolViolationException("MQTT protocol version do not match MQTT v3.");
      var packet = new MqttConnectPacket();
      var num2 = body.ReadByte();
      if ((num2 & 1) > 0)
        throw new MqttProtocolViolationException("The first bit of the Connect Flags must be set to 0.");
      packet.CleanSession = (num2 & 2) > 0;
      var flag1 = (num2 & 4) > 0;
      var num3 = (num2 & 24) >> 3;
      var flag2 = (num2 & 32) > 0;
      var num4 = ((int) num2 & 64) > 0 ? 1 : 0;
      var num5 = ((int) num2 & 128) > 0 ? 1 : 0;
      packet.KeepAlivePeriod = body.ReadTwoByteInteger();
      packet.ClientId = body.ReadStringWithLengthPrefix();
      if (flag1)
        packet.WillMessage = new MqttApplicationMessage
        {
          Topic = body.ReadStringWithLengthPrefix(),
          Payload = body.ReadWithLengthPrefix(),
          QualityOfServiceLevel = (MqttQualityOfServiceLevel) num3,
          Retain = flag2
        };
      if (num5 != 0)
        packet.Username = body.ReadStringWithLengthPrefix();
      if (num4 != 0)
        packet.Password = body.ReadWithLengthPrefix();
      ValidateConnectPacket(packet);
      return packet;
    }

    private static MqttBasePacket DecodeSubAckPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttSubAckPacket = new MqttSubAckPacket
      {
        PacketIdentifier = body.ReadTwoByteInteger()
      };
      while (!body.EndOfStream)
        mqttSubAckPacket.ReturnCodes.Add((MqttSubscribeReturnCode) body.ReadByte());
      return mqttSubAckPacket;
    }

    protected virtual MqttBasePacket DecodeConnAckPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttConnAckPacket = new MqttConnAckPacket();
      int num = body.ReadByte();
      mqttConnAckPacket.ReturnCode = (MqttConnectReturnCode) body.ReadByte();
      return mqttConnAckPacket;
    }

    protected void ValidateConnectPacket(MqttConnectPacket packet)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      if (string.IsNullOrEmpty(packet.ClientId) && !packet.CleanSession)
        throw new MqttProtocolViolationException("CleanSession must be set if ClientId is empty [MQTT-3.1.3-7].");
    }

    private static void ValidatePublishPacket(MqttPublishPacket packet)
    {
      if (packet.QualityOfServiceLevel == MqttQualityOfServiceLevel.AtMostOnce && packet.Dup)
        throw new MqttProtocolViolationException("Dup flag must be false for QoS 0 packets [MQTT-3.3.1-2].");
    }

    protected virtual byte EncodeConnectPacket(
      MqttConnectPacket packet,
      IMqttPacketWriter packetWriter)
    {
      ValidateConnectPacket(packet);
      packetWriter.WriteWithLengthPrefix("MQIsdp");
      packetWriter.Write(3);
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

    protected virtual byte EncodeConnAckPacket(
      MqttConnAckPacket packet,
      IMqttPacketWriter packetWriter)
    {
      packetWriter.Write(0);
      packetWriter.Write((byte) packet.ReturnCode.Value);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.ConnAck);
    }

    private static byte EncodePubRelPacket(MqttPubRelPacket packet, IMqttPacketWriter packetWriter)
    {
      if (!packet.PacketIdentifier.HasValue)
        throw new MqttProtocolViolationException("PubRel packet has no packet identifier.");
      packetWriter.Write(packet.PacketIdentifier.Value);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PubRel, 2);
    }

    private static byte EncodePublishPacket(
      MqttPublishPacket packet,
      IMqttPacketWriter packetWriter)
    {
      ValidatePublishPacket(packet);
      packetWriter.WriteWithLengthPrefix(packet.Topic);
      if (packet.QualityOfServiceLevel > MqttQualityOfServiceLevel.AtMostOnce)
      {
        if (!packet.PacketIdentifier.HasValue)
          throw new MqttProtocolViolationException("Publish packet has no packet identifier.");
        packetWriter.Write(packet.PacketIdentifier.Value);
      }
      else
      {
        var packetIdentifier = packet.PacketIdentifier;
        var nullable = packetIdentifier.HasValue ? packetIdentifier.GetValueOrDefault() : new int?();
        var num = 0;
        if (nullable.GetValueOrDefault() > num & nullable.HasValue)
          throw new MqttProtocolViolationException("Packet identifier must be empty if QoS == 0 [MQTT-2.3.1-5].");
      }
      var payload = packet.Payload;
      if ((payload != null ? ((uint) payload.Length > 0U ? 1 : 0) : 0) != 0)
        packetWriter.Write(packet.Payload, 0, packet.Payload.Length);
      byte num1 = 0;
      if (packet.Retain)
        num1 |= 1;
      var flags = (byte) (num1 | (uint) (byte) ((uint) (byte) packet.QualityOfServiceLevel << 1));
      if (packet.Dup)
        flags |= 8;
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.Publish, flags);
    }

    private static byte EncodePubAckPacket(MqttPubAckPacket packet, IMqttPacketWriter packetWriter)
    {
      if (!packet.PacketIdentifier.HasValue)
        throw new MqttProtocolViolationException("PubAck packet has no packet identifier.");
      packetWriter.Write(packet.PacketIdentifier.Value);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PubAck);
    }

    private static byte EncodePubRecPacket(MqttPubRecPacket packet, IMqttPacketWriter packetWriter)
    {
      if (!packet.PacketIdentifier.HasValue)
        throw new MqttProtocolViolationException("PubRec packet has no packet identifier.");
      packetWriter.Write(packet.PacketIdentifier.Value);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PubRec);
    }

    private static byte EncodePubCompPacket(
      MqttPubCompPacket packet,
      IMqttPacketWriter packetWriter)
    {
      if (!packet.PacketIdentifier.HasValue)
        throw new MqttProtocolViolationException("PubComp packet has no packet identifier.");
      packetWriter.Write(packet.PacketIdentifier.Value);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PubComp);
    }

    private static byte EncodeSubscribePacket(
      MqttSubscribePacket packet,
      IMqttPacketWriter packetWriter)
    {
      var nullable = packet.TopicFilters.Any() ? packet.PacketIdentifier : throw new MqttProtocolViolationException("At least one topic filter must be set [MQTT-3.8.3-3].");
      if (!nullable.HasValue)
        throw new MqttProtocolViolationException("Subscribe packet has no packet identifier.");
      var mqttPacketWriter = packetWriter;
      nullable = packet.PacketIdentifier;
      int num = nullable.Value;
      mqttPacketWriter.Write((ushort) num);
      var topicFilters = packet.TopicFilters;
      // ISSUE: explicit non-virtual call
      if ((topicFilters != null ? ( (topicFilters.Count) > 0 ? 1 : 0) : 0) != 0)
      {
        foreach (var topicFilter in packet.TopicFilters)
        {
          packetWriter.WriteWithLengthPrefix(topicFilter.Topic);
          packetWriter.Write((byte) topicFilter.QualityOfServiceLevel);
        }
      }
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.Subscribe, 2);
    }

    private static byte EncodeSubAckPacket(MqttSubAckPacket packet, IMqttPacketWriter packetWriter)
    {
      if (!packet.PacketIdentifier.HasValue)
        throw new MqttProtocolViolationException("SubAck packet has no packet identifier.");
      packetWriter.Write(packet.PacketIdentifier.Value);
      var returnCodes = packet.ReturnCodes;
      if ((returnCodes != null ? (returnCodes.Any() ? 1 : 0) : 0) != 0)
      {
        foreach (var returnCode in packet.ReturnCodes)
          packetWriter.Write((byte) returnCode);
      }
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.SubAck);
    }

    private static byte EncodeUnsubscribePacket(
      MqttUnsubscribePacket packet,
      IMqttPacketWriter packetWriter)
    {
      var nullable = packet.TopicFilters.Any() ? packet.PacketIdentifier : throw new MqttProtocolViolationException("At least one topic filter must be set [MQTT-3.10.3-2].");
      if (!nullable.HasValue)
        throw new MqttProtocolViolationException("Unsubscribe packet has no packet identifier.");
      var mqttPacketWriter = packetWriter;
      nullable = packet.PacketIdentifier;
      int num = nullable.Value;
      mqttPacketWriter.Write((ushort) num);
      var topicFilters = packet.TopicFilters;
      if ((topicFilters != null ? (topicFilters.Any() ? 1 : 0) : 0) != 0)
      {
        foreach (var topicFilter in packet.TopicFilters)
          packetWriter.WriteWithLengthPrefix(topicFilter);
      }
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.Unsubscibe, 2);
    }

    private static byte EncodeUnsubAckPacket(
      MqttUnsubAckPacket packet,
      IMqttPacketWriter packetWriter)
    {
      if (!packet.PacketIdentifier.HasValue)
        throw new MqttProtocolViolationException("UnsubAck packet has no packet identifier.");
      packetWriter.Write(packet.PacketIdentifier.Value);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.UnsubAck);
    }

    private static byte EncodeEmptyPacket(MqttControlPacketType type) => MqttPacketWriter.BuildFixedHeader(type);

    protected static void ThrowIfBodyIsEmpty(IMqttPacketBodyReader body)
    {
      if (body == null || body.Length == 0)
        throw new MqttProtocolViolationException("Data from the body is required but not present.");
    }
  }
}
