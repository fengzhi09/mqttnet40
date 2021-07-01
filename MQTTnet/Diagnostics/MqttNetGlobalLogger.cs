// Decompiled with JetBrains decompiler
// Type: MQTTnet.Diagnostics.MqttNetGlobalLogger
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Diagnostics
{
  public static class MqttNetGlobalLogger
  {
    public static event EventHandler<MqttNetLogMessagePublishedEventArgs> LogMessagePublished;

    public static bool HasListeners => LogMessagePublished != null;

    public static void Publish(MqttNetLogMessage logMessage)
    {
      if (logMessage == null)
        throw new ArgumentNullException(nameof (logMessage));
      var messagePublished = LogMessagePublished;
      if (messagePublished == null)
        return;
      messagePublished(null, new MqttNetLogMessagePublishedEventArgs(logMessage));
    }
  }
}
