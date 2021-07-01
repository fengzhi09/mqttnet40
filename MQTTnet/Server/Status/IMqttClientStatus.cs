// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.Status.IMqttClientStatus
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;
using MQTTnet.Formatter;

namespace MQTTnet.Server.Status
{
  public interface IMqttClientStatus
  {
    string ClientId { get; }

    string Endpoint { get; }

    MqttProtocolVersion ProtocolVersion { get; }

    DateTime LastPacketReceivedTimestamp { get; }

    DateTime LastNonKeepAlivePacketReceivedTimestamp { get; }

    long ReceivedApplicationMessagesCount { get; }

    long SentApplicationMessagesCount { get; }

    long ReceivedPacketsCount { get; }

    long SentPacketsCount { get; }

    IMqttSessionStatus Session { get; }

    long BytesSent { get; }

    long BytesReceived { get; }

    Task DisconnectAsync();

    void ResetStatistics();
  }
}
