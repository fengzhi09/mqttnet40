// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.IMqttPacketWriter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Formatter
{
  public interface IMqttPacketWriter
  {
    int Length { get; }

    void WriteWithLengthPrefix(string value);

    void Write(byte value);

    void WriteWithLengthPrefix(byte[] value);

    void Write(ushort value);

    void Write(IMqttPacketWriter value);

    void WriteVariableLengthInteger(uint value);

    void Write(byte[] value, int offset, int length);

    void Reset(int length);

    void Seek(int offset);

    void FreeBuffer();

    byte[] GetBuffer();
  }
}
