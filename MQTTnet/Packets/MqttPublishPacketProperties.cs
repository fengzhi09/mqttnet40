// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttPublishPacketProperties
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;
using MQTTnet.Protocol;

namespace MQTTnet.Packets
{
  public class MqttPublishPacketProperties
  {
    public MqttPayloadFormatIndicator? PayloadFormatIndicator { get; set; }

    public uint? MessageExpiryInterval { get; set; }

    public ushort? TopicAlias { get; set; }

    public string ResponseTopic { get; set; }

    public byte[] CorrelationData { get; set; }

    public List<MqttUserProperty> UserProperties { get; set; }

    public List<uint> SubscriptionIdentifiers { get; set; }

    public string ContentType { get; set; }
  }
}
