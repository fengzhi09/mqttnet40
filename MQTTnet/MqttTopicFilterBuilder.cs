// Decompiled with JetBrains decompiler
// Type: MQTTnet.MqttTopicFilterBuilder
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Exceptions;
using MQTTnet.Protocol;

namespace MQTTnet
{
  public class MqttTopicFilterBuilder
  {
    private MqttQualityOfServiceLevel _qualityOfServiceLevel;
    private string _topic;

    public MqttTopicFilterBuilder WithTopic(string topic)
    {
      _topic = topic;
      return this;
    }

    public MqttTopicFilterBuilder WithQualityOfServiceLevel(
      MqttQualityOfServiceLevel qualityOfServiceLevel)
    {
      _qualityOfServiceLevel = qualityOfServiceLevel;
      return this;
    }

    public MqttTopicFilterBuilder WithAtLeastOnceQoS()
    {
      _qualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce;
      return this;
    }

    public MqttTopicFilterBuilder WithAtMostOnceQoS()
    {
      _qualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce;
      return this;
    }

    public MqttTopicFilterBuilder WithExactlyOnceQoS()
    {
      _qualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce;
      return this;
    }

    public MqttTopicFilter Build() => !string.IsNullOrEmpty(_topic) ? new MqttTopicFilter
    {
      Topic = _topic,
      QualityOfServiceLevel = _qualityOfServiceLevel
    } : throw new MqttProtocolViolationException("Topic is not set.");
  }
}
