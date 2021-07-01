// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Options.MqttClientTcpOptionsExtensions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Client.Options
{
  public static class MqttClientTcpOptionsExtensions
  {
    public static int GetPort(this MqttClientTcpOptions options)
    {
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      if (options.Port.HasValue)
        return options.Port.Value;
      return options.TlsOptions.UseTls ? 8883 : 1883;
    }
  }
}
