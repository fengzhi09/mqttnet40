// Decompiled with JetBrains decompiler
// Type: MQTTnet.Adapter.IMqttChannelAdapter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Formatter;
using MQTTnet.Packets;

namespace MQTTnet.Adapter
{
  public interface IMqttChannelAdapter : IDisposable
  {
    string Endpoint { get; }

    bool IsSecureConnection { get; }

    X509Certificate2 ClientCertificate { get; }

    MqttPacketFormatterAdapter PacketFormatterAdapter { get; }

    long BytesSent { get; }

    long BytesReceived { get; }

    Action ReadingPacketStartedCallback { get; set; }

    Action ReadingPacketCompletedCallback { get; set; }

    Task ConnectAsync(TimeSpan timeout, CancellationToken cancellationToken);

    Task DisconnectAsync(TimeSpan timeout, CancellationToken cancellationToken);

    Task SendPacketAsync(
      MqttBasePacket packet,
      TimeSpan timeout,
      CancellationToken cancellationToken);

    Task<MqttBasePacket> ReceivePacketAsync(
      TimeSpan timeout,
      CancellationToken cancellationToken);

    void ResetStatistics();
  }
}
