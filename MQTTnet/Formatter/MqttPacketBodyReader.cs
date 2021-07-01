// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.MqttPacketBodyReader
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Linq;
using System.Text;
using MQTTnet.Exceptions;

namespace MQTTnet.Formatter
{
  public class MqttPacketBodyReader : IMqttPacketBodyReader
  {
    private readonly byte[] _buffer;
    private readonly int _initialOffset;
    private readonly int _length;
    private int _offset;

    public MqttPacketBodyReader(byte[] buffer, int offset, int length)
    {
      _buffer = buffer;
      _initialOffset = offset;
      _offset = offset;
      _length = length;
    }

    public int Offset => _offset;

    public int Length => _length - _offset;

    public bool EndOfStream => _offset == _length;

    public void Seek(int position) => _offset = _initialOffset + position;

    public byte ReadByte()
    {
      ValidateReceiveBuffer(1);
      return _buffer[_offset++];
    }

    public bool ReadBoolean()
    {
      ValidateReceiveBuffer(1);
      switch (_buffer[_offset++])
      {
        case 0:
          return false;
        case 1:
          return true;
        default:
          throw new MqttProtocolViolationException("Boolean values can be 0 or 1 only.");
      }
    }

    public byte[] ReadRemainingData()
    {
      var length = _length - _offset;
      var numArray = new byte[length];
      Array.Copy(_buffer, _offset, numArray, 0, length);
      return numArray;
    }

    public ushort ReadTwoByteInteger()
    {
      ValidateReceiveBuffer(2);
      return (ushort) ((uint) (_buffer[_offset++] << 8) | _buffer[_offset++]);
    }

    public uint ReadFourByteInteger()
    {
      ValidateReceiveBuffer(4);
      return (uint) (_buffer[_offset++] << 24 | _buffer[_offset++] << 16 | _buffer[_offset++] << 8) | _buffer[_offset++];
    }

    public uint ReadVariableLengthInteger()
    {
      var num1 = 1;
      uint num2 = 0;
      byte num3;
      do
      {
        num3 = ReadByte();
        num2 += (uint) ((num3 & sbyte.MaxValue) * num1);
        if (num1 > 2097152)
          throw new MqttProtocolViolationException("Variable length integer is invalid.");
        num1 *= 128;
      }
      while ((num3 & 128) != 0);
      return num2;
    }

    public byte[] ReadWithLengthPrefix() => ReadSegmentWithLengthPrefix().Array.ToArray();

    private ArraySegment<byte> ReadSegmentWithLengthPrefix()
    {
      var num = ReadTwoByteInteger();
      ValidateReceiveBuffer(num);
      var arraySegment = new ArraySegment<byte>(_buffer, _offset, num);
      _offset += num;
      return arraySegment;
    }

    private void ValidateReceiveBuffer(int length)
    {
      if (_length < _offset + length)
        throw new MqttProtocolViolationException(string.Format("Expected at least {0} bytes but there are only {1} bytes", _offset + length, _length));
    }

    public string ReadStringWithLengthPrefix()
    {
      var arraySegment = ReadSegmentWithLengthPrefix();
      return Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
    }
  }
}
