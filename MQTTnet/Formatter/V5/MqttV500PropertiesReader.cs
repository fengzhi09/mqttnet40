// Decompiled with JetBrains decompiler
// Type: MQTTnet.Formatter.V5.MqttV500PropertiesReader
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Formatter.V5
{
  public class MqttV500PropertiesReader
  {
    private readonly IMqttPacketBodyReader _body;
    private readonly int _length;
    private readonly int _targetOffset;

    public MqttV500PropertiesReader(IMqttPacketBodyReader body)
    {
      _body = body ?? throw new ArgumentNullException(nameof (body));
      if (!body.EndOfStream)
        _length = (int) body.ReadVariableLengthInteger();
      _targetOffset = body.Offset + _length;
    }

    public MqttPropertyId CurrentPropertyId { get; private set; }

    public bool MoveNext()
    {
      if (_length == 0 || _body.Offset >= _targetOffset)
        return false;
      CurrentPropertyId = (MqttPropertyId) _body.ReadByte();
      return true;
    }

    public void AddUserPropertyTo(List<MqttUserProperty> userProperties)
    {
      if (userProperties == null)
        throw new ArgumentNullException(nameof (userProperties));
      var name = _body.ReadStringWithLengthPrefix();
      var str = _body.ReadStringWithLengthPrefix();
      userProperties.Add(new MqttUserProperty(name, str));
    }

    public string ReadReasonString() => _body.ReadStringWithLengthPrefix();

    public string ReadAuthenticationMethod() => _body.ReadStringWithLengthPrefix();

    public byte[] ReadAuthenticationData() => _body.ReadWithLengthPrefix();

    public bool ReadRetainAvailable() => _body.ReadBoolean();

    public uint ReadSessionExpiryInterval() => _body.ReadFourByteInteger();

    public ushort ReadReceiveMaximum() => _body.ReadTwoByteInteger();

    public string ReadAssignedClientIdentifier() => _body.ReadStringWithLengthPrefix();

    public string ReadServerReference() => _body.ReadStringWithLengthPrefix();

    public ushort ReadTopicAliasMaximum() => _body.ReadTwoByteInteger();

    public uint ReadMaximumPacketSize() => _body.ReadFourByteInteger();

    public ushort ReadServerKeepAlive() => _body.ReadTwoByteInteger();

    public string ReadResponseInformation() => _body.ReadStringWithLengthPrefix();

    public bool ReadSharedSubscriptionAvailable() => _body.ReadBoolean();

    public bool ReadSubscriptionIdentifiersAvailable() => _body.ReadBoolean();

    public bool ReadWildcardSubscriptionAvailable() => _body.ReadBoolean();

    public uint ReadSubscriptionIdentifier() => _body.ReadVariableLengthInteger();

    public MqttPayloadFormatIndicator? ReadPayloadFormatIndicator() => (MqttPayloadFormatIndicator) _body.ReadByte();

    public uint ReadMessageExpiryInterval() => _body.ReadFourByteInteger();

    public ushort ReadTopicAlias() => _body.ReadTwoByteInteger();

    public string ReadResponseTopic() => _body.ReadStringWithLengthPrefix();

    public byte[] ReadCorrelationData() => _body.ReadWithLengthPrefix();

    public string ReadContentType() => _body.ReadStringWithLengthPrefix();

    public uint ReadWillDelayInterval() => _body.ReadFourByteInteger();

    public bool RequestResponseInformation() => _body.ReadBoolean();

    public bool RequestProblemInformation() => _body.ReadBoolean();

    public void ThrowInvalidPropertyIdException(Type type) => throw new MqttProtocolViolationException(string.Format("Property ID '{0}' is not supported for package type '{1}'.", CurrentPropertyId, type.Name));
  }
}
