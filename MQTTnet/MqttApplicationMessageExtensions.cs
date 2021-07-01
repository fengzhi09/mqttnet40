// Decompiled with JetBrains decompiler
// Type: MQTTnet.MqttApplicationMessageExtensions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Text;

namespace MQTTnet
{
  public static class MqttApplicationMessageExtensions
  {
    public static string ConvertPayloadToString(this MqttApplicationMessage applicationMessage)
    {
      if (applicationMessage == null)
        throw new ArgumentNullException(nameof (applicationMessage));
      if (applicationMessage.Payload == null)
        return null;
      return applicationMessage.Payload.Length == 0 ? string.Empty : Encoding.UTF8.GetString(applicationMessage.Payload, 0, applicationMessage.Payload.Length);
    }
  }
}
