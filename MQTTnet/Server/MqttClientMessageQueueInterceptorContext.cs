// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttClientMessageQueueInterceptorContext
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Server
{
  public class MqttClientMessageQueueInterceptorContext
  {
    public MqttClientMessageQueueInterceptorContext(
      string senderClientId,
      string receiverClientId,
      MqttApplicationMessage applicationMessage)
    {
      SenderClientId = senderClientId;
      ReceiverClientId = receiverClientId;
      ApplicationMessage = applicationMessage;
    }

    public string SenderClientId { get; }

    public string ReceiverClientId { get; }

    public MqttApplicationMessage ApplicationMessage { get; set; }

    public bool AcceptEnqueue { get; set; } = true;
  }
}
