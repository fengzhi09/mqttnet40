// Decompiled with JetBrains decompiler
// Type: MQTTnet.Diagnostics.IMqttNetLogger
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Diagnostics
{
  public interface IMqttNetLogger
  {
    event EventHandler<MqttNetLogMessagePublishedEventArgs> LogMessagePublished;

    IMqttNetScopedLogger CreateScopedLogger(string source);

    void Publish(
      MqttNetLogLevel logLevel,
      string source,
      string message,
      object[] parameters,
      Exception exception);
  }
}
