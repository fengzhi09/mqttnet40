// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.IMqttServerPersistedSession
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;

namespace MQTTnet.Server
{
  public interface IMqttServerPersistedSession
  {
    string ClientId { get; }

    IDictionary<object, object> Items { get; }

    IList<MqttTopicFilter> Subscriptions { get; }

    MqttApplicationMessage WillMessage { get; }

    uint? WillDelayInterval { get; }

    DateTime? SessionExpiryTimestamp { get; }

    IList<MqttQueuedApplicationMessage> PendingApplicationMessages { get; }
  }
}
