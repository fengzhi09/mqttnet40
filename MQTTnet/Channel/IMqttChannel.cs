// Decompiled with JetBrains decompiler
// Type: MQTTnet.Channel.IMqttChannel
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTnet.Channel
{
  public interface IMqttChannel : IDisposable
  {
    string Endpoint { get; }

    bool IsSecureConnection { get; }

    X509Certificate2 ClientCertificate { get; }

    Task ConnectAsync(CancellationToken cancellationToken);

    Task DisconnectAsync(CancellationToken cancellationToken);

    Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken);

    Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
  }
}
