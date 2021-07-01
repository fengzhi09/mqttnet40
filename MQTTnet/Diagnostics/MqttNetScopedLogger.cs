// Decompiled with JetBrains decompiler
// Type: MQTTnet.Diagnostics.MqttNetScopedLogger
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Diagnostics
{
  public sealed class MqttNetScopedLogger : IMqttNetScopedLogger
  {
    private readonly IMqttNetLogger _logger;
    private readonly string _source;

    public MqttNetScopedLogger(IMqttNetLogger logger, string source)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof (logger));
      _source = source ?? throw new ArgumentNullException(nameof (source));
    }

    public IMqttNetScopedLogger CreateScopedLogger(string source) => new MqttNetScopedLogger(_logger, source);

    public void Publish(
      MqttNetLogLevel logLevel,
      string message,
      object[] parameters,
      Exception exception)
    {
      _logger.Publish(logLevel, _source, message, parameters, exception);
    }
  }
}
