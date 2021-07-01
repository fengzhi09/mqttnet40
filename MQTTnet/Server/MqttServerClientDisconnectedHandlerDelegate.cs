// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerClientDisconnectedHandlerDelegate
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;

namespace MQTTnet.Server
{
  public class MqttServerClientDisconnectedHandlerDelegate : IMqttServerClientDisconnectedHandler
  {
    private readonly Func<MqttServerClientDisconnectedEventArgs, Task> _handler;

    public MqttServerClientDisconnectedHandlerDelegate(
      Action<MqttServerClientDisconnectedEventArgs> handler)
    {
      _handler = handler != null ? (Func<MqttServerClientDisconnectedEventArgs, Task>) (eventArgs =>
      {
        handler(eventArgs);
        return (Task) TaskExtension.FromResult(0);
      }) : throw new ArgumentNullException(nameof (handler));
    }

    public MqttServerClientDisconnectedHandlerDelegate(
      Func<MqttServerClientDisconnectedEventArgs, Task> handler)
    {
      _handler = handler ?? throw new ArgumentNullException(nameof (handler));
    }

    public Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs) => _handler(eventArgs);
  }
}
