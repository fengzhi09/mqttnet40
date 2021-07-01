// Decompiled with JetBrains decompiler
// Type: MQTTnet.MqttApplicationMessageBuilder
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet
{
  public class MqttApplicationMessageBuilder
  {
    private MqttQualityOfServiceLevel _qualityOfServiceLevel;
    private string _topic;
    private byte[] _payload;
    private bool _retain;
    private string _contentType;
    private string _responseTopic;
    private byte[] _correlationData;
    private ushort? _topicAlias;
    private List<uint> _subscriptionIdentifiers;
    private uint? _messageExpiryInterval;
    private MqttPayloadFormatIndicator? _payloadFormatIndicator;
    private List<MqttUserProperty> _userProperties;

    public MqttApplicationMessageBuilder WithTopic(string topic)
    {
      _topic = topic;
      return this;
    }

    public MqttApplicationMessageBuilder WithPayload(byte[] payload)
    {
      _payload = payload;
      return this;
    }

    public MqttApplicationMessageBuilder WithPayload(
      IEnumerable<byte> payload)
    {
      if (payload == null)
      {
        _payload = null;
        return this;
      }
      _payload = payload as byte[];
      if (_payload == null)
        _payload = payload.ToArray();
      return this;
    }

    public MqttApplicationMessageBuilder WithPayload(Stream payload)
    {
      if (payload != null)
        return WithPayload(payload, payload.Length - payload.Position);
      _payload = null;
      return this;
    }

    public MqttApplicationMessageBuilder WithPayload(
      Stream payload,
      long length)
    {
      if (payload == null)
      {
        _payload = null;
        return this;
      }
      if (payload.Length == 0L)
      {
        _payload = null;
      }
      else
      {
        _payload = new byte[length];
        payload.Read(_payload, 0, _payload.Length);
      }
      return this;
    }

    public MqttApplicationMessageBuilder WithPayload(string payload)
    {
      if (payload == null)
      {
        _payload = null;
        return this;
      }
      _payload = string.IsNullOrEmpty(payload) ? null : Encoding.UTF8.GetBytes(payload);
      return this;
    }

    public MqttApplicationMessageBuilder WithQualityOfServiceLevel(
      MqttQualityOfServiceLevel qualityOfServiceLevel)
    {
      _qualityOfServiceLevel = qualityOfServiceLevel;
      return this;
    }

    public MqttApplicationMessageBuilder WithRetainFlag(bool value = true)
    {
      _retain = value;
      return this;
    }

    public MqttApplicationMessageBuilder WithAtLeastOnceQoS()
    {
      _qualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce;
      return this;
    }

    public MqttApplicationMessageBuilder WithAtMostOnceQoS()
    {
      _qualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce;
      return this;
    }

    public MqttApplicationMessageBuilder WithExactlyOnceQoS()
    {
      _qualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce;
      return this;
    }

    public MqttApplicationMessageBuilder WithUserProperty(
      string name,
      string value)
    {
      if (_userProperties == null)
        _userProperties = new List<MqttUserProperty>();
      _userProperties.Add(new MqttUserProperty(name, value));
      return this;
    }

    public MqttApplicationMessageBuilder WithContentType(
      string contentType)
    {
      _contentType = contentType;
      return this;
    }

    public MqttApplicationMessageBuilder WithResponseTopic(
      string responseTopic)
    {
      _responseTopic = responseTopic;
      return this;
    }

    public MqttApplicationMessageBuilder WithCorrelationData(
      byte[] correlationData)
    {
      _correlationData = correlationData;
      return this;
    }

    public MqttApplicationMessageBuilder WithTopicAlias(
      ushort topicAlias)
    {
      _topicAlias = topicAlias;
      return this;
    }

    public MqttApplicationMessageBuilder WithSubscriptionIdentifier(
      uint subscriptionIdentifier)
    {
      if (_subscriptionIdentifiers == null)
        _subscriptionIdentifiers = new List<uint>();
      _subscriptionIdentifiers.Add(subscriptionIdentifier);
      return this;
    }

    public MqttApplicationMessageBuilder WithMessageExpiryInterval(
      uint messageExpiryInterval)
    {
      _messageExpiryInterval = messageExpiryInterval;
      return this;
    }

    public MqttApplicationMessageBuilder WithPayloadFormatIndicator(
      MqttPayloadFormatIndicator payloadFormatIndicator)
    {
      _payloadFormatIndicator = payloadFormatIndicator;
      return this;
    }

    public MqttApplicationMessage Build()
    {
      if (string.IsNullOrEmpty(_topic))
        throw new MqttProtocolViolationException("Topic is not set.");
      return new MqttApplicationMessage
      {
        Topic = _topic,
        Payload = _payload,
        QualityOfServiceLevel = _qualityOfServiceLevel,
        Retain = _retain,
        ContentType = _contentType,
        ResponseTopic = _responseTopic,
        CorrelationData = _correlationData,
        TopicAlias = _topicAlias,
        SubscriptionIdentifiers = _subscriptionIdentifiers,
        MessageExpiryInterval = _messageExpiryInterval,
        PayloadFormatIndicator = _payloadFormatIndicator,
        UserProperties = _userProperties
      };
    }
  }
}
