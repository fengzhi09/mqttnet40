// Decompiled with JetBrains decompiler
// Type: MQTTnet.Diagnostics.MqttNetLogger
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading;

namespace MQTTnet.Diagnostics
{
    public class MqttNetLogger : IMqttNetLogger
    {
        private readonly string _logId;

        public MqttNetLogger()
        {
        }

        public MqttNetLogger(string logId) => _logId = logId;

        public event EventHandler<MqttNetLogMessagePublishedEventArgs> LogMessagePublished;

        public IMqttNetScopedLogger CreateScopedLogger(string source) => source != null
            ? (IMqttNetScopedLogger) new MqttNetScopedLogger(this, source)
            : throw new ArgumentNullException(nameof(source));

        public void Publish(
            MqttNetLogLevel level,
            string source,
            string message,
            object[] parameters,
            Exception exception)
        {
            var flag = LogMessagePublished != null;
            var hasListeners = MqttNetGlobalLogger.HasListeners;
            if (!flag && !hasListeners)
                return;
            try
            {
                message = string.Format(message ?? string.Empty, parameters);
            }
            catch (FormatException)
            {
                message = "MESSAGE FORMAT INVALID: " + message;
            }

            var logMessage = new MqttNetLogMessage
            {
                LogId = _logId,
                Timestamp = DateTime.UtcNow,
                Source = source,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Level = level,
                Message = message,
                Exception = exception
            };
            if (hasListeners)
                MqttNetGlobalLogger.Publish(logMessage);
            if (!flag)
                return;
            var messagePublished = LogMessagePublished;
            if (messagePublished == null)
                return;
            messagePublished(this, new MqttNetLogMessagePublishedEventArgs(logMessage));
        }
    }
}