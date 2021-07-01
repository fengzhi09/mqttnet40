// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttUnsubscribePacket
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;

namespace MQTTnet.Packets
{
  public class MqttUnsubscribePacket : MqttBasePacket, IMqttPacketWithIdentifier
  {
    public ushort? PacketIdentifier { get; set; }

    public List<string> TopicFilters { get; set; } = new List<string>();

    public MqttUnsubscribePacketProperties Properties { get; set; }

    public override string ToString() => "Unsubscribe: [PacketIdentifier=" + PacketIdentifier + "] [TopicFilters=" + string.Join(",", TopicFilters) + "]";
  }
}
