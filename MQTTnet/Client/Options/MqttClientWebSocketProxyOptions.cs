// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Options.MqttClientWebSocketProxyOptions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Client.Options
{
  public class MqttClientWebSocketProxyOptions
  {
    public string Address { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Domain { get; set; }

    public bool BypassOnLocal { get; set; }

    public string[] BypassList { get; set; }
  }
}
