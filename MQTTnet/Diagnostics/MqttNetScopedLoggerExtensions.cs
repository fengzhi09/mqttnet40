// Decompiled with JetBrains decompiler
// Type: MQTTnet.Diagnostics.MqttNetScopedLoggerExtensions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Diagnostics
{
  public static class MqttNetScopedLoggerExtensions
  {
    public static void Verbose(
      this IMqttNetScopedLogger logger,
      string message,
      params object[] parameters)
    {
      logger.Publish(MqttNetLogLevel.Verbose, message, parameters, null);
    }

    public static void Info(
      this IMqttNetScopedLogger logger,
      string message,
      params object[] parameters)
    {
      logger.Publish(MqttNetLogLevel.Info, message, parameters, null);
    }

    public static void Warning(
      this IMqttNetScopedLogger logger,
      Exception exception,
      string message,
      params object[] parameters)
    {
      logger.Publish(MqttNetLogLevel.Warning, message, parameters, exception);
    }

    public static void Error(
      this IMqttNetScopedLogger logger,
      Exception exception,
      string message,
      params object[] parameters)
    {
      logger.Publish(MqttNetLogLevel.Error, message, parameters, exception);
    }
  }
}
