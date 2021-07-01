// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.ExtendedAuthenticationExchange.MqttExtendedAuthenticationExchangeContext
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Client.ExtendedAuthenticationExchange
{
  public class MqttExtendedAuthenticationExchangeContext
  {
    public MqttExtendedAuthenticationExchangeContext(MqttAuthPacket authPacket, IMqttClient client)
    {
      ReasonCode = authPacket != null ? authPacket.ReasonCode : throw new ArgumentNullException(nameof (authPacket));
      ReasonString = authPacket.Properties?.ReasonString;
      AuthenticationMethod = authPacket.Properties?.AuthenticationMethod;
      AuthenticationData = authPacket.Properties?.AuthenticationData;
      UserProperties = authPacket.Properties?.UserProperties;
      Client = client ?? throw new ArgumentNullException(nameof (client));
    }

    public MqttAuthenticateReasonCode ReasonCode { get; }

    public string ReasonString { get; }

    public string AuthenticationMethod { get; }

    public byte[] AuthenticationData { get; }

    public List<MqttUserProperty> UserProperties { get; }

    public IMqttClient Client { get; }
  }
}
