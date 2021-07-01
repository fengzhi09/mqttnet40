// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttConnectPacket
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Packets
{
  public class MqttConnectPacket : MqttBasePacket
  {
    public string ClientId { get; set; }

    public string Username { get; set; }

    public byte[] Password { get; set; }

    public ushort KeepAlivePeriod { get; set; }

    public bool CleanSession { get; set; }

    public MqttApplicationMessage WillMessage { get; set; }

    public MqttConnectPacketProperties Properties { get; set; }

    public override string ToString()
    {
      var str = string.Empty;
      if (Password != null)
        str = "****";
      return "Connect: [ClientId=" + ClientId + "] [Username=" + Username + "] [Password=" + str + "] [KeepAlivePeriod=" + KeepAlivePeriod + "] [CleanSession=" + CleanSession + "]";
    }
  }
}
