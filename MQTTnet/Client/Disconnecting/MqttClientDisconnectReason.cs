// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Disconnecting.MqttClientDisconnectReason
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Client.Disconnecting
{
  public enum MqttClientDisconnectReason
  {
    NormalDisconnection = 0,
    DisconnectWithWillMessage = 4,
    UnspecifiedError = 128, // 0x00000080
    MalformedPacket = 129, // 0x00000081
    ProtocolError = 130, // 0x00000082
    ImplementationSpecificError = 131, // 0x00000083
    NotAuthorized = 135, // 0x00000087
    ServerBusy = 137, // 0x00000089
    ServerShuttingDown = 139, // 0x0000008B
    BadAuthenticationMethod = 140, // 0x0000008C
    KeepaliveTimeout = 141, // 0x0000008D
    SessionTakenOver = 142, // 0x0000008E
    TopicFilterInvalid = 143, // 0x0000008F
    TopicNameInvalid = 144, // 0x00000090
    ReceiveMaximumExceeded = 147, // 0x00000093
    TopicAliasInvalid = 148, // 0x00000094
    PacketTooLarge = 149, // 0x00000095
    MessageRateTooHigh = 150, // 0x00000096
    QuotaExceeded = 151, // 0x00000097
    AdministrativeAction = 152, // 0x00000098
    PayloadFormatInvalid = 153, // 0x00000099
    RetainNotSupported = 154, // 0x0000009A
    QosNotSupported = 155, // 0x0000009B
    UseAnotherServer = 156, // 0x0000009C
    ServerMoved = 157, // 0x0000009D
    SharedSubscriptionsNotSupported = 158, // 0x0000009E
    ConnectionRateExceeded = 159, // 0x0000009F
    MaximumConnectTime = 160, // 0x000000A0
    SubscriptionIdentifiersNotSupported = 161, // 0x000000A1
    WildcardSubscriptionsNotSupported = 162, // 0x000000A2
  }
}
