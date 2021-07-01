// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Unsubscribing.MqttClientUnsubscribeOptionsBuilder
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using MQTTnet.Packets;

namespace MQTTnet.Client.Unsubscribing
{
  public class MqttClientUnsubscribeOptionsBuilder
  {
    private readonly MqttClientUnsubscribeOptions _unsubscribeOptions = new MqttClientUnsubscribeOptions();

    public MqttClientUnsubscribeOptionsBuilder WithUserProperty(
      string name,
      string value)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      return value != null ? WithUserProperty(new MqttUserProperty(name, value)) : throw new ArgumentNullException(nameof (value));
    }

    public MqttClientUnsubscribeOptionsBuilder WithUserProperty(
      MqttUserProperty userProperty)
    {
      if (userProperty == null)
        throw new ArgumentNullException(nameof (userProperty));
      if (_unsubscribeOptions.UserProperties == null)
        _unsubscribeOptions.UserProperties = new List<MqttUserProperty>();
      _unsubscribeOptions.UserProperties.Add(userProperty);
      return this;
    }

    public MqttClientUnsubscribeOptionsBuilder WithTopicFilter(
      string topic)
    {
      if (topic == null)
        throw new ArgumentNullException(nameof (topic));
      if (_unsubscribeOptions.TopicFilters == null)
        _unsubscribeOptions.TopicFilters = new List<string>();
      _unsubscribeOptions.TopicFilters.Add(topic);
      return this;
    }

    public MqttClientUnsubscribeOptionsBuilder WithTopicFilter(
      MqttTopicFilter topicFilter)
    {
      return topicFilter != null ? WithTopicFilter(topicFilter.Topic) : throw new ArgumentNullException(nameof (topicFilter));
    }

    public MqttClientUnsubscribeOptions Build() => _unsubscribeOptions;
  }
}
