// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.MqttFixedHeader
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Formatter
{
  public struct MqttFixedHeader
  {
    public MqttFixedHeader(byte flags, int remainingLength, int totalLength)
    {
      Flags = flags;
      RemainingLength = remainingLength;
      TotalLength = totalLength;
    }

    public byte Flags { get; }

    public int RemainingLength { get; }

    public int TotalLength { get; }
  }
}
