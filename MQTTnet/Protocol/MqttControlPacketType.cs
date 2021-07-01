// Decompiled with JetBrains decompiler
// Type: MQTTnet.Protocol.MqttControlPacketType
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Protocol
{
  public enum MqttControlPacketType
  {
    Connect = 1,
    ConnAck = 2,
    Publish = 3,
    PubAck = 4,
    PubRec = 5,
    PubRel = 6,
    PubComp = 7,
    Subscribe = 8,
    SubAck = 9,
    Unsubscibe = 10, // 0x0000000A
    UnsubAck = 11, // 0x0000000B
    PingReq = 12, // 0x0000000C
    PingResp = 13, // 0x0000000D
    Disconnect = 14, // 0x0000000E
    Auth = 15, // 0x0000000F
  }
}
