// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Disconnecting.MqttClientDisconnectedHandlerDelegate
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;

namespace MQTTnet.Client.Disconnecting
{
  public class MqttClientDisconnectedHandlerDelegate : IMqttClientDisconnectedHandler
  {
    private readonly Func<MqttClientDisconnectedEventArgs, Task> _handler;

    public MqttClientDisconnectedHandlerDelegate(Action<MqttClientDisconnectedEventArgs> handler) => _handler = handler != null ? (Func<MqttClientDisconnectedEventArgs, Task>) (context =>
    {
      handler(context);
      return (Task) TaskExtension.FromResult(0);
    }) : throw new ArgumentNullException(nameof (handler));

    public MqttClientDisconnectedHandlerDelegate(
      Func<MqttClientDisconnectedEventArgs, Task> handler)
    {
      _handler = handler ?? throw new ArgumentNullException(nameof (handler));
    }

    public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs) => _handler(eventArgs);
  }
}
