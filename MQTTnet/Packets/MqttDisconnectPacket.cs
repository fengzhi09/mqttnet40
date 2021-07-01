// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttDisconnectPacket
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Protocol;

namespace MQTTnet.Packets
{
  public class MqttDisconnectPacket : MqttBasePacket
  {
    public MqttDisconnectReasonCode? ReasonCode { get; set; }

    public MqttDisconnectPacketProperties Properties { get; set; }

    public override string ToString() => "Disconnect: [ReasonCode=" + ReasonCode + "]";
  }
}
