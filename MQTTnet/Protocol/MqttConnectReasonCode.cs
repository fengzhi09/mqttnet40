// Decompiled with JetBrains decompiler
// Type: MQTTnet.Protocol.MqttConnectReasonCode
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Protocol
{
  public enum MqttConnectReasonCode
  {
    Success = 0,
    UnspecifiedError = 128, // 0x00000080
    MalformedPacket = 129, // 0x00000081
    ProtocolError = 130, // 0x00000082
    ImplementationSpecificError = 131, // 0x00000083
    UnsupportedProtocolVersion = 132, // 0x00000084
    ClientIdentifierNotValid = 133, // 0x00000085
    BadUserNameOrPassword = 134, // 0x00000086
    NotAuthorized = 135, // 0x00000087
    ServerUnavailable = 136, // 0x00000088
    ServerBusy = 137, // 0x00000089
    Banned = 138, // 0x0000008A
    BadAuthenticationMethod = 140, // 0x0000008C
    TopicNameInvalid = 144, // 0x00000090
    PacketTooLarge = 149, // 0x00000095
    QuotaExceeded = 151, // 0x00000097
    PayloadFormatInvalid = 153, // 0x00000099
    RetainNotSupported = 154, // 0x0000009A
    QoSNotSupported = 155, // 0x0000009B
    UseAnotherServer = 156, // 0x0000009C
    ServerMoved = 157, // 0x0000009D
    ConnectionRateExceeded = 159, // 0x0000009F
  }
}
