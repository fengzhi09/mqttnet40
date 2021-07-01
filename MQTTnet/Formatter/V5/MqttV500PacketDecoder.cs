// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.V5.MqttV500PacketDecoder
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using MQTTnet.Adapter;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Formatter.V5
{
  public class MqttV500PacketDecoder
  {
    private static readonly MqttPingReqPacket PingReqPacket = new MqttPingReqPacket();
    private static readonly MqttPingRespPacket PingRespPacket = new MqttPingRespPacket();

    public MqttBasePacket Decode(ReceivedMqttPacket receivedMqttPacket)
    {
      if (receivedMqttPacket == null)
        throw new ArgumentNullException(nameof (receivedMqttPacket));
      var num = receivedMqttPacket.FixedHeader >> 4;
      if (num < 1 || num > 15)
        throw new MqttProtocolViolationException(string.Format("The packet type is invalid ({0}).", num));
      switch (num)
      {
        case 1:
          return DecodeConnectPacket(receivedMqttPacket.Body);
        case 2:
          return DecodeConnAckPacket(receivedMqttPacket.Body);
        case 3:
          return DecodePublishPacket(receivedMqttPacket.FixedHeader, receivedMqttPacket.Body);
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
          return DecodePingReqPacket();
        case 13:
          return DecodePingRespPacket();
        case 14:
          return DecodeDisconnectPacket(receivedMqttPacket.Body);
        case 15:
          return DecodeAuthPacket(receivedMqttPacket.Body);
        default:
          throw new MqttProtocolViolationException(string.Format("Packet type ({0}) not supported.", num));
      }
    }

    private static MqttBasePacket DecodeConnectPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttConnectPacket = new MqttConnectPacket();
      var str = body.ReadStringWithLengthPrefix();
      var num1 = body.ReadByte();
      if (str != "MQTT" && num1 != 5)
        throw new MqttProtocolViolationException("MQTT protocol name and version do not match MQTT v5.");
      int num2 = body.ReadByte();
      var flag1 = (num2 & 2) > 0;
      var flag2 = (num2 & 4) > 0;
      var num3 = (byte) (num2 >> 3 & 3);
      var flag3 = (num2 & 32) > 0;
      var flag4 = (num2 & 64) > 0;
      var flag5 = (num2 & 128) > 0;
      mqttConnectPacket.CleanSession = flag1;
      if (flag2)
        mqttConnectPacket.WillMessage = new MqttApplicationMessage
        {
          QualityOfServiceLevel = (MqttQualityOfServiceLevel) num3,
          Retain = flag3
        };
      mqttConnectPacket.KeepAlivePeriod = body.ReadTwoByteInteger();
      var propertiesReader1 = new MqttV500PropertiesReader(body);
      while (propertiesReader1.MoveNext())
      {
        if (mqttConnectPacket.Properties == null)
          mqttConnectPacket.Properties = new MqttConnectPacketProperties();
        if (propertiesReader1.CurrentPropertyId == MqttPropertyId.SessionExpiryInterval)
          mqttConnectPacket.Properties.SessionExpiryInterval = propertiesReader1.ReadSessionExpiryInterval();
        else if (propertiesReader1.CurrentPropertyId == MqttPropertyId.AuthenticationMethod)
          mqttConnectPacket.Properties.AuthenticationMethod = propertiesReader1.ReadAuthenticationMethod();
        else if (propertiesReader1.CurrentPropertyId == MqttPropertyId.AuthenticationData)
          mqttConnectPacket.Properties.AuthenticationData = propertiesReader1.ReadAuthenticationData();
        else if (propertiesReader1.CurrentPropertyId == MqttPropertyId.ReceiveMaximum)
          mqttConnectPacket.Properties.ReceiveMaximum = propertiesReader1.ReadReceiveMaximum();
        else if (propertiesReader1.CurrentPropertyId == MqttPropertyId.TopicAliasMaximum)
          mqttConnectPacket.Properties.TopicAliasMaximum = propertiesReader1.ReadTopicAliasMaximum();
        else if (propertiesReader1.CurrentPropertyId == MqttPropertyId.MaximumPacketSize)
          mqttConnectPacket.Properties.MaximumPacketSize = propertiesReader1.ReadMaximumPacketSize();
        else if (propertiesReader1.CurrentPropertyId == MqttPropertyId.RequestResponseInformation)
          mqttConnectPacket.Properties.RequestResponseInformation = propertiesReader1.RequestResponseInformation();
        else if (propertiesReader1.CurrentPropertyId == MqttPropertyId.RequestProblemInformation)
          mqttConnectPacket.Properties.RequestProblemInformation = propertiesReader1.RequestProblemInformation();
        else if (propertiesReader1.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttConnectPacket.Properties.UserProperties == null)
            mqttConnectPacket.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader1.AddUserPropertyTo(mqttConnectPacket.Properties.UserProperties);
        }
        else
          propertiesReader1.ThrowInvalidPropertyIdException(typeof (MqttConnectPacket));
      }
      mqttConnectPacket.ClientId = body.ReadStringWithLengthPrefix();
      if (mqttConnectPacket.WillMessage != null)
      {
        var propertiesReader2 = new MqttV500PropertiesReader(body);
        while (propertiesReader2.MoveNext())
        {
          if (propertiesReader2.CurrentPropertyId == MqttPropertyId.PayloadFormatIndicator)
            mqttConnectPacket.WillMessage.PayloadFormatIndicator = propertiesReader1.ReadPayloadFormatIndicator();
          else if (propertiesReader2.CurrentPropertyId == MqttPropertyId.MessageExpiryInterval)
            mqttConnectPacket.WillMessage.MessageExpiryInterval = propertiesReader1.ReadMessageExpiryInterval();
          else if (propertiesReader2.CurrentPropertyId == MqttPropertyId.TopicAlias)
            mqttConnectPacket.WillMessage.TopicAlias = propertiesReader1.ReadTopicAlias();
          else if (propertiesReader2.CurrentPropertyId == MqttPropertyId.ResponseTopic)
            mqttConnectPacket.WillMessage.ResponseTopic = propertiesReader1.ReadResponseTopic();
          else if (propertiesReader2.CurrentPropertyId == MqttPropertyId.CorrelationData)
            mqttConnectPacket.WillMessage.CorrelationData = propertiesReader1.ReadCorrelationData();
          else if (propertiesReader2.CurrentPropertyId == MqttPropertyId.SubscriptionIdentifier)
          {
            if (mqttConnectPacket.WillMessage.SubscriptionIdentifiers == null)
              mqttConnectPacket.WillMessage.SubscriptionIdentifiers = new List<uint>();
            mqttConnectPacket.WillMessage.SubscriptionIdentifiers.Add(propertiesReader1.ReadSubscriptionIdentifier());
          }
          else if (propertiesReader2.CurrentPropertyId == MqttPropertyId.ContentType)
            mqttConnectPacket.WillMessage.ContentType = propertiesReader1.ReadContentType();
          else if (propertiesReader2.CurrentPropertyId == MqttPropertyId.WillDelayInterval)
            mqttConnectPacket.Properties.WillDelayInterval = propertiesReader1.ReadWillDelayInterval();
          else if (propertiesReader2.CurrentPropertyId == MqttPropertyId.UserProperty)
          {
            if (mqttConnectPacket.WillMessage.UserProperties == null)
              mqttConnectPacket.WillMessage.UserProperties = new List<MqttUserProperty>();
            propertiesReader1.AddUserPropertyTo(mqttConnectPacket.Properties.UserProperties);
          }
          else
            propertiesReader1.ThrowInvalidPropertyIdException(typeof (MqttPublishPacket));
        }
        mqttConnectPacket.WillMessage.Topic = body.ReadStringWithLengthPrefix();
        mqttConnectPacket.WillMessage.Payload = body.ReadWithLengthPrefix();
      }
      if (flag5)
        mqttConnectPacket.Username = body.ReadStringWithLengthPrefix();
      if (flag4)
        mqttConnectPacket.Password = body.ReadWithLengthPrefix();
      return mqttConnectPacket;
    }

    private static MqttBasePacket DecodeConnAckPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var num = body.ReadByte();
      var mqttConnAckPacket = new MqttConnAckPacket
      {
        IsSessionPresent = (num & 1) > 0,
        ReasonCode = (MqttConnectReasonCode) body.ReadByte()
      };
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttConnAckPacket.Properties == null)
          mqttConnAckPacket.Properties = new MqttConnAckPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.SessionExpiryInterval)
          mqttConnAckPacket.Properties.SessionExpiryInterval = propertiesReader.ReadSessionExpiryInterval();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.AuthenticationMethod)
          mqttConnAckPacket.Properties.AuthenticationMethod = propertiesReader.ReadAuthenticationMethod();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.AuthenticationData)
          mqttConnAckPacket.Properties.AuthenticationData = propertiesReader.ReadAuthenticationData();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.RetainAvailable)
          mqttConnAckPacket.Properties.RetainAvailable = propertiesReader.ReadRetainAvailable();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReceiveMaximum)
          mqttConnAckPacket.Properties.ReceiveMaximum = propertiesReader.ReadReceiveMaximum();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.AssignedClientIdentifier)
          mqttConnAckPacket.Properties.AssignedClientIdentifier = propertiesReader.ReadAssignedClientIdentifier();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.TopicAliasMaximum)
          mqttConnAckPacket.Properties.TopicAliasMaximum = propertiesReader.ReadTopicAliasMaximum();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReasonString)
          mqttConnAckPacket.Properties.ReasonString = propertiesReader.ReadReasonString();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.MaximumPacketSize)
          mqttConnAckPacket.Properties.MaximumPacketSize = propertiesReader.ReadMaximumPacketSize();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.WildcardSubscriptionAvailable)
          mqttConnAckPacket.Properties.WildcardSubscriptionAvailable = propertiesReader.ReadWildcardSubscriptionAvailable();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.SubscriptionIdentifiersAvailable)
          mqttConnAckPacket.Properties.SubscriptionIdentifiersAvailable = propertiesReader.ReadSubscriptionIdentifiersAvailable();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.SharedSubscriptionAvailable)
          mqttConnAckPacket.Properties.SharedSubscriptionAvailable = propertiesReader.ReadSharedSubscriptionAvailable();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ServerKeepAlive)
          mqttConnAckPacket.Properties.ServerKeepAlive = propertiesReader.ReadServerKeepAlive();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ResponseInformation)
          mqttConnAckPacket.Properties.ResponseInformation = propertiesReader.ReadResponseInformation();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ServerReference)
          mqttConnAckPacket.Properties.ServerReference = propertiesReader.ReadServerReference();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttConnAckPacket.Properties.UserProperties == null)
            mqttConnAckPacket.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttConnAckPacket.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttConnAckPacket));
      }
      return mqttConnAckPacket;
    }

    private static MqttBasePacket DecodeDisconnectPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var disconnectPacket = new MqttDisconnectPacket
      {
        ReasonCode = (MqttDisconnectReasonCode) body.ReadByte()
      };
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (disconnectPacket.Properties == null)
          disconnectPacket.Properties = new MqttDisconnectPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.SessionExpiryInterval)
          disconnectPacket.Properties.SessionExpiryInterval = propertiesReader.ReadSessionExpiryInterval();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReasonString)
          disconnectPacket.Properties.ReasonString = propertiesReader.ReadReasonString();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ServerReference)
          disconnectPacket.Properties.ServerReference = propertiesReader.ReadServerReference();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (disconnectPacket.Properties.UserProperties == null)
            disconnectPacket.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(disconnectPacket.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttDisconnectPacket));
      }
      return disconnectPacket;
    }

    private static MqttBasePacket DecodeSubscribePacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttSubscribePacket = new MqttSubscribePacket
      {
        PacketIdentifier = body.ReadTwoByteInteger()
      };
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttSubscribePacket.Properties == null)
          mqttSubscribePacket.Properties = new MqttSubscribePacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.SubscriptionIdentifier)
          mqttSubscribePacket.Properties.SubscriptionIdentifier = propertiesReader.ReadSubscriptionIdentifier();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttSubscribePacket.Properties.UserProperties == null)
            mqttSubscribePacket.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttSubscribePacket.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttSubscribePacket));
      }
      while (!body.EndOfStream)
      {
        var str = body.ReadStringWithLengthPrefix();
        int num = body.ReadByte();
        var qualityOfServiceLevel = (MqttQualityOfServiceLevel) (num & 3);
        var flag1 = (num & 4) > 0;
        var flag2 = (num & 8) > 0;
        var mqttRetainHandling = (MqttRetainHandling) (num >> 4 & 3);
        mqttSubscribePacket.TopicFilters.Add(new MqttTopicFilter
        {
          Topic = str,
          QualityOfServiceLevel = qualityOfServiceLevel,
          NoLocal = flag1,
          RetainAsPublished = flag2,
          RetainHandling = mqttRetainHandling
        });
      }
      return mqttSubscribePacket;
    }

    private static MqttBasePacket DecodeSubAckPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttSubAckPacket = new MqttSubAckPacket
      {
        PacketIdentifier = body.ReadTwoByteInteger()
      };
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttSubAckPacket.Properties == null)
          mqttSubAckPacket.Properties = new MqttSubAckPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReasonString)
          mqttSubAckPacket.Properties.ReasonString = propertiesReader.ReadReasonString();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttSubAckPacket.Properties.UserProperties == null)
            mqttSubAckPacket.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttSubAckPacket.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttSubAckPacket));
      }
      while (!body.EndOfStream)
      {
        var subscribeReasonCode = (MqttSubscribeReasonCode) body.ReadByte();
        mqttSubAckPacket.ReasonCodes.Add(subscribeReasonCode);
      }
      return mqttSubAckPacket;
    }

    private static MqttBasePacket DecodeUnsubscribePacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var unsubscribePacket = new MqttUnsubscribePacket
      {
        PacketIdentifier = body.ReadTwoByteInteger()
      };
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (unsubscribePacket.Properties == null)
          unsubscribePacket.Properties = new MqttUnsubscribePacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (unsubscribePacket.Properties.UserProperties == null)
            unsubscribePacket.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(unsubscribePacket.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttUnsubscribePacket));
      }
      while (!body.EndOfStream)
        unsubscribePacket.TopicFilters.Add(body.ReadStringWithLengthPrefix());
      return unsubscribePacket;
    }

    private static MqttBasePacket DecodeUnsubAckPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttUnsubAckPacket = new MqttUnsubAckPacket
      {
        PacketIdentifier = body.ReadTwoByteInteger()
      };
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttUnsubAckPacket.Properties == null)
          mqttUnsubAckPacket.Properties = new MqttUnsubAckPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReasonString)
          mqttUnsubAckPacket.Properties.ReasonString = propertiesReader.ReadReasonString();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttUnsubAckPacket.Properties.UserProperties == null)
            mqttUnsubAckPacket.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttUnsubAckPacket.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttUnsubAckPacket));
      }
      while (!body.EndOfStream)
      {
        var unsubscribeReasonCode = (MqttUnsubscribeReasonCode) body.ReadByte();
        mqttUnsubAckPacket.ReasonCodes.Add(unsubscribeReasonCode);
      }
      return mqttUnsubAckPacket;
    }

    private static MqttBasePacket DecodePingReqPacket() => PingReqPacket;

    private static MqttBasePacket DecodePingRespPacket() => PingRespPacket;

    private static MqttBasePacket DecodePublishPacket(
      byte header,
      IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var flag1 = (header & 1) > 0;
      var qualityOfServiceLevel = (MqttQualityOfServiceLevel) (header >> 1 & 3);
      var flag2 = (header >> 3 & 1) > 0;
      var mqttPublishPacket = new MqttPublishPacket
      {
        Topic = body.ReadStringWithLengthPrefix(),
        Retain = flag1,
        QualityOfServiceLevel = qualityOfServiceLevel,
        Dup = flag2
      };
      if (qualityOfServiceLevel > MqttQualityOfServiceLevel.AtMostOnce)
        mqttPublishPacket.PacketIdentifier = body.ReadTwoByteInteger();
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttPublishPacket.Properties == null)
          mqttPublishPacket.Properties = new MqttPublishPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.PayloadFormatIndicator)
          mqttPublishPacket.Properties.PayloadFormatIndicator = propertiesReader.ReadPayloadFormatIndicator();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.MessageExpiryInterval)
          mqttPublishPacket.Properties.MessageExpiryInterval = propertiesReader.ReadMessageExpiryInterval();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.TopicAlias)
          mqttPublishPacket.Properties.TopicAlias = propertiesReader.ReadTopicAlias();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ResponseTopic)
          mqttPublishPacket.Properties.ResponseTopic = propertiesReader.ReadResponseTopic();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.CorrelationData)
          mqttPublishPacket.Properties.CorrelationData = propertiesReader.ReadCorrelationData();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.SubscriptionIdentifier)
        {
          if (mqttPublishPacket.Properties.SubscriptionIdentifiers == null)
            mqttPublishPacket.Properties.SubscriptionIdentifiers = new List<uint>();
          mqttPublishPacket.Properties.SubscriptionIdentifiers.Add(propertiesReader.ReadSubscriptionIdentifier());
        }
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ContentType)
          mqttPublishPacket.Properties.ContentType = propertiesReader.ReadContentType();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttPublishPacket.Properties.UserProperties == null)
            mqttPublishPacket.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttPublishPacket.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttPublishPacket));
      }
      if (!body.EndOfStream)
        mqttPublishPacket.Payload = body.ReadRemainingData();
      return mqttPublishPacket;
    }

    private static MqttBasePacket DecodePubAckPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttPubAckPacket1 = new MqttPubAckPacket();
      mqttPubAckPacket1.PacketIdentifier = body.ReadTwoByteInteger();
      var mqttPubAckPacket2 = mqttPubAckPacket1;
      if (body.EndOfStream)
      {
        mqttPubAckPacket2.ReasonCode = MqttPubAckReasonCode.Success;
        return mqttPubAckPacket2;
      }
      mqttPubAckPacket2.ReasonCode = (MqttPubAckReasonCode) body.ReadByte();
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttPubAckPacket2.Properties == null)
          mqttPubAckPacket2.Properties = new MqttPubAckPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReasonString)
          mqttPubAckPacket2.Properties.ReasonString = propertiesReader.ReadReasonString();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttPubAckPacket2.Properties.UserProperties == null)
            mqttPubAckPacket2.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttPubAckPacket2.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttPubAckPacket));
      }
      return mqttPubAckPacket2;
    }

    private static MqttBasePacket DecodePubRecPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttPubRecPacket1 = new MqttPubRecPacket();
      mqttPubRecPacket1.PacketIdentifier = body.ReadTwoByteInteger();
      var mqttPubRecPacket2 = mqttPubRecPacket1;
      if (body.EndOfStream)
      {
        mqttPubRecPacket2.ReasonCode = MqttPubRecReasonCode.Success;
        return mqttPubRecPacket2;
      }
      mqttPubRecPacket2.ReasonCode = (MqttPubRecReasonCode) body.ReadByte();
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttPubRecPacket2.Properties == null)
          mqttPubRecPacket2.Properties = new MqttPubRecPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReasonString)
          mqttPubRecPacket2.Properties.ReasonString = propertiesReader.ReadReasonString();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttPubRecPacket2.Properties.UserProperties == null)
            mqttPubRecPacket2.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttPubRecPacket2.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttPubRecPacket));
      }
      return mqttPubRecPacket2;
    }

    private static MqttBasePacket DecodePubRelPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttPubRelPacket1 = new MqttPubRelPacket();
      mqttPubRelPacket1.PacketIdentifier = body.ReadTwoByteInteger();
      var mqttPubRelPacket2 = mqttPubRelPacket1;
      if (body.EndOfStream)
      {
        mqttPubRelPacket2.ReasonCode = MqttPubRelReasonCode.Success;
        return mqttPubRelPacket2;
      }
      mqttPubRelPacket2.ReasonCode = (MqttPubRelReasonCode) body.ReadByte();
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttPubRelPacket2.Properties == null)
          mqttPubRelPacket2.Properties = new MqttPubRelPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReasonString)
          mqttPubRelPacket2.Properties.ReasonString = propertiesReader.ReadReasonString();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttPubRelPacket2.Properties.UserProperties == null)
            mqttPubRelPacket2.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttPubRelPacket2.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttPubRelPacket));
      }
      return mqttPubRelPacket2;
    }

    private static MqttBasePacket DecodePubCompPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttPubCompPacket1 = new MqttPubCompPacket();
      mqttPubCompPacket1.PacketIdentifier = body.ReadTwoByteInteger();
      var mqttPubCompPacket2 = mqttPubCompPacket1;
      if (body.EndOfStream)
      {
        mqttPubCompPacket2.ReasonCode = MqttPubCompReasonCode.Success;
        return mqttPubCompPacket2;
      }
      mqttPubCompPacket2.ReasonCode = (MqttPubCompReasonCode) body.ReadByte();
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttPubCompPacket2.Properties == null)
          mqttPubCompPacket2.Properties = new MqttPubCompPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReasonString)
          mqttPubCompPacket2.Properties.ReasonString = propertiesReader.ReadReasonString();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttPubCompPacket2.Properties.UserProperties == null)
            mqttPubCompPacket2.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttPubCompPacket2.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttPubCompPacket));
      }
      return mqttPubCompPacket2;
    }

    private static MqttBasePacket DecodeAuthPacket(IMqttPacketBodyReader body)
    {
      ThrowIfBodyIsEmpty(body);
      var mqttAuthPacket = new MqttAuthPacket();
      if (body.EndOfStream)
      {
        mqttAuthPacket.ReasonCode = MqttAuthenticateReasonCode.Success;
        return mqttAuthPacket;
      }
      mqttAuthPacket.ReasonCode = (MqttAuthenticateReasonCode) body.ReadByte();
      var propertiesReader = new MqttV500PropertiesReader(body);
      while (propertiesReader.MoveNext())
      {
        if (mqttAuthPacket.Properties == null)
          mqttAuthPacket.Properties = new MqttAuthPacketProperties();
        if (propertiesReader.CurrentPropertyId == MqttPropertyId.AuthenticationMethod)
          mqttAuthPacket.Properties.AuthenticationMethod = propertiesReader.ReadAuthenticationMethod();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.AuthenticationData)
          mqttAuthPacket.Properties.AuthenticationData = propertiesReader.ReadAuthenticationData();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.ReasonString)
          mqttAuthPacket.Properties.ReasonString = propertiesReader.ReadReasonString();
        else if (propertiesReader.CurrentPropertyId == MqttPropertyId.UserProperty)
        {
          if (mqttAuthPacket.Properties.UserProperties == null)
            mqttAuthPacket.Properties.UserProperties = new List<MqttUserProperty>();
          propertiesReader.AddUserPropertyTo(mqttAuthPacket.Properties.UserProperties);
        }
        else
          propertiesReader.ThrowInvalidPropertyIdException(typeof (MqttAuthPacket));
      }
      return mqttAuthPacket;
    }

    private static void ThrowIfBodyIsEmpty(IMqttPacketBodyReader body)
    {
      if (body == null || body.Length == 0)
        throw new MqttProtocolViolationException("Data from the body is required but not present.");
    }
  }
}
