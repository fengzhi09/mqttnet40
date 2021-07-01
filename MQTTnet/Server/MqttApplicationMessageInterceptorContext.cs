// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttApplicationMessageInterceptorContext
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;

namespace MQTTnet.Server
{
  public class MqttApplicationMessageInterceptorContext
  {
    public MqttApplicationMessageInterceptorContext(
      string clientId,
      IDictionary<object, object> sessionItems,
      MqttApplicationMessage applicationMessage)
    {
      ClientId = clientId;
      ApplicationMessage = applicationMessage;
      SessionItems = sessionItems;
    }

    public string ClientId { get; }

    public MqttApplicationMessage ApplicationMessage { get; set; }

    public IDictionary<object, object> SessionItems { get; }

    public bool AcceptPublish { get; set; } = true;

    public bool CloseConnection { get; set; }
  }
}
