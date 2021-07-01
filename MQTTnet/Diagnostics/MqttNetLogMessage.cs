// Decompiled with JetBrains decompiler
// Type: MQTTnet.Diagnostics.MqttNetLogMessage
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Diagnostics
{
  public class MqttNetLogMessage
  {
    public string LogId { get; set; }

    public DateTime Timestamp { get; set; }

    public int ThreadId { get; set; }

    public string Source { get; set; }

    public MqttNetLogLevel Level { get; set; }

    public string Message { get; set; }

    public Exception Exception { get; set; }

    public override string ToString()
    {
      var str = string.Format("[{0:O}] [{1}] [{2}] [{3}] [{4}]: {5}", (object) Timestamp, (object) LogId, (object) ThreadId, (object) Source, (object) Level, (object) Message);
      if (Exception != null)
        str = str + Environment.NewLine + Exception;
      return str;
    }
  }
}
