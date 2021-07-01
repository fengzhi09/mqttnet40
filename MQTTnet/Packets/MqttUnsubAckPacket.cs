// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttUnsubAckPacket
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;
using System.Linq;
using MQTTnet.Protocol;

namespace MQTTnet.Packets
{
  public class MqttUnsubAckPacket : MqttBasePacket, IMqttPacketWithIdentifier
  {
    public ushort? PacketIdentifier { get; set; }

    public MqttUnsubAckPacketProperties Properties { get; set; }

    public List<MqttUnsubscribeReasonCode> ReasonCodes { get; set; } = new List<MqttUnsubscribeReasonCode>();

    public override string ToString() => "UnsubAck: [PacketIdentifier=" + PacketIdentifier + "] [ReasonCodes=" + string.Join(",", ReasonCodes.Select(f => f.ToString())) + "]";
  }
}
