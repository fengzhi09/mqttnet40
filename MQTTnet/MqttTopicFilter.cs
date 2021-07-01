// Decompiled with JetBrains decompiler
// Type: MQTTnet.MqttTopicFilter
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Protocol;

namespace MQTTnet
{
  public class MqttTopicFilter
  {
    public string Topic { get; set; }

    public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

    public bool? NoLocal { get; set; }

    public bool? RetainAsPublished { get; set; }

    public MqttRetainHandling? RetainHandling { get; set; }

    public override string ToString() => "TopicFilter: [Topic=" + Topic + "] [QualityOfServiceLevel=" + QualityOfServiceLevel + "] [NoLocal=" + NoLocal + "] [RetainAsPublished=" + RetainAsPublished + "] [RetainHandling=" + RetainHandling + "]";
  }
}
