// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.MqttPacketWriter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Text;
using MQTTnet.Protocol;

namespace MQTTnet.Formatter
{
  public sealed class MqttPacketWriter : IMqttPacketWriter
  {
    private static readonly ArraySegment<byte> ZeroVariableLengthIntegerArray = new ArraySegment<byte>(new byte[1], 0, 1);
    private static readonly ArraySegment<byte> ZeroTwoByteIntegerArray = new ArraySegment<byte>(new byte[2], 0, 2);
    public static int InitialBufferSize = 128;
    public static int MaxBufferSize = 4096;
    private byte[] _buffer = new byte[InitialBufferSize];
    private int _offset;

    public int Length { get; private set; }

    public static byte BuildFixedHeader(MqttControlPacketType packetType, byte flags = 0) => (byte) ((uint) packetType << 4 | flags);

    public static int GetLengthOfVariableInteger(uint value)
    {
      var num1 = 0;
      var num2 = value;
      do
      {
        num2 /= 128U;
        ++num1;
      }
      while (num2 > 0U);
      return num1;
    }

    public static ArraySegment<byte> EncodeVariableLengthInteger(uint value)
    {
      if (value == 0U)
        return ZeroVariableLengthIntegerArray;
      if (value <= (uint) sbyte.MaxValue)
        return new ArraySegment<byte>(new byte[1]
        {
          (byte) value
        }, 0, 1);
      var array = new byte[4];
      var count = 0;
      var num1 = value;
      do
      {
        var num2 = num1 % 128U;
        num1 /= 128U;
        if (num1 > 0U)
          num2 |= 128U;
        array[count] = (byte) num2;
        ++count;
      }
      while (num1 > 0U);
      return new ArraySegment<byte>(array, 0, count);
    }

    public void WriteVariableLengthInteger(uint value) => Write(EncodeVariableLengthInteger(value));

    public void WriteWithLengthPrefix(string value)
    {
      if (string.IsNullOrEmpty(value))
        Write(ZeroTwoByteIntegerArray);
      else
        WriteWithLengthPrefix(Encoding.UTF8.GetBytes(value));
    }

    public void WriteWithLengthPrefix(byte[] value)
    {
      if (value == null || value.Length == 0)
      {
        Write(ZeroTwoByteIntegerArray);
      }
      else
      {
        EnsureAdditionalCapacity(value.Length + 2);
        Write((ushort) value.Length);
        Write(value, 0, value.Length);
      }
    }

    public void Write(byte @byte)
    {
      EnsureAdditionalCapacity(1);
      _buffer[_offset] = @byte;
      IncreasePosition(1);
    }

    public void Write(ushort value)
    {
      EnsureAdditionalCapacity(2);
      _buffer[_offset] = (byte) ((uint) value >> 8);
      IncreasePosition(1);
      _buffer[_offset] = (byte) value;
      IncreasePosition(1);
    }

    public void Write(byte[] buffer, int offset, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (count == 0)
        return;
      EnsureAdditionalCapacity(count);
      Array.Copy(buffer, offset, _buffer, _offset, count);
      IncreasePosition(count);
    }

    public void Write(IMqttPacketWriter propertyWriter)
    {
      if (propertyWriter == null)
        throw new ArgumentNullException(nameof (propertyWriter));
      if (!(propertyWriter is MqttPacketWriter mqttPacketWriter))
        throw new InvalidOperationException("propertyWriter must be of type " + typeof (MqttPacketWriter).Name);
      if (mqttPacketWriter.Length == 0)
        return;
      Write(mqttPacketWriter._buffer, 0, mqttPacketWriter.Length);
    }

    public void Reset(int length) => Length = length;

    public void Seek(int position)
    {
      EnsureCapacity(position);
      _offset = position;
    }

    public byte[] GetBuffer() => _buffer;

    public void FreeBuffer()
    {
      if (_buffer.Length < MaxBufferSize)
        return;
      Array.Resize(ref _buffer, MaxBufferSize);
    }

    private void Write(ArraySegment<byte> buffer) => Write(buffer.Array, buffer.Offset, buffer.Count);

    private void EnsureAdditionalCapacity(int additionalCapacity)
    {
      var num = _buffer.Length - _offset;
      if (num >= additionalCapacity)
        return;
      EnsureCapacity(_buffer.Length + additionalCapacity - num);
    }

    private void EnsureCapacity(int capacity)
    {
      var length = _buffer.Length;
      if (length >= capacity)
        return;
      while (length < capacity)
        length *= 2;
      Array.Resize(ref _buffer, length);
    }

    private void IncreasePosition(int length)
    {
      _offset += length;
      if (_offset <= Length)
        return;
      Length = _offset;
    }
  }
}
