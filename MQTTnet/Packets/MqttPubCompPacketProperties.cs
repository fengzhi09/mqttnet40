// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttPubCompPacketProperties
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;

namespace MQTTnet.Packets
{
  public class MqttPubCompPacketProperties
  {
    public string ReasonString { get; set; }

    public List<MqttUserProperty> UserProperties { get; set; }
  }
}
