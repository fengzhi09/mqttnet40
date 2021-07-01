// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttConnAckPacketProperties
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;

namespace MQTTnet.Packets
{
  public class MqttConnAckPacketProperties
  {
    public uint? SessionExpiryInterval { get; set; }

    public ushort? ReceiveMaximum { get; set; }

    public bool? RetainAvailable { get; set; }

    public uint? MaximumPacketSize { get; set; }

    public string AssignedClientIdentifier { get; set; }

    public ushort? TopicAliasMaximum { get; set; }

    public string ReasonString { get; set; }

    public List<MqttUserProperty> UserProperties { get; set; }

    public bool? WildcardSubscriptionAvailable { get; set; }

    public bool? SubscriptionIdentifiersAvailable { get; set; }

    public bool? SharedSubscriptionAvailable { get; set; }

    public ushort? ServerKeepAlive { get; set; }

    public string ResponseInformation { get; set; }

    public string ServerReference { get; set; }

    public string AuthenticationMethod { get; set; }

    public byte[] AuthenticationData { get; set; }
  }
}
