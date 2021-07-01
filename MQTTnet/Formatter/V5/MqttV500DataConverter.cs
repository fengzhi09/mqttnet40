// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.V5.MqttV500DataConverter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Linq;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MqttClientSubscribeResult = MQTTnet.Client.Subscribing.MqttClientSubscribeResult;

namespace MQTTnet.Formatter.V5
{
  public class MqttV500DataConverter : IMqttDataConverter
  {
    public MqttPublishPacket CreatePublishPacket(
      MqttApplicationMessage applicationMessage)
    {
      if (applicationMessage == null)
        throw new ArgumentNullException(nameof (applicationMessage));
      var mqttPublishPacket = new MqttPublishPacket
      {
        Topic = applicationMessage.Topic,
        Payload = applicationMessage.Payload,
        QualityOfServiceLevel = applicationMessage.QualityOfServiceLevel,
        Retain = applicationMessage.Retain,
        Dup = false,
        Properties = new MqttPublishPacketProperties
        {
          ContentType = applicationMessage.ContentType,
          CorrelationData = applicationMessage.CorrelationData,
          MessageExpiryInterval = applicationMessage.MessageExpiryInterval,
          PayloadFormatIndicator = applicationMessage.PayloadFormatIndicator,
          ResponseTopic = applicationMessage.ResponseTopic,
          SubscriptionIdentifiers = applicationMessage.SubscriptionIdentifiers,
          TopicAlias = applicationMessage.TopicAlias
        }
      };
      if (applicationMessage.UserProperties != null)
      {
        mqttPublishPacket.Properties.UserProperties = new List<MqttUserProperty>();
        mqttPublishPacket.Properties.UserProperties.AddRange(applicationMessage.UserProperties);
      }
      return mqttPublishPacket;
    }

    public MqttPubAckPacket CreatePubAckPacket(MqttPublishPacket publishPacket)
    {
      var mqttPubAckPacket = new MqttPubAckPacket();
      mqttPubAckPacket.PacketIdentifier = publishPacket.PacketIdentifier;
      mqttPubAckPacket.ReasonCode = MqttPubAckReasonCode.Success;
      return mqttPubAckPacket;
    }

    public MqttApplicationMessage CreateApplicationMessage(
      MqttPublishPacket publishPacket)
    {
      return new MqttApplicationMessage
      {
        Topic = publishPacket.Topic,
        Payload = publishPacket.Payload,
        QualityOfServiceLevel = publishPacket.QualityOfServiceLevel,
        Retain = publishPacket.Retain,
        ResponseTopic = publishPacket.Properties?.ResponseTopic,
        ContentType = publishPacket.Properties?.ContentType,
        CorrelationData = publishPacket.Properties?.CorrelationData,
        MessageExpiryInterval = publishPacket.Properties?.MessageExpiryInterval,
        SubscriptionIdentifiers = publishPacket.Properties?.SubscriptionIdentifiers,
        TopicAlias = publishPacket.Properties?.TopicAlias,
        PayloadFormatIndicator = publishPacket.Properties?.PayloadFormatIndicator,
        UserProperties = publishPacket.Properties?.UserProperties ?? new List<MqttUserProperty>()
      };
    }

    public MqttClientAuthenticateResult CreateClientConnectResult(
      MqttConnAckPacket connAckPacket)
    {
      if (connAckPacket == null)
        throw new ArgumentNullException(nameof (connAckPacket));
      return new MqttClientAuthenticateResult
      {
        IsSessionPresent = connAckPacket.IsSessionPresent,
        ResultCode = (MqttClientConnectResultCode) connAckPacket.ReasonCode.Value,
        WildcardSubscriptionAvailable = connAckPacket.Properties?.WildcardSubscriptionAvailable,
        RetainAvailable = connAckPacket.Properties?.RetainAvailable,
        AssignedClientIdentifier = connAckPacket.Properties?.AssignedClientIdentifier,
        AuthenticationMethod = connAckPacket.Properties?.AuthenticationMethod,
        AuthenticationData = connAckPacket.Properties?.AuthenticationData,
        MaximumPacketSize = connAckPacket.Properties?.MaximumPacketSize,
        ReasonString = connAckPacket.Properties?.ReasonString,
        ReceiveMaximum = connAckPacket.Properties?.ReceiveMaximum,
        ResponseInformation = connAckPacket.Properties?.ResponseInformation,
        TopicAliasMaximum = connAckPacket.Properties?.TopicAliasMaximum,
        ServerReference = connAckPacket.Properties?.ServerReference,
        ServerKeepAlive = connAckPacket.Properties?.ServerKeepAlive,
        SessionExpiryInterval = connAckPacket.Properties?.SessionExpiryInterval,
        SubscriptionIdentifiersAvailable = connAckPacket.Properties?.SubscriptionIdentifiersAvailable,
        SharedSubscriptionAvailable = connAckPacket.Properties?.SharedSubscriptionAvailable,
        UserProperties = connAckPacket.Properties?.UserProperties
      };
    }

