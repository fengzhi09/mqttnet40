// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Connecting.MqttClientConnectedHandlerDelegate
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;

namespace MQTTnet.Client.Connecting
{
  public class MqttClientConnectedHandlerDelegate : IMqttClientConnectedHandler
  {
    private readonly Func<MqttClientConnectedEventArgs, Task> _handler;

    public MqttClientConnectedHandlerDelegate(Action<MqttClientConnectedEventArgs> handler) => _handler = handler != null ? (Func<MqttClientConnectedEventArgs, Task>) (context =>
    {
      handler(context);
      return (Task) TaskExtension.FromResult(0);
    }) : throw new ArgumentNullException(nameof (handler));

    public MqttClientConnectedHandlerDelegate(Func<MqttClientConnectedEventArgs, Task> handler) => _handler = handler ?? throw new ArgumentNullException(nameof (handler));

    public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs) => _handler(eventArgs);
  }
}
