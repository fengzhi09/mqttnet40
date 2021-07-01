// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Unsubscribing.MqttClientUnsubscribeResultCode
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Client.Unsubscribing
{
  public enum MqttClientUnsubscribeResultCode
  {
    Success = 0,
    NoSubscriptionExisted = 17, // 0x00000011
    UnspecifiedError = 128, // 0x00000080
    ImplementationSpecificError = 131, // 0x00000083
    NotAuthorized = 135, // 0x00000087
    TopicFilterInvalid = 143, // 0x0000008F
    PacketIdentifierInUse = 145, // 0x00000091
  }
}