    public MqttConnectPacket CreateConnectPacket(
      MqttApplicationMessage willApplicationMessage,
      IMqttClientOptions options)
    {
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      return new MqttConnectPacket
      {
        ClientId = options.ClientId,
        Username = options.Credentials?.Username,
        Password = options.Credentials?.Password,
        CleanSession = options.CleanSession,
        KeepAlivePeriod = (ushort) options.KeepAlivePeriod.TotalSeconds,
        WillMessage = willApplicationMessage,
        Properties = new MqttConnectPacketProperties
        {
          AuthenticationMethod = options.AuthenticationMethod,
          AuthenticationData = options.AuthenticationData,
          WillDelayInterval = options.WillDelayInterval,
          MaximumPacketSize = options.MaximumPacketSize,
          ReceiveMaximum = options.ReceiveMaximum,
          RequestProblemInformation = options.RequestProblemInformation,
          RequestResponseInformation = options.RequestResponseInformation,
          SessionExpiryInterval = options.SessionExpiryInterval,
          TopicAliasMaximum = options.TopicAliasMaximum,
          UserProperties = options.UserProperties
        }
      };
    }

    public MqttConnAckPacket CreateConnAckPacket(
      MqttConnectionValidatorContext connectionValidatorContext)
    {
      return new MqttConnAckPacket
      {
        ReasonCode = connectionValidatorContext.ReasonCode,
        Properties = new MqttConnAckPacketProperties
        {
          UserProperties = connectionValidatorContext.ResponseUserProperties,
          AuthenticationMethod = connectionValidatorContext.AuthenticationMethod,
          AuthenticationData = connectionValidatorContext.ResponseAuthenticationData,
          AssignedClientIdentifier = connectionValidatorContext.AssignedClientIdentifier,
          ReasonString = connectionValidatorContext.ReasonString
        }
      };
    }

    public MqttClientSubscribeResult CreateClientSubscribeResult(
      MqttSubscribePacket subscribePacket,
      MqttSubAckPacket subAckPacket)
    {
      if (subscribePacket == null)
        throw new ArgumentNullException(nameof (subscribePacket));
      if (subAckPacket == null)
        throw new ArgumentNullException(nameof (subAckPacket));
      if (subAckPacket.ReasonCodes.Count != subscribePacket.TopicFilters.Count)
        throw new MqttProtocolViolationException("The reason codes are not matching the topic filters [MQTT-3.9.3-1].");
      var clientSubscribeResult = new MqttClientSubscribeResult();
      clientSubscribeResult.Items.AddRange(subscribePacket.TopicFilters.Select((t, i) => new MqttClientSubscribeResultItem(t, (MqttClientSubscribeResultCode) subAckPacket.ReasonCodes[i])));
      return clientSubscribeResult;
    }

