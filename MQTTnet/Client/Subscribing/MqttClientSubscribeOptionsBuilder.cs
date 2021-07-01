// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Subscribing.MqttClientSubscribeOptionsBuilder
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Client.Subscribing
{
  public class MqttClientSubscribeOptionsBuilder
  {
    private readonly MqttClientSubscribeOptions _subscribeOptions = new MqttClientSubscribeOptions();

    public MqttClientSubscribeOptionsBuilder WithUserProperty(
      string name,
      string value)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (_subscribeOptions.UserProperties == null)
        _subscribeOptions.UserProperties = new List<MqttUserProperty>();
      _subscribeOptions.UserProperties.Add(new MqttUserProperty(name, value));
      return this;
    }

    public MqttClientSubscribeOptionsBuilder WithSubscriptionIdentifier(
      uint? subscriptionIdentifier)
    {
      _subscribeOptions.SubscriptionIdentifier = subscriptionIdentifier;
      return this;
    }

    public MqttClientSubscribeOptionsBuilder WithTopicFilter(
      string topic,
      MqttQualityOfServiceLevel qualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce,
      bool? noLocal = null,
      bool? retainAsPublished = null,
      MqttRetainHandling? retainHandling = null)
    {
      return WithTopicFilter(new MqttTopicFilter
      {
        Topic = topic,
        QualityOfServiceLevel = qualityOfServiceLevel,
        NoLocal = noLocal,
        RetainAsPublished = retainAsPublished,
        RetainHandling = retainHandling
      });
    }

    public MqttClientSubscribeOptionsBuilder WithTopicFilter(
      Action<MqttTopicFilterBuilder> topicFilterBuilder)
    {
      if (topicFilterBuilder == null)
        throw new ArgumentNullException(nameof (topicFilterBuilder));
      var topicFilterBuilder1 = new MqttTopicFilterBuilder();
      topicFilterBuilder(topicFilterBuilder1);
      return WithTopicFilter(topicFilterBuilder1);
    }

    public MqttClientSubscribeOptionsBuilder WithTopicFilter(
      MqttTopicFilterBuilder topicFilterBuilder)
    {
      return topicFilterBuilder != null ? WithTopicFilter(topicFilterBuilder.Build()) : throw new ArgumentNullException(nameof (topicFilterBuilder));
    }

    public MqttClientSubscribeOptionsBuilder WithTopicFilter(
      MqttTopicFilter topicFilter)
    {
      if (topicFilter == null)
        throw new ArgumentNullException(nameof (topicFilter));
      if (_subscribeOptions.TopicFilters == null)
        _subscribeOptions.TopicFilters = new List<MqttTopicFilter>();
      _subscribeOptions.TopicFilters.Add(topicFilter);
      return this;
    }

    public MqttClientSubscribeOptions Build() => _subscribeOptions;
  }
}
