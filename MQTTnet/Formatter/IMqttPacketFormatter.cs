// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.IMqttPacketFormatter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using MQTTnet.Adapter;
using MQTTnet.Packets;

namespace MQTTnet.Formatter
{
  public interface IMqttPacketFormatter
  {
    IMqttDataConverter DataConverter { get; }

    ArraySegment<byte> Encode(MqttBasePacket mqttPacket);

    MqttBasePacket Decode(ReceivedMqttPacket receivedMqttPacket);

    void FreeBuffer();
  }
}