    public MqttClientUnsubscribeResult CreateClientUnsubscribeResult(
      MqttUnsubscribePacket unsubscribePacket,
      MqttUnsubAckPacket unsubAckPacket)
    {
      if (unsubscribePacket == null)
        throw new ArgumentNullException(nameof (unsubscribePacket));
      if (unsubAckPacket == null)
        throw new ArgumentNullException(nameof (unsubAckPacket));
      if (unsubAckPacket.ReasonCodes.Count != unsubscribePacket.TopicFilters.Count)
        throw new MqttProtocolViolationException("The return codes are not matching the topic filters [MQTT-3.9.3-1].");
      var unsubscribeResult = new MqttClientUnsubscribeResult();
      unsubscribeResult.Items.AddRange(unsubscribePacket.TopicFilters.Select((t, i) => new MqttClientUnsubscribeResultItem(t, (MqttClientUnsubscribeResultCode) unsubAckPacket.ReasonCodes[i])));
      return unsubscribeResult;
    }

    public MqttSubscribePacket CreateSubscribePacket(
      MqttClientSubscribeOptions options)
    {
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      var mqttSubscribePacket = new MqttSubscribePacket();
      mqttSubscribePacket.Properties = new MqttSubscribePacketProperties();
      mqttSubscribePacket.TopicFilters.AddRange(options.TopicFilters);
      mqttSubscribePacket.Properties.SubscriptionIdentifier = options.SubscriptionIdentifier;
      mqttSubscribePacket.Properties.UserProperties = options.UserProperties;
      return mqttSubscribePacket;
    }

    public MqttUnsubscribePacket CreateUnsubscribePacket(
      MqttClientUnsubscribeOptions options)
    {
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      var unsubscribePacket = new MqttUnsubscribePacket();
      unsubscribePacket.Properties = new MqttUnsubscribePacketProperties();
      unsubscribePacket.TopicFilters.AddRange(options.TopicFilters);
      unsubscribePacket.Properties.UserProperties = options.UserProperties;
      return unsubscribePacket;
    }

    public MqttDisconnectPacket CreateDisconnectPacket(
      MqttClientDisconnectOptions options)
    {
      return new MqttDisconnectPacket
      {
        ReasonCode = options != null ? (MqttDisconnectReasonCode) options.ReasonCode : MqttDisconnectReasonCode.NormalDisconnection
      };
    }

    public MqttClientPublishResult CreatePublishResult(
      MqttPubAckPacket pubAckPacket)
    {
      var clientPublishResult = new MqttClientPublishResult
      {
        ReasonCode = MqttClientPublishReasonCode.Success,
        ReasonString = pubAckPacket?.Properties?.ReasonString,
        UserProperties = pubAckPacket?.Properties?.UserProperties
      };
      if (pubAckPacket != null)
      {
        clientPublishResult.ReasonCode = (MqttClientPublishReasonCode) pubAckPacket.ReasonCode.Value;
        clientPublishResult.PacketIdentifier = pubAckPacket.PacketIdentifier;
      }
      return clientPublishResult;
    }

    public MqttClientPublishResult CreatePublishResult(
      MqttPubRecPacket pubRecPacket,
      MqttPubCompPacket pubCompPacket)
    {
      if (pubRecPacket == null || pubCompPacket == null)
        return new MqttClientPublishResult
        {
          ReasonCode = MqttClientPublishReasonCode.UnspecifiedError
        };
      var reasonCode = pubCompPacket.ReasonCode;
      var pubCompReasonCode = MqttPubCompReasonCode.PacketIdentifierNotFound;
      if (reasonCode.GetValueOrDefault() == pubCompReasonCode & reasonCode.HasValue)
        return new MqttClientPublishResult
        {
          PacketIdentifier = pubCompPacket.PacketIdentifier,
          ReasonCode = MqttClientPublishReasonCode.UnspecifiedError,
          ReasonString = pubCompPacket.Properties?.ReasonString,
          UserProperties = pubCompPacket.Properties?.UserProperties
        };
      var clientPublishResult = new MqttClientPublishResult
      {
        PacketIdentifier = pubCompPacket.PacketIdentifier,
        ReasonCode = MqttClientPublishReasonCode.Success,
        ReasonString = pubCompPacket.Properties?.ReasonString,
        UserProperties = pubCompPacket.Properties?.UserProperties
      };
      if (pubRecPacket.ReasonCode.HasValue)
        clientPublishResult.ReasonCode = (MqttClientPublishReasonCode) pubRecPacket.ReasonCode.Value;
      return clientPublishResult;
    }
  }
}
