// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerClientConnectedHandlerDelegate
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;

namespace MQTTnet.Server
{
  public class MqttServerClientConnectedHandlerDelegate : IMqttServerClientConnectedHandler
  {
    private readonly Func<MqttServerClientConnectedEventArgs, Task> _handler;

    public MqttServerClientConnectedHandlerDelegate(
      Action<MqttServerClientConnectedEventArgs> handler)
    {
      _handler = handler != null ? (Func<MqttServerClientConnectedEventArgs, Task>) (eventArgs =>
      {
        handler(eventArgs);
        return (Task) TaskExtension.FromResult(0);
      }) : throw new ArgumentNullException(nameof (handler));
    }

    public MqttServerClientConnectedHandlerDelegate(
      Func<MqttServerClientConnectedEventArgs, Task> handler)
    {
      _handler = handler ?? throw new ArgumentNullException(nameof (handler));
    }

    public Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs) => _handler(eventArgs);
  }
}
