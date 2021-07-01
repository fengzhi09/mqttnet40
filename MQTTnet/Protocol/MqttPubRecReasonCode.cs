// Decompiled with JetBrains decompiler
// Type: MQTTnet.Protocol.MqttPubRecReasonCode
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Protocol
{
  public enum MqttPubRecReasonCode
  {
    Success = 0,
    NoMatchingSubscribers = 16, // 0x00000010
    UnspecifiedError = 128, // 0x00000080
    ImplementationSpecificError = 131, // 0x00000083
    NotAuthorized = 135, // 0x00000087
    TopicNameInvalid = 144, // 0x00000090
    PacketIdentifierInUse = 145, // 0x00000091
    QuotaExceeded = 151, // 0x00000097
    PayloadFormatInvalid = 153, // 0x00000099
  }
}
