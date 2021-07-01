// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttClientSession
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet.Diagnostics;
using MQTTnet.Server.Status;

namespace MQTTnet.Server
{
  public class MqttClientSession
  {
    private readonly IMqttNetScopedLogger _logger;
    private readonly DateTime _createdTimestamp = DateTime.UtcNow;
    private readonly IMqttRetainedMessagesManager _retainedMessagesManager;

    public MqttClientSession(
      string clientId,
      IDictionary<object, object> items,
      MqttServerEventDispatcher eventDispatcher,
      IMqttServerOptions serverOptions,
      IMqttRetainedMessagesManager retainedMessagesManager,
      IMqttNetLogger logger)
    {
      ClientId = clientId ?? throw new ArgumentNullException(nameof (clientId));
      Items = items ?? throw new ArgumentNullException(nameof (items));
      _retainedMessagesManager = retainedMessagesManager ?? throw new ArgumentNullException(nameof (retainedMessagesManager));
      SubscriptionsManager = new MqttClientSubscriptionsManager(this, eventDispatcher, serverOptions);
      ApplicationMessagesQueue = new MqttClientSessionApplicationMessagesQueue(serverOptions);
      _logger = logger != null ? logger.CreateScopedLogger(nameof (MqttClientSession)) : throw new ArgumentNullException(nameof (logger));
    }

    public string ClientId { get; }

    public bool IsCleanSession { get; set; } = true;

    public MqttApplicationMessage WillMessage { get; set; }

    public MqttClientSubscriptionsManager SubscriptionsManager { get; }

    public MqttClientSessionApplicationMessagesQueue ApplicationMessagesQueue { get; }

    public IDictionary<object, object> Items { get; }

    public bool EnqueueApplicationMessage(
      MqttApplicationMessage applicationMessage,
      string senderClientId,
      bool isRetainedApplicationMessage)
    {
      var subscriptionsResult = SubscriptionsManager.CheckSubscriptions(applicationMessage.Topic, applicationMessage.QualityOfServiceLevel);
      if (!subscriptionsResult.IsSubscribed)
        return false;
      _logger.Verbose("Queued application message with topic '{0}' (ClientId: {1}).", (object) applicationMessage.Topic, (object) ClientId);
      ApplicationMessagesQueue.Enqueue(applicationMessage, senderClientId, subscriptionsResult.QualityOfServiceLevel, isRetainedApplicationMessage);
      return true;
    }

    public async Task SubscribeAsync(ICollection<MqttTopicFilter> topicFilters)
    {
      if (topicFilters == null)
        throw new ArgumentNullException(nameof (topicFilters));
      await SubscriptionsManager.SubscribeAsync(topicFilters).ConfigureAwait(false);
      foreach (var applicationMessage in await _retainedMessagesManager.GetSubscribedMessagesAsync(topicFilters).ConfigureAwait(false))
        EnqueueApplicationMessage(applicationMessage, null, true);
    }

    public Task UnsubscribeAsync(IEnumerable<string> topicFilters) => topicFilters != null ? SubscriptionsManager.UnsubscribeAsync(topicFilters) : throw new ArgumentNullException(nameof (topicFilters));

    public void FillStatus(MqttSessionStatus status)
    {
      status.ClientId = ClientId;
      status.CreatedTimestamp = _createdTimestamp;
      status.PendingApplicationMessagesCount = ApplicationMessagesQueue.Count;
      status.Items = Items;
    }
  }
}
