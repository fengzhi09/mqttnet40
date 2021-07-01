// Decompiled with JetBrains decompiler
// Type: MQTTnet.Exceptions.MqttUnexpectedDisconnectReceivedException
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.Exceptions
{
  public class MqttUnexpectedDisconnectReceivedException : MqttCommunicationException
  {
    public MqttUnexpectedDisconnectReceivedException(MqttDisconnectPacket disconnectPacket)
      : base(string.Format("Unexpected DISCONNECT (Reason code={0}) received.", disconnectPacket.ReasonCode))
    {
      ReasonCode = disconnectPacket.ReasonCode;
      SessionExpiryInterval = disconnectPacket.Properties?.SessionExpiryInterval;
      ReasonString = disconnectPacket.Properties?.ReasonString;
      ServerReference = disconnectPacket.Properties?.ServerReference;
      UserProperties = disconnectPacket.Properties?.UserProperties;
    }

    public MqttDisconnectReasonCode? ReasonCode { get; }

    public uint? SessionExpiryInterval { get; }

    public string ReasonString { get; }

    public List<MqttUserProperty> UserProperties { get; }

    public string ServerReference { get; }
  }
}
