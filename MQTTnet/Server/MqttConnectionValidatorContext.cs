// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttConnectionValidatorContext
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MQTTnet.Adapter;
using MQTTnet.Formatter;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Server
{
  public class MqttConnectionValidatorContext
  {
    private readonly MqttConnectPacket _connectPacket;
    private readonly IMqttChannelAdapter _clientAdapter;

    public MqttConnectionValidatorContext(
      MqttConnectPacket connectPacket,
      IMqttChannelAdapter clientAdapter,
      IDictionary<object, object> sessionItems)
    {
      _connectPacket = connectPacket;
      _clientAdapter = clientAdapter ?? throw new ArgumentNullException(nameof (clientAdapter));
      SessionItems = sessionItems;
    }

    public string ClientId => _connectPacket.ClientId;

    public string Endpoint => _clientAdapter.Endpoint;

    public bool IsSecureConnection => _clientAdapter.IsSecureConnection;

    public X509Certificate2 ClientCertificate => _clientAdapter.ClientCertificate;

    public MqttProtocolVersion ProtocolVersion => _clientAdapter.PacketFormatterAdapter.ProtocolVersion;

    public string Username => _connectPacket?.Username;

    public byte[] RawPassword => _connectPacket?.Password;

    public string Password => Encoding.UTF8.GetString(RawPassword ?? new byte[0]);

    public MqttApplicationMessage WillMessage => _connectPacket?.WillMessage;

    public bool? CleanSession => _connectPacket?.CleanSession;

    public ushort? KeepAlivePeriod => _connectPacket?.KeepAlivePeriod;

    public List<MqttUserProperty> UserProperties => _connectPacket?.Properties?.UserProperties;

    public byte[] AuthenticationData => _connectPacket?.Properties?.AuthenticationData;

    public string AuthenticationMethod => _connectPacket?.Properties?.AuthenticationMethod;

    public uint? MaximumPacketSize => _connectPacket?.Properties?.MaximumPacketSize;

    public ushort? ReceiveMaximum => _connectPacket?.Properties?.ReceiveMaximum;

    public ushort? TopicAliasMaximum => _connectPacket?.Properties?.TopicAliasMaximum;

    public bool? RequestProblemInformation => _connectPacket?.Properties?.RequestProblemInformation;

    public bool? RequestResponseInformation => _connectPacket?.Properties?.RequestResponseInformation;

    public uint? SessionExpiryInterval => _connectPacket?.Properties?.SessionExpiryInterval;

    public uint? WillDelayInterval => _connectPacket?.Properties?.WillDelayInterval;

    public IDictionary<object, object> SessionItems { get; }

    [Obsolete("Use ReasonCode instead. It is MQTTv5 only but will be converted to a valid ReturnCode.")]
    public MqttConnectReturnCode ReturnCode
    {
      get => new MqttConnectReasonCodeConverter().ToConnectReturnCode(ReasonCode);
      set => ReasonCode = new MqttConnectReasonCodeConverter().ToConnectReasonCode(value);
    }

    public MqttConnectReasonCode ReasonCode { get; set; }

    public List<MqttUserProperty> ResponseUserProperties { get; set; }

    public byte[] ResponseAuthenticationData { get; set; }

    public string AssignedClientIdentifier { get; set; }

    public string ReasonString { get; set; }
  }
}
