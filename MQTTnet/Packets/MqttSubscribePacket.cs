// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttSubscribePacket
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;
using System.Linq;

namespace MQTTnet.Packets
{
  public class MqttSubscribePacket : MqttBasePacket, IMqttPacketWithIdentifier
  {
    public ushort? PacketIdentifier { get; set; }

    public List<MqttTopicFilter> TopicFilters { get; set; } = new List<MqttTopicFilter>();

    public MqttSubscribePacketProperties Properties { get; set; }

    public override string ToString() => "Subscribe: [PacketIdentifier=" + PacketIdentifier + "] [TopicFilters=" + string.Join(",", TopicFilters.Select(f => f.Topic + "@" + f.QualityOfServiceLevel)) + "]";
  }
}
