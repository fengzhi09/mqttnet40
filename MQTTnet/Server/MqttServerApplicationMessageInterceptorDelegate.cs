// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerApplicationMessageInterceptorDelegate
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;

namespace MQTTnet.Server
{
  public class MqttServerApplicationMessageInterceptorDelegate : 
    IMqttServerApplicationMessageInterceptor
  {
    private readonly Func<MqttApplicationMessageInterceptorContext, Task> _callback;

    public MqttServerApplicationMessageInterceptorDelegate(
      Action<MqttApplicationMessageInterceptorContext> callback)
    {
      _callback = callback != null ? (Func<MqttApplicationMessageInterceptorContext, Task>) (context =>
      {
        callback(context);
        return (Task) TaskExtension.FromResult(0);
      }) : throw new ArgumentNullException(nameof (callback));
    }

    public MqttServerApplicationMessageInterceptorDelegate(
      Func<MqttApplicationMessageInterceptorContext, Task> callback)
    {
      _callback = callback ?? throw new ArgumentNullException(nameof (callback));
    }

    public Task InterceptApplicationMessagePublishAsync(
      MqttApplicationMessageInterceptorContext context)
    {
      return _callback(context);
    }
  }
}
