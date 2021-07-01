// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.MqttPacketReader
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Channel;
using MQTTnet.Exceptions;

namespace MQTTnet.Formatter
{
  public class MqttPacketReader
  {
    private readonly byte[] _singleByteBuffer = new byte[1];
    private readonly IMqttChannel _channel;

    public MqttPacketReader(IMqttChannel channel) => _channel = channel ?? throw new ArgumentNullException(nameof (channel));

    public async Task<ReadFixedHeaderResult> ReadFixedHeaderAsync(
      byte[] fixedHeaderBuffer,
      CancellationToken cancellationToken)
    {
      var buffer = fixedHeaderBuffer;
      int totalBytesRead;
      int num;
      for (totalBytesRead = 0; totalBytesRead < buffer.Length; totalBytesRead += num)
      {
        num = await _channel.ReadAsync(buffer, totalBytesRead, buffer.Length - totalBytesRead, cancellationToken).ConfigureAwait(false);
        if (cancellationToken.IsCancellationRequested)
          return null;
        if (num == 0)
          return new ReadFixedHeaderResult
          {
            ConnectionClosed = true
          };
      }
      if (buffer[1] <= 0)
        return new ReadFixedHeaderResult
        {
          FixedHeader = new MqttFixedHeader(buffer[0], 0, totalBytesRead)
        };
      var nullable = await ReadBodyLengthAsync(buffer[1], cancellationToken).ConfigureAwait(false);
      if (!nullable.HasValue)
        return new ReadFixedHeaderResult
        {
          ConnectionClosed = true
        };
      totalBytesRead += nullable.Value;
      return new ReadFixedHeaderResult
      {
        FixedHeader = new MqttFixedHeader(buffer[0], nullable.Value, totalBytesRead)
      };
    }

    private async Task<int?> ReadBodyLengthAsync(
      byte initialEncodedByte,
      CancellationToken cancellationToken)
    {
      var offset = 0;
      var multiplier = 128;
      var value = initialEncodedByte & sbyte.MaxValue;
      int num1 = initialEncodedByte;
      while ((num1 & 128) != 0)
      {
        ++offset;
        if (offset > 3)
          throw new MqttProtocolViolationException("Remaining length is invalid.");
        if (cancellationToken.IsCancellationRequested)
          return new int?();
        var num2 = await _channel.ReadAsync(_singleByteBuffer, 0, 1, cancellationToken).ConfigureAwait(false);
        if (cancellationToken.IsCancellationRequested)
          return new int?();
        if (num2 == 0)
          return new int?();
        num1 = _singleByteBuffer[0];
        value += (num1 & sbyte.MaxValue) * multiplier;
        multiplier *= 128;
      }
      return value;
    }
  }
}
