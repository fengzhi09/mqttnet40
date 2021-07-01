// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Options.IMqttClientOptions
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
  public interface IMqttClientOptions
  {
    string ClientId { get; }

    bool CleanSession { get; }

    IMqttClientCredentials Credentials { get; }

    IMqttExtendedAuthenticationExchangeHandler ExtendedAuthenticationExchangeHandler { get; }

    MqttProtocolVersion ProtocolVersion { get; }

    IMqttClientChannelOptions ChannelOptions { get; }

    TimeSpan CommunicationTimeout { get; }

    TimeSpan KeepAlivePeriod { get; }

    MqttApplicationMessage WillMessage { get; }

    uint? WillDelayInterval { get; }

    string AuthenticationMethod { get; }

    byte[] AuthenticationData { get; }

    uint? MaximumPacketSize { get; }

    ushort? ReceiveMaximum { get; }

    bool? RequestProblemInformation { get; }

    bool? RequestResponseInformation { get; }

    uint? SessionExpiryInterval { get; }

    ushort? TopicAliasMaximum { get; }

    List<MqttUserProperty> UserProperties { get; set; }
  }
}
