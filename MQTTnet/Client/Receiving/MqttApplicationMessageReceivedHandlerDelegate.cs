// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Receiving.MqttApplicationMessageReceivedHandlerDelegate
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;

namespace MQTTnet.Client.Receiving
{
  public class MqttApplicationMessageReceivedHandlerDelegate : IMqttApplicationMessageReceivedHandler
  {
    private readonly Func<MqttApplicationMessageReceivedEventArgs, Task> _handler;

    public MqttApplicationMessageReceivedHandlerDelegate(
      Action<MqttApplicationMessageReceivedEventArgs> handler)
    {
      _handler = handler != null ? (Func<MqttApplicationMessageReceivedEventArgs, Task>) (context =>
      {
        handler(context);
        return (Task) TaskExtension.FromResult(0);
      }) : throw new ArgumentNullException(nameof (handler));
    }

    public MqttApplicationMessageReceivedHandlerDelegate(
      Func<MqttApplicationMessageReceivedEventArgs, Task> handler)
    {
      _handler = handler ?? throw new ArgumentNullException(nameof (handler));
    }

    public Task HandleApplicationMessageReceivedAsync(
      MqttApplicationMessageReceivedEventArgs context)
    {
      return _handler(context);
    }
  }
}
