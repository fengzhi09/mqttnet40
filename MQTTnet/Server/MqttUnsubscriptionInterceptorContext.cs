// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttUnsubscriptionInterceptorContext
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;

namespace MQTTnet.Server
{
  public class MqttUnsubscriptionInterceptorContext
  {
    public MqttUnsubscriptionInterceptorContext(
      string clientId,
      string topic,
      IDictionary<object, object> sessionItems)
    {
      ClientId = clientId;
      Topic = topic;
      SessionItems = sessionItems;
    }

    public string ClientId { get; }

    public string Topic { get; set; }

    public IDictionary<object, object> SessionItems { get; }

    public bool AcceptUnsubscription { get; set; } = true;

    public bool CloseConnection { get; set; }
  }
}
