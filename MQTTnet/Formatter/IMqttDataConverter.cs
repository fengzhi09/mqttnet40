// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.IMqttDataConverter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;
using MQTTnet.Packets;
using MQTTnet.Server;
using MqttClientSubscribeResult = MQTTnet.Client.Subscribing.MqttClientSubscribeResult;

namespace MQTTnet.Formatter
{
  public interface IMqttDataConverter
  {
    MqttPublishPacket CreatePublishPacket(MqttApplicationMessage applicationMessage);

    MqttPubAckPacket CreatePubAckPacket(MqttPublishPacket publishPacket);

    MqttApplicationMessage CreateApplicationMessage(
      MqttPublishPacket publishPacket);

    MqttClientAuthenticateResult CreateClientConnectResult(
      MqttConnAckPacket connAckPacket);

    MqttConnectPacket CreateConnectPacket(
      MqttApplicationMessage willApplicationMessage,
      IMqttClientOptions options);

    MqttConnAckPacket CreateConnAckPacket(
      MqttConnectionValidatorContext connectionValidatorContext);

    MqttClientSubscribeResult CreateClientSubscribeResult(
      MqttSubscribePacket subscribePacket,
      MqttSubAckPacket subAckPacket);

    MqttClientUnsubscribeResult CreateClientUnsubscribeResult(
      MqttUnsubscribePacket unsubscribePacket,
      MqttUnsubAckPacket unsubAckPacket);

    MqttSubscribePacket CreateSubscribePacket(MqttClientSubscribeOptions options);

    MqttUnsubscribePacket CreateUnsubscribePacket(
      MqttClientUnsubscribeOptions options);

    MqttDisconnectPacket CreateDisconnectPacket(
      MqttClientDisconnectOptions options);

    MqttClientPublishResult CreatePublishResult(MqttPubAckPacket pubAckPacket);

    MqttClientPublishResult CreatePublishResult(
      MqttPubRecPacket pubRecPacket,
      MqttPubCompPacket pubCompPacket);
  }
}
