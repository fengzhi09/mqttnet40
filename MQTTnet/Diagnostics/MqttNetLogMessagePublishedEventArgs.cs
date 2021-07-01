// Decompiled with JetBrains decompiler
// Type: MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Diagnostics
{
  public class MqttNetLogMessagePublishedEventArgs : EventArgs
  {
    public MqttNetLogMessagePublishedEventArgs(MqttNetLogMessage logMessage)
    {
      LogMessage = logMessage ?? throw new ArgumentNullException(nameof (logMessage));
    }


    public MqttNetLogMessage LogMessage { get; }
  }
}
