// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttClientSubscriptionsManager
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Server
{
  public class MqttClientSubscriptionsManager
  {
    private readonly Dictionary<string, MqttTopicFilter> _subscriptions = new Dictionary<string, MqttTopicFilter>();
    private readonly MqttClientSession _clientSession;
    private readonly IMqttServerOptions _serverOptions;
    private readonly MqttServerEventDispatcher _eventDispatcher;

    public MqttClientSubscriptionsManager(
      MqttClientSession clientSession,
      MqttServerEventDispatcher eventDispatcher,
      IMqttServerOptions serverOptions)
    {
      _clientSession = clientSession ?? throw new ArgumentNullException(nameof (clientSession));
      _serverOptions = serverOptions ?? throw new ArgumentNullException(nameof (serverOptions));
      _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof (eventDispatcher));
    }

    public async Task<MqttClientSubscribeResult> SubscribeAsync(
      MqttSubscribePacket subscribePacket,
      MqttConnectPacket connectPacket)
    {
      if (subscribePacket == null)
        throw new ArgumentNullException(nameof (subscribePacket));
      if (connectPacket == null)
        throw new ArgumentNullException(nameof (connectPacket));
      var result = new MqttClientSubscribeResult
      {
        ResponsePacket = new MqttSubAckPacket
        {
          PacketIdentifier = subscribePacket.PacketIdentifier
        },
        CloseConnection = false
      };
      foreach (var topicFilter1 in subscribePacket.TopicFilters)
      {
        var interceptorContext = await InterceptSubscribeAsync(topicFilter1).ConfigureAwait(false);
        var topicFilter2 = interceptorContext.TopicFilter;
        if (topicFilter2 == null || string.IsNullOrEmpty(topicFilter2.Topic) || !interceptorContext.AcceptSubscription)
        {
          result.ResponsePacket.ReturnCodes.Add(MqttSubscribeReturnCode.Failure);
          result.ResponsePacket.ReasonCodes.Add(MqttSubscribeReasonCode.UnspecifiedError);
        }
        else
        {
          result.ResponsePacket.ReturnCodes.Add(ConvertToSubscribeReturnCode(topicFilter2.QualityOfServiceLevel));
          result.ResponsePacket.ReasonCodes.Add(ConvertToSubscribeReasonCode(topicFilter2.QualityOfServiceLevel));
        }
        if (interceptorContext.CloseConnection)
          result.CloseConnection = true;
        if (interceptorContext.AcceptSubscription && !string.IsNullOrEmpty(topicFilter2?.Topic))
        {
          lock (_subscriptions)
            _subscriptions[topicFilter2.Topic] = topicFilter2;
          await _eventDispatcher.SafeNotifyClientSubscribedTopicAsync(_clientSession.ClientId, topicFilter2).ConfigureAwait(false);
        }
      }
      var clientSubscribeResult = result;
      result = null;
      return clientSubscribeResult;
    }

    public async Task SubscribeAsync(IEnumerable<MqttTopicFilter> topicFilters)
    {
      if (topicFilters == null)
        throw new ArgumentNullException(nameof (topicFilters));
      foreach (var topicFilter1 in topicFilters)
      {
        var topicFilter = topicFilter1;
        var interceptorContext = await InterceptSubscribeAsync(topicFilter).ConfigureAwait(false);
        if (interceptorContext.AcceptSubscription)
        {
          if (interceptorContext.AcceptSubscription)
          {
            lock (_subscriptions)
              _subscriptions[topicFilter.Topic] = topicFilter;
            await _eventDispatcher.SafeNotifyClientSubscribedTopicAsync(_clientSession.ClientId, topicFilter).ConfigureAwait(false);
          }
          topicFilter = null;
        }
      }
    }

    public async Task<MqttUnsubAckPacket> UnsubscribeAsync(
      MqttUnsubscribePacket unsubscribePacket)
    {
      var unsubAckPacket = unsubscribePacket != null ? new MqttUnsubAckPacket
      {
        PacketIdentifier = unsubscribePacket.PacketIdentifier
      } : throw new ArgumentNullException(nameof (unsubscribePacket));
      foreach (var topicFilter1 in unsubscribePacket.TopicFilters)
      {
        var topicFilter = topicFilter1;
        if (!(await InterceptUnsubscribeAsync(topicFilter).ConfigureAwait(false)).AcceptUnsubscription)
        {
          unsubAckPacket.ReasonCodes.Add(MqttUnsubscribeReasonCode.ImplementationSpecificError);
        }
        else
        {
          lock (_subscriptions)
          {
            if (_subscriptions.Remove(topicFilter))
              unsubAckPacket.ReasonCodes.Add(MqttUnsubscribeReasonCode.Success);
            else
              unsubAckPacket.ReasonCodes.Add(MqttUnsubscribeReasonCode.NoSubscriptionExisted);
          }
          topicFilter = null;
        }
      }
      foreach (var topicFilter in unsubscribePacket.TopicFilters)
        await _eventDispatcher.SafeNotifyClientUnsubscribedTopicAsync(_clientSession.ClientId, topicFilter).ConfigureAwait(false);
      var mqttUnsubAckPacket = unsubAckPacket;
      unsubAckPacket = null;
      return mqttUnsubAckPacket;
    }

    public async Task UnsubscribeAsync(IEnumerable<string> topicFilters)
    {
      if (topicFilters == null)
        throw new ArgumentNullException(nameof (topicFilters));
      foreach (var topicFilter1 in topicFilters)
      {
        var topicFilter = topicFilter1;
        if ((await InterceptUnsubscribeAsync(topicFilter).ConfigureAwait(false)).AcceptUnsubscription)
        {
          lock (_subscriptions)
            _subscriptions.Remove(topicFilter);
          topicFilter = null;
        }
      }
    }

    public CheckSubscriptionsResult CheckSubscriptions(
      string topic,
      MqttQualityOfServiceLevel qosLevel)
    {
      var subscribedQoSLevels = new HashSet<MqttQualityOfServiceLevel>();
      lock (_subscriptions)
      {
        foreach (var subscription in _subscriptions)
        {
          if (MqttTopicFilterComparer.IsMatch(topic, subscription.Key))
            subscribedQoSLevels.Add(subscription.Value.QualityOfServiceLevel);
        }
      }
      if (subscribedQoSLevels.Count != 0)
        return CreateSubscriptionResult(qosLevel, subscribedQoSLevels);
      return new CheckSubscriptionsResult
      {
        IsSubscribed = false
      };
    }

    private static MqttSubscribeReturnCode ConvertToSubscribeReturnCode(
      MqttQualityOfServiceLevel qualityOfServiceLevel)
    {
      switch (qualityOfServiceLevel)
      {
        case MqttQualityOfServiceLevel.AtMostOnce:
          return MqttSubscribeReturnCode.SuccessMaximumQoS0;
        case MqttQualityOfServiceLevel.AtLeastOnce:
          return MqttSubscribeReturnCode.SuccessMaximumQoS1;
        case MqttQualityOfServiceLevel.ExactlyOnce:
          return MqttSubscribeReturnCode.SuccessMaximumQoS2;
        default:
          return MqttSubscribeReturnCode.Failure;
      }
    }

    private static MqttSubscribeReasonCode ConvertToSubscribeReasonCode(
      MqttQualityOfServiceLevel qualityOfServiceLevel)
    {
      switch (qualityOfServiceLevel)
      {
        case MqttQualityOfServiceLevel.AtMostOnce:
          return MqttSubscribeReasonCode.GrantedQoS0;
        case MqttQualityOfServiceLevel.AtLeastOnce:
          return MqttSubscribeReasonCode.GrantedQoS1;
        case MqttQualityOfServiceLevel.ExactlyOnce:
          return MqttSubscribeReasonCode.GrantedQoS2;
        default:
          return MqttSubscribeReasonCode.UnspecifiedError;
      }
    }

    private async Task<MqttSubscriptionInterceptorContext> InterceptSubscribeAsync(
      MqttTopicFilter topicFilter)
    {
      var context = new MqttSubscriptionInterceptorContext(_clientSession.ClientId, topicFilter, _clientSession.Items);
      if (_serverOptions.SubscriptionInterceptor != null)
        await _serverOptions.SubscriptionInterceptor.InterceptSubscriptionAsync(context).ConfigureAwait(false);
      var interceptorContext = context;
      context = null;
      return interceptorContext;
    }

    private async Task<MqttUnsubscriptionInterceptorContext> InterceptUnsubscribeAsync(
      string topicFilter)
    {
      var context = new MqttUnsubscriptionInterceptorContext(_clientSession.ClientId, topicFilter, _clientSession.Items);
      if (_serverOptions.UnsubscriptionInterceptor != null)
        await _serverOptions.UnsubscriptionInterceptor.InterceptUnsubscriptionAsync(context).ConfigureAwait(false);
      var interceptorContext = context;
      context = null;
      return interceptorContext;
    }

    private static CheckSubscriptionsResult CreateSubscriptionResult(
      MqttQualityOfServiceLevel qosLevel,
      HashSet<MqttQualityOfServiceLevel> subscribedQoSLevels)
    {
      var qualityOfServiceLevel = !subscribedQoSLevels.Contains(qosLevel) ? (subscribedQoSLevels.Count != 1 ? subscribedQoSLevels.Max() : subscribedQoSLevels.First()) : qosLevel;
      return new CheckSubscriptionsResult
      {
        IsSubscribed = true,
        QualityOfServiceLevel = qualityOfServiceLevel
      };
    }
  }
}
