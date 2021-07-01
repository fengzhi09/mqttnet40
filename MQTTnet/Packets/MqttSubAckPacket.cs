// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttSubAckPacket
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;
using System.Linq;
using MQTTnet.Protocol;

namespace MQTTnet.Packets
{
  public class MqttSubAckPacket : MqttBasePacket, IMqttPacketWithIdentifier
  {
    public ushort? PacketIdentifier { get; set; }

    public List<MqttSubscribeReturnCode> ReturnCodes { get; set; } = new List<MqttSubscribeReturnCode>();

    public List<MqttSubscribeReasonCode> ReasonCodes { get; } = new List<MqttSubscribeReasonCode>();

    public MqttSubAckPacketProperties Properties { get; set; }

    public override string ToString() => "SubAck: [PacketIdentifier=" + PacketIdentifier + "] [ReturnCodes=" + string.Join(",", ReturnCodes.Select(f => f.ToString())) + "] [ReasonCode=" + string.Join(",", ReasonCodes.Select(f => f.ToString())) + "]";
  }
}
