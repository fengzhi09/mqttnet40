// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttQueuedApplicationMessage
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Protocol;

namespace MQTTnet.Server
{
  public class MqttQueuedApplicationMessage
  {
    public MqttApplicationMessage ApplicationMessage { get; set; }

    public string SenderClientId { get; set; }

    public bool IsRetainedMessage { get; set; }

    public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

    public bool IsDuplicate { get; set; }
  }
}
