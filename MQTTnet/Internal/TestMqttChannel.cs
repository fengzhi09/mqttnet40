// Decompiled with JetBrains decompiler
// Type: MQTTnet.Internal.TestMqttChannel
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Channel;

namespace MQTTnet.Internal
{
  public class TestMqttChannel : IMqttChannel, IDisposable
  {
    private readonly MemoryStream _stream;

    public TestMqttChannel(MemoryStream stream) => _stream = stream;

    public string Endpoint { get; } = "<Test channel>";

    public bool IsSecureConnection { get; }

    public X509Certificate2 ClientCertificate { get; }

    public Task ConnectAsync(CancellationToken cancellationToken) => TaskExtension.FromResult(0);

    public Task DisconnectAsync(CancellationToken cancellationToken) => TaskExtension.FromResult(0);

    public Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return _stream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return _stream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public void Dispose()
    {
    }
  }
}
