// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerSubscriptionInterceptorDelegate
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;

namespace MQTTnet.Server
{
  public class MqttServerSubscriptionInterceptorDelegate : IMqttServerSubscriptionInterceptor
  {
    private readonly Func<MqttSubscriptionInterceptorContext, Task> _callback;

    public MqttServerSubscriptionInterceptorDelegate(
      Action<MqttSubscriptionInterceptorContext> callback)
    {
      _callback = callback != null ? (Func<MqttSubscriptionInterceptorContext, Task>) (context =>
      {
        callback(context);
        return (Task) TaskExtension.FromResult(0);
      }) : throw new ArgumentNullException(nameof (callback));
    }

    public MqttServerSubscriptionInterceptorDelegate(
      Func<MqttSubscriptionInterceptorContext, Task> callback)
    {
      _callback = callback ?? throw new ArgumentNullException(nameof (callback));
    }

    public Task InterceptSubscriptionAsync(MqttSubscriptionInterceptorContext context) => _callback(context);
  }
}
