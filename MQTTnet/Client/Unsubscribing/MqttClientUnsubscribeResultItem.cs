// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Unsubscribing.MqttClientUnsubscribeResultItem
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Client.Unsubscribing
{
  public class MqttClientUnsubscribeResultItem
  {
    public MqttClientUnsubscribeResultItem(
      string topicFilter,
      MqttClientUnsubscribeResultCode reasonCode)
    {
      TopicFilter = topicFilter ?? throw new ArgumentNullException(nameof (topicFilter));
      ReasonCode = reasonCode;
    }

    public string TopicFilter { get; }

    public MqttClientUnsubscribeResultCode ReasonCode { get; }
  }
}
