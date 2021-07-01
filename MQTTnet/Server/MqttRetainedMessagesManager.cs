// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttRetainedMessagesManager
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MQTTnet.Diagnostics;
using MQTTnet.Implementations;
using MQTTnet.Internal;

namespace MQTTnet.Server
{
  public class MqttRetainedMessagesManager : IMqttRetainedMessagesManager
  {
    private readonly byte[] _emptyArray = new byte[0];
    private readonly AsyncLock _messagesLock = new AsyncLock();
    private readonly Dictionary<string, MqttApplicationMessage> _messages = new Dictionary<string, MqttApplicationMessage>();
    private IMqttNetScopedLogger _logger;
    private IMqttServerOptions _options;

    public Task Start(IMqttServerOptions options, IMqttNetLogger logger)
    {
      _logger = logger != null ? logger.CreateScopedLogger(nameof (MqttRetainedMessagesManager)) : throw new ArgumentNullException(nameof (logger));
      _options = options ?? throw new ArgumentNullException(nameof (options));
      return PlatformAbstractionLayer.CompletedTask;
    }

    public async Task LoadMessagesAsync()
    {
      if (_options.Storage == null)
        return;
      try
      {
        var retainedMessages = await _options.Storage.LoadRetainedMessagesAsync().ConfigureAwait(false);
        var source = retainedMessages;
        if ((source != null ? (source.Any() ? 1 : 0) : 0) != 0)
        {
          using (await _messagesLock.WaitAsync().ConfigureAwait(false))
          {
            _messages.Clear();
            foreach (var applicationMessage in retainedMessages)
              _messages[applicationMessage.Topic] = applicationMessage;
          }
        }
        retainedMessages = null;
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Unhandled exception while loading retained messages.");
      }
    }

    public async Task HandleMessageAsync(
      string clientId,
      MqttApplicationMessage applicationMessage)
    {
      if (applicationMessage == null)
        throw new ArgumentNullException(nameof (applicationMessage));
      try
      {
        using (await _messagesLock.WaitAsync().ConfigureAwait(false))
        {
          var flag = false;
          if ((applicationMessage.Payload == null ? 0 : ((uint) applicationMessage.Payload.Length > 0U ? 1 : 0)) == 0)
          {
            flag = _messages.Remove(applicationMessage.Topic);
            _logger.Verbose("Client '{0}' cleared retained message for topic '{1}'.", (object) clientId, (object) applicationMessage.Topic);
          }
          else
          {
            MqttApplicationMessage applicationMessage1;
            if (!_messages.TryGetValue(applicationMessage.Topic, out applicationMessage1))
            {
              _messages[applicationMessage.Topic] = applicationMessage;
              flag = true;
            }
            else if (applicationMessage1.QualityOfServiceLevel != applicationMessage.QualityOfServiceLevel || !applicationMessage1.Payload.SequenceEqual(applicationMessage.Payload ?? _emptyArray))
            {
              _messages[applicationMessage.Topic] = applicationMessage;
              flag = true;
            }
            _logger.Verbose("Client '{0}' set retained message for topic '{1}'.", (object) clientId, (object) applicationMessage.Topic);
          }
          if (flag)
          {
            if (_options.Storage != null)
              await _options.Storage.SaveRetainedMessagesAsync(new List<MqttApplicationMessage>(_messages.Values)).ConfigureAwait(false);
          }
        }
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Unhandled exception while handling retained messages.");
      }
    }

    public async Task<IList<MqttApplicationMessage>> GetSubscribedMessagesAsync(
      ICollection<MqttTopicFilter> topicFilters)
    {
      if (topicFilters == null)
        throw new ArgumentNullException(nameof (topicFilters));
      var matchingRetainedMessages = new List<MqttApplicationMessage>();
      List<MqttApplicationMessage> list;
      using (await _messagesLock.WaitAsync().ConfigureAwait(false))
        list = _messages.Values.ToList();
      foreach (var applicationMessage in list)
      {
        foreach (var topicFilter in topicFilters)
        {
          if (MqttTopicFilterComparer.IsMatch(applicationMessage.Topic, topicFilter.Topic))
          {
            matchingRetainedMessages.Add(applicationMessage);
            break;
          }
        }
      }
      IList<MqttApplicationMessage> applicationMessageList = matchingRetainedMessages;
      matchingRetainedMessages = null;
      return applicationMessageList;
    }

    public async Task<IList<MqttApplicationMessage>> GetMessagesAsync()
    {
      IList<MqttApplicationMessage> list;
      using (await _messagesLock.WaitAsync().ConfigureAwait(false))
        list = _messages.Values.ToList();
      return list;
    }

    public async Task ClearMessagesAsync()
    {
      using (await _messagesLock.WaitAsync().ConfigureAwait(false))
      {
        _messages.Clear();
        if (_options.Storage != null)
          await _options.Storage.SaveRetainedMessagesAsync(new List<MqttApplicationMessage>()).ConfigureAwait(false);
      }
    }
  }
}
