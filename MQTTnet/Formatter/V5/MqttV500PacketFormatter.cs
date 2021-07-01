// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.V5.MqttV500PacketFormatter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using MQTTnet.Adapter;
using MQTTnet.Packets;

namespace MQTTnet.Formatter.V5
{
  public class MqttV500PacketFormatter : IMqttPacketFormatter
  {
    private readonly MqttV500PacketEncoder _encoder;
    private readonly MqttV500PacketDecoder _decoder = new MqttV500PacketDecoder();

    public MqttV500PacketFormatter() => _encoder = new MqttV500PacketEncoder();

    public MqttV500PacketFormatter(IMqttPacketWriter writer) => _encoder = new MqttV500PacketEncoder(writer);

    public IMqttDataConverter DataConverter { get; } = (IMqttDataConverter) new MqttV500DataConverter();

    public ArraySegment<byte> Encode(MqttBasePacket mqttPacket) => mqttPacket != null ? _encoder.Encode(mqttPacket) : throw new ArgumentNullException(nameof (mqttPacket));

    public MqttBasePacket Decode(ReceivedMqttPacket receivedMqttPacket) => receivedMqttPacket != null ? _decoder.Decode(receivedMqttPacket) : throw new ArgumentNullException(nameof (receivedMqttPacket));

    public void FreeBuffer() => _encoder.FreeBuffer();
  }
}
