// Decompiled with JetBrains decompiler
// Type: MQTTnet.Adapter.ReceivedMqttPacket
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Formatter;

namespace MQTTnet.Adapter
{
  public class ReceivedMqttPacket
  {
    public ReceivedMqttPacket(byte fixedHeader, IMqttPacketBodyReader body, int totalLength)
    {
      FixedHeader = fixedHeader;
      Body = body;
      TotalLength = totalLength;
    }

    public byte FixedHeader { get; set; }

    public IMqttPacketBodyReader Body { get; }

    public int TotalLength { get; }
  }
}
