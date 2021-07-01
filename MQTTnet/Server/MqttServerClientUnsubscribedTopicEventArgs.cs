// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerClientUnsubscribedTopicEventArgs
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Server
{
  public class MqttServerClientUnsubscribedTopicEventArgs : EventArgs
  {
    public MqttServerClientUnsubscribedTopicEventArgs(string clientId, string topicFilter)
    {
      ClientId = clientId ?? throw new ArgumentNullException(nameof (clientId));
      TopicFilter = topicFilter ?? throw new ArgumentNullException(nameof (topicFilter));
    }

    public string ClientId { get; }

    public string TopicFilter { get; }
  }
}
