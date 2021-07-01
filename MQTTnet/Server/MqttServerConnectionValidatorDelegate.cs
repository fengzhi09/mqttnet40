// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerConnectionValidatorDelegate
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;

namespace MQTTnet.Server
{
  public class MqttServerConnectionValidatorDelegate : IMqttServerConnectionValidator
  {
    private readonly Func<MqttConnectionValidatorContext, Task> _callback;

    public MqttServerConnectionValidatorDelegate(Action<MqttConnectionValidatorContext> callback) => _callback = callback != null ? (Func<MqttConnectionValidatorContext, Task>) (context =>
    {
      callback(context);
      return (Task) TaskExtension.FromResult(0);
    }) : throw new ArgumentNullException(nameof (callback));

    public MqttServerConnectionValidatorDelegate(
      Func<MqttConnectionValidatorContext, Task> callback)
    {
      _callback = callback ?? throw new ArgumentNullException(nameof (callback));
    }

    public Task ValidateConnectionAsync(MqttConnectionValidatorContext context) => _callback(context);
  }
}
