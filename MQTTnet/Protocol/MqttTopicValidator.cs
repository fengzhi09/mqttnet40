// Decompiled with JetBrains decompiler
// Type: MQTTnet.Protocol.MqttTopicValidator
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Exceptions;

namespace MQTTnet.Protocol
{
  public static class MqttTopicValidator
  {
    public static void ThrowIfInvalid(string topic)
    {
      if (string.IsNullOrEmpty(topic))
        throw new MqttProtocolViolationException("Topic should not be empty.");
      foreach (int num in topic)
      {
        switch (num)
        {
          case 35:
            throw new MqttProtocolViolationException("The character '#' is not allowed in topics.");
          case 43:
            throw new MqttProtocolViolationException("The character '+' is not allowed in topics.");
          default:
            continue;
        }
      }
    }
  }
}
