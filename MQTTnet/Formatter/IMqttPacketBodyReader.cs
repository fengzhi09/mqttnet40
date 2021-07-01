// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.IMqttPacketBodyReader
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Formatter
{
  public interface IMqttPacketBodyReader
  {
    int Length { get; }

    int Offset { get; }

    bool EndOfStream { get; }

    byte ReadByte();

    byte[] ReadRemainingData();

    ushort ReadTwoByteInteger();

    string ReadStringWithLengthPrefix();

    byte[] ReadWithLengthPrefix();

    uint ReadFourByteInteger();

    uint ReadVariableLengthInteger();

    bool ReadBoolean();

    void Seek(int position);
  }
}
