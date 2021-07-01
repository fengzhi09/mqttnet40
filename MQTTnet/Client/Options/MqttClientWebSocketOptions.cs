// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Options.MqttClientWebSocketOptions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;
using System.Net;

namespace MQTTnet.Client.Options
{
  public class MqttClientWebSocketOptions : IMqttClientChannelOptions
  {
    public string Uri { get; set; }

    public IDictionary<string, string> RequestHeaders { get; set; }

    public ICollection<string> SubProtocols { get; set; } = (ICollection<string>) new List<string>
    {
      "mqtt"
    };

    public CookieContainer CookieContainer { get; set; }

    public MqttClientWebSocketProxyOptions ProxyOptions { get; set; }

    public MqttClientTlsOptions TlsOptions { get; set; } = new MqttClientTlsOptions();

    public override string ToString() => Uri;
  }
}
