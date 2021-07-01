// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.V5.MqttV500PacketEncoder
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Linq;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Formatter.V5
{
  public class MqttV500PacketEncoder
  {
    private readonly IMqttPacketWriter _packetWriter;

    public MqttV500PacketEncoder()
      : this(new MqttPacketWriter())
    {
    }

    public MqttV500PacketEncoder(IMqttPacketWriter packetWriter) => _packetWriter = packetWriter;

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

    public void FreeBuffer() => _packetWriter.FreeBuffer();

    private static byte EncodePacket(MqttBasePacket packet, IMqttPacketWriter packetWriter)
    {
      switch (packet)
      {
        case MqttConnectPacket packet1:
          return EncodeConnectPacket(packet1, packetWriter);
        case MqttConnAckPacket packet2:
          return EncodeConnAckPacket(packet2, packetWriter);
        case MqttDisconnectPacket packet3:
          return EncodeDisconnectPacket(packet3, packetWriter);
        case MqttPingReqPacket _:
          return EncodePingReqPacket();
        case MqttPingRespPacket _:
          return EncodePingRespPacket();
        case MqttPublishPacket packet4:
          return EncodePublishPacket(packet4, packetWriter);
        case MqttPubAckPacket packet5:
          return EncodePubAckPacket(packet5, packetWriter);
        case MqttPubRecPacket packet6:
          return EncodePubRecPacket(packet6, packetWriter);
        case MqttPubRelPacket packet7:
          return EncodePubRelPacket(packet7, packetWriter);
        case MqttPubCompPacket packet8:
          return EncodePubCompPacket(packet8, packetWriter);
        case MqttSubscribePacket packet9:
          return EncodeSubscribePacket(packet9, packetWriter);
        case MqttSubAckPacket packet10:
          return EncodeSubAckPacket(packet10, packetWriter);
        case MqttUnsubscribePacket packet11:
          return EncodeUnsubscribePacket(packet11, packetWriter);
        case MqttUnsubAckPacket packet12:
          return EncodeUnsubAckPacket(packet12, packetWriter);
        case MqttAuthPacket packet13:
          return EncodeAuthPacket(packet13, packetWriter);
        default:
          throw new MqttProtocolViolationException("Packet type invalid.");
      }
    }

    private static byte EncodeConnectPacket(
      MqttConnectPacket packet,
      IMqttPacketWriter packetWriter)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      if (packetWriter == null)
        throw new ArgumentNullException(nameof (packetWriter));
      if (string.IsNullOrEmpty(packet.ClientId) && !packet.CleanSession)
        throw new MqttProtocolViolationException("CleanSession must be set if ClientId is empty [MQTT-3.1.3-7].");
      packetWriter.WriteWithLengthPrefix("MQTT");
      packetWriter.Write(5);
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
      var propertiesWriter1 = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter1.WriteSessionExpiryInterval(packet.Properties.SessionExpiryInterval);
        propertiesWriter1.WriteAuthenticationMethod(packet.Properties.AuthenticationMethod);
        propertiesWriter1.WriteAuthenticationData(packet.Properties.AuthenticationData);
        propertiesWriter1.WriteRequestProblemInformation(packet.Properties.RequestProblemInformation);
        propertiesWriter1.WriteRequestResponseInformation(packet.Properties.RequestResponseInformation);
        propertiesWriter1.WriteReceiveMaximum(packet.Properties.ReceiveMaximum);
        propertiesWriter1.WriteTopicAliasMaximum(packet.Properties.TopicAliasMaximum);
        propertiesWriter1.WriteMaximumPacketSize(packet.Properties.MaximumPacketSize);
        propertiesWriter1.WriteUserProperties(packet.Properties.UserProperties);
      }
      propertiesWriter1.WriteTo(packetWriter);
      packetWriter.WriteWithLengthPrefix(packet.ClientId);
      if (packet.WillMessage != null)
      {
        var propertiesWriter2 = new MqttV500PropertiesWriter();
        propertiesWriter2.WritePayloadFormatIndicator(packet.WillMessage.PayloadFormatIndicator);
        propertiesWriter2.WriteMessageExpiryInterval(packet.WillMessage.MessageExpiryInterval);
        propertiesWriter2.WriteTopicAlias(packet.WillMessage.TopicAlias);
        propertiesWriter2.WriteResponseTopic(packet.WillMessage.ResponseTopic);
        propertiesWriter2.WriteCorrelationData(packet.WillMessage.CorrelationData);
        propertiesWriter2.WriteSubscriptionIdentifiers(packet.WillMessage.SubscriptionIdentifiers);
        propertiesWriter2.WriteContentType(packet.WillMessage.ContentType);
        propertiesWriter2.WriteUserProperties(packet.WillMessage.UserProperties);
        propertiesWriter2.WriteWillDelayInterval(packet.Properties?.WillDelayInterval);
        propertiesWriter2.WriteTo(packetWriter);
        packetWriter.WriteWithLengthPrefix(packet.WillMessage.Topic);
        packetWriter.WriteWithLengthPrefix(packet.WillMessage.Payload);
      }
      if (packet.Username != null)
        packetWriter.WriteWithLengthPrefix(packet.Username);
      if (packet.Password != null)
        packetWriter.WriteWithLengthPrefix(packet.Password);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.Connect);
    }

    private static byte EncodeConnAckPacket(
      MqttConnAckPacket packet,
      IMqttPacketWriter packetWriter)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      if (packetWriter == null)
        throw new ArgumentNullException(nameof (packetWriter));
      if (!packet.ReasonCode.HasValue)
        ThrowReasonCodeNotSetException();
      byte num = 0;
      if (packet.IsSessionPresent)
        num |= 1;
      packetWriter.Write(num);
      packetWriter.Write((byte) packet.ReasonCode.Value);
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteSessionExpiryInterval(packet.Properties.SessionExpiryInterval);
        propertiesWriter.WriteAuthenticationMethod(packet.Properties.AuthenticationMethod);
        propertiesWriter.WriteAuthenticationData(packet.Properties.AuthenticationData);
        propertiesWriter.WriteRetainAvailable(packet.Properties.RetainAvailable);
        propertiesWriter.WriteReceiveMaximum(packet.Properties.ReceiveMaximum);
        propertiesWriter.WriteAssignedClientIdentifier(packet.Properties.AssignedClientIdentifier);
        propertiesWriter.WriteTopicAliasMaximum(packet.Properties.TopicAliasMaximum);
        propertiesWriter.WriteReasonString(packet.Properties.ReasonString);
        propertiesWriter.WriteMaximumPacketSize(packet.Properties.MaximumPacketSize);
        propertiesWriter.WriteWildcardSubscriptionAvailable(packet.Properties.WildcardSubscriptionAvailable);
        propertiesWriter.WriteSubscriptionIdentifiersAvailable(packet.Properties.SubscriptionIdentifiersAvailable);
        propertiesWriter.WriteSharedSubscriptionAvailable(packet.Properties.SharedSubscriptionAvailable);
        propertiesWriter.WriteServerKeepAlive(packet.Properties.ServerKeepAlive);
        propertiesWriter.WriteResponseInformation(packet.Properties.ResponseInformation);
        propertiesWriter.WriteServerReference(packet.Properties.ServerReference);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      propertiesWriter.WriteTo(packetWriter);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.ConnAck);
    }

    private static byte EncodePublishPacket(
      MqttPublishPacket packet,
      IMqttPacketWriter packetWriter)
    {
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      if (packetWriter == null)
        throw new ArgumentNullException(nameof (packetWriter));
      if (packet.QualityOfServiceLevel == MqttQualityOfServiceLevel.AtMostOnce && packet.Dup)
        throw new MqttProtocolViolationException("Dup flag must be false for QoS 0 packets [MQTT-3.3.1-2].");
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
          throw new MqttProtocolViolationException("Packet identifier must be 0 if QoS == 0 [MQTT-2.3.1-5].");
      }
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WritePayloadFormatIndicator(packet.Properties.PayloadFormatIndicator);
        propertiesWriter.WriteMessageExpiryInterval(packet.Properties.MessageExpiryInterval);
        propertiesWriter.WriteTopicAlias(packet.Properties.TopicAlias);
        propertiesWriter.WriteResponseTopic(packet.Properties.ResponseTopic);
        propertiesWriter.WriteCorrelationData(packet.Properties.CorrelationData);
        propertiesWriter.WriteSubscriptionIdentifiers(packet.Properties.SubscriptionIdentifiers);
        propertiesWriter.WriteContentType(packet.Properties.ContentType);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      propertiesWriter.WriteTo(packetWriter);
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
      if (packet == null)
        throw new ArgumentNullException(nameof (packet));
      if (packetWriter == null)
        throw new ArgumentNullException(nameof (packetWriter));
      if (!packet.PacketIdentifier.HasValue)
        throw new MqttProtocolViolationException("PubAck packet has no packet identifier.");
      if (!packet.ReasonCode.HasValue)
        throw new MqttProtocolViolationException("PubAck packet must contain a reason code.");
      packetWriter.Write(packet.PacketIdentifier.Value);
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteReasonString(packet.Properties.ReasonString);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      if (packetWriter.Length > 0 || packet.ReasonCode.Value != MqttPubAckReasonCode.Success)
      {
        packetWriter.Write((byte) packet.ReasonCode.Value);
        propertiesWriter.WriteTo(packetWriter);
      }
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PubAck);
    }

    private static byte EncodePubRecPacket(MqttPubRecPacket packet, IMqttPacketWriter packetWriter)
    {
      ThrowIfPacketIdentifierIsInvalid(packet);
      if (!packet.ReasonCode.HasValue)
        ThrowReasonCodeNotSetException();
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteReasonString(packet.Properties.ReasonString);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      packetWriter.Write(packet.PacketIdentifier.Value);
      if (packetWriter.Length > 0 || packet.ReasonCode.Value != MqttPubRecReasonCode.Success)
      {
        packetWriter.Write((byte) packet.ReasonCode.Value);
        propertiesWriter.WriteTo(packetWriter);
      }
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PubRec);
    }

    private static byte EncodePubRelPacket(MqttPubRelPacket packet, IMqttPacketWriter packetWriter)
    {
      ThrowIfPacketIdentifierIsInvalid(packet);
      if (!packet.ReasonCode.HasValue)
        ThrowReasonCodeNotSetException();
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteReasonString(packet.Properties.ReasonString);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      packetWriter.Write(packet.PacketIdentifier.Value);
      if (propertiesWriter.Length > 0 || packet.ReasonCode.Value != MqttPubRelReasonCode.Success)
      {
        packetWriter.Write((byte) packet.ReasonCode.Value);
        propertiesWriter.WriteTo(packetWriter);
      }
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PubRel, 2);
    }

    private static byte EncodePubCompPacket(
      MqttPubCompPacket packet,
      IMqttPacketWriter packetWriter)
    {
      ThrowIfPacketIdentifierIsInvalid(packet);
      if (!packet.ReasonCode.HasValue)
        ThrowReasonCodeNotSetException();
      packetWriter.Write(packet.PacketIdentifier.Value);
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteReasonString(packet.Properties.ReasonString);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      if (propertiesWriter.Length > 0 || packet.ReasonCode.Value != MqttPubCompReasonCode.Success)
      {
        packetWriter.Write((byte) packet.ReasonCode.Value);
        propertiesWriter.WriteTo(packetWriter);
      }
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PubComp);
    }

    private static byte EncodeSubscribePacket(
      MqttSubscribePacket packet,
      IMqttPacketWriter packetWriter)
    {
      var topicFilters1 = packet.TopicFilters;
      if ((topicFilters1 != null ? (!topicFilters1.Any() ? 1 : 0) : 1) != 0)
        throw new MqttProtocolViolationException("At least one topic filter must be set [MQTT-3.8.3-3].");
      ThrowIfPacketIdentifierIsInvalid(packet);
      packetWriter.Write(packet.PacketIdentifier.Value);
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteSubscriptionIdentifier(packet.Properties.SubscriptionIdentifier);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      propertiesWriter.WriteTo(packetWriter);
      var topicFilters2 = packet.TopicFilters;
      // ISSUE: explicit non-virtual call
      if ((topicFilters2 != null ? ( (topicFilters2.Count) > 0 ? 1 : 0) : 0) != 0)
      {
        foreach (var topicFilter in packet.TopicFilters)
        {
          packetWriter.WriteWithLengthPrefix(topicFilter.Topic);
          var num1 = (byte) topicFilter.QualityOfServiceLevel;
          var noLocal = topicFilter.NoLocal;
          var flag1 = true;
          if (noLocal.GetValueOrDefault() == flag1 & noLocal.HasValue)
            num1 |= 4;
          var retainAsPublished = topicFilter.RetainAsPublished;
          var flag2 = true;
          if (retainAsPublished.GetValueOrDefault() == flag2 & retainAsPublished.HasValue)
            num1 |= 8;
          var retainHandling = topicFilter.RetainHandling;
          if (retainHandling.HasValue)
          {
            int num2 = num1;
            retainHandling = topicFilter.RetainHandling;
            int num3 = (byte) ((uint) (byte) retainHandling.Value << 4);
            num1 = (byte) (num2 | num3);
          }
          packetWriter.Write(num1);
        }
      }
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.Subscribe, 2);
    }

    private static byte EncodeSubAckPacket(MqttSubAckPacket packet, IMqttPacketWriter packetWriter)
    {
      var reasonCodes = packet.ReasonCodes;
      if ((reasonCodes != null ? (!reasonCodes.Any() ? 1 : 0) : 1) != 0)
        throw new MqttProtocolViolationException("At least one reason code must be set[MQTT - 3.8.3 - 3].");
      ThrowIfPacketIdentifierIsInvalid(packet);
      packetWriter.Write(packet.PacketIdentifier.Value);
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteReasonString(packet.Properties.ReasonString);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      propertiesWriter.WriteTo(packetWriter);
      foreach (var reasonCode in packet.ReasonCodes)
        packetWriter.Write((byte) reasonCode);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.SubAck);
    }

    private static byte EncodeUnsubscribePacket(
      MqttUnsubscribePacket packet,
      IMqttPacketWriter packetWriter)
    {
      var topicFilters = packet.TopicFilters;
      if ((topicFilters != null ? (!topicFilters.Any() ? 1 : 0) : 1) != 0)
        throw new MqttProtocolViolationException("At least one topic filter must be set [MQTT-3.10.3-2].");
      ThrowIfPacketIdentifierIsInvalid(packet);
      packetWriter.Write(packet.PacketIdentifier.Value);
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      propertiesWriter.WriteTo(packetWriter);
      foreach (var topicFilter in packet.TopicFilters)
        packetWriter.WriteWithLengthPrefix(topicFilter);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.Unsubscibe, 2);
    }

    private static byte EncodeUnsubAckPacket(
      MqttUnsubAckPacket packet,
      IMqttPacketWriter packetWriter)
    {
      var reasonCodes = packet.ReasonCodes;
      if ((reasonCodes != null ? (!reasonCodes.Any() ? 1 : 0) : 1) != 0)
        throw new MqttProtocolViolationException("At least one reason code must be set[MQTT - 3.8.3 - 3].");
      ThrowIfPacketIdentifierIsInvalid(packet);
      packetWriter.Write(packet.PacketIdentifier.Value);
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteReasonString(packet.Properties.ReasonString);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      propertiesWriter.WriteTo(packetWriter);
      foreach (var reasonCode in packet.ReasonCodes)
        packetWriter.Write((byte) reasonCode);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.UnsubAck);
    }

    private static byte EncodeDisconnectPacket(
      MqttDisconnectPacket packet,
      IMqttPacketWriter packetWriter)
    {
      if (!packet.ReasonCode.HasValue)
        ThrowReasonCodeNotSetException();
      packetWriter.Write((byte) packet.ReasonCode.Value);
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteServerReference(packet.Properties.ServerReference);
        propertiesWriter.WriteReasonString(packet.Properties.ReasonString);
        propertiesWriter.WriteSessionExpiryInterval(packet.Properties.SessionExpiryInterval);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      propertiesWriter.WriteTo(packetWriter);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.Disconnect);
    }

    private static byte EncodePingReqPacket() => MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PingReq);

    private static byte EncodePingRespPacket() => MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.PingResp);

    private static byte EncodeAuthPacket(MqttAuthPacket packet, IMqttPacketWriter packetWriter)
    {
      packetWriter.Write((byte) packet.ReasonCode);
      var propertiesWriter = new MqttV500PropertiesWriter();
      if (packet.Properties != null)
      {
        propertiesWriter.WriteAuthenticationMethod(packet.Properties.AuthenticationMethod);
        propertiesWriter.WriteAuthenticationData(packet.Properties.AuthenticationData);
        propertiesWriter.WriteReasonString(packet.Properties.ReasonString);
        propertiesWriter.WriteUserProperties(packet.Properties.UserProperties);
      }
      propertiesWriter.WriteTo(packetWriter);
      return MqttPacketWriter.BuildFixedHeader(MqttControlPacketType.Auth);
    }

    private static void ThrowReasonCodeNotSetException() => throw new MqttProtocolViolationException("The ReasonCode must be set for MQTT version 5.");

    private static void ThrowIfPacketIdentifierIsInvalid(IMqttPacketWithIdentifier packet)
    {
      if (!packet.PacketIdentifier.HasValue)
        throw new MqttProtocolViolationException("Packet identifier is not set for " + packet.GetType().Name + ".");
    }
  }
}
