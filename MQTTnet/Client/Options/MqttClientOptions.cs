// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Options.MqttClientOptions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using MQTTnet.Client.ExtendedAuthenticationExchange;
using MQTTnet.Formatter;
using MQTTnet.Packets;

namespace MQTTnet.Client.Options
{
  public class MqttClientOptions : IMqttClientOptions
  {
    public string ClientId { get; set; } = Guid.NewGuid().ToString("N");

    public bool CleanSession { get; set; } = true;

    public IMqttClientCredentials Credentials { get; set; }

    public IMqttExtendedAuthenticationExchangeHandler ExtendedAuthenticationExchangeHandler { get; set; }

    public MqttProtocolVersion ProtocolVersion { get; set; } = MqttProtocolVersion.V311;

    public IMqttClientChannelOptions ChannelOptions { get; set; }

    public TimeSpan CommunicationTimeout { get; set; } = TimeSpan.FromSeconds(10.0);

    public TimeSpan KeepAlivePeriod { get; set; } = TimeSpan.FromSeconds(15.0);

    public MqttApplicationMessage WillMessage { get; set; }

    public uint? WillDelayInterval { get; set; }

    public string AuthenticationMethod { get; set; }

    public byte[] AuthenticationData { get; set; }

    public uint? MaximumPacketSize { get; set; }

    public ushort? ReceiveMaximum { get; set; }

    public bool? RequestProblemInformation { get; set; }

    public bool? RequestResponseInformation { get; set; }

    public uint? SessionExpiryInterval { get; set; }

    public ushort? TopicAliasMaximum { get; set; }

    public List<MqttUserProperty> UserProperties { get; set; }
  }
}
