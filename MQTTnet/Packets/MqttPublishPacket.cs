// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttPublishPacket
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Protocol;

namespace MQTTnet.Packets
{
  public class MqttPublishPacket : MqttBasePublishPacket
  {
    public bool Retain { get; set; }

    public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

    public bool Dup { get; set; }

    public string Topic { get; set; }

    public byte[] Payload { get; set; }

    public MqttPublishPacketProperties Properties { get; set; }

    public override string ToString() => "Publish: [Topic=" + Topic + "] [Payload.Length=" + Payload?.Length + "] [QoSLevel=" + QualityOfServiceLevel + "] [Dup=" + Dup + "] [Retain=" + Retain + "] [PacketIdentifier=" + PacketIdentifier + "]";
  }
}
