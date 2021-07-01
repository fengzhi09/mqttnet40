// Decompiled with JetBrains decompiler
// Type: MQTTnet.Protocol.MqttPropertyId
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Protocol
{
  public enum MqttPropertyId
  {
    PayloadFormatIndicator = 1,
    MessageExpiryInterval = 2,
    ContentType = 3,
    ResponseTopic = 8,
    CorrelationData = 9,
    SubscriptionIdentifier = 11, // 0x0000000B
    SessionExpiryInterval = 17, // 0x00000011
    AssignedClientIdentifier = 18, // 0x00000012
    ServerKeepAlive = 19, // 0x00000013
    AuthenticationMethod = 21, // 0x00000015
    AuthenticationData = 22, // 0x00000016
    RequestProblemInformation = 23, // 0x00000017
    WillDelayInterval = 24, // 0x00000018
    RequestResponseInformation = 25, // 0x00000019
    ResponseInformation = 26, // 0x0000001A
    ServerReference = 28, // 0x0000001C
    ReasonString = 31, // 0x0000001F
    ReceiveMaximum = 33, // 0x00000021
    TopicAliasMaximum = 34, // 0x00000022
    TopicAlias = 35, // 0x00000023
    MaximumQoS = 36, // 0x00000024
    RetainAvailable = 37, // 0x00000025
    UserProperty = 38, // 0x00000026
    MaximumPacketSize = 39, // 0x00000027
    WildcardSubscriptionAvailable = 40, // 0x00000028
    SubscriptionIdentifiersAvailable = 41, // 0x00000029
    SharedSubscriptionAvailable = 42, // 0x0000002A
  }
}
