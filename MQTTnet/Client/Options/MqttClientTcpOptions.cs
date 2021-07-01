// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Options.MqttClientTcpOptions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Net.Sockets;

namespace MQTTnet.Client.Options
{
  public class MqttClientTcpOptions : IMqttClientChannelOptions
  {
    public string Server { get; set; }

    public int? Port { get; set; }

    public int BufferSize { get; set; } = 65536;

    public bool? DualMode { get; set; }

    public bool NoDelay { get; set; } = true;

    public AddressFamily AddressFamily { get; set; }

    public MqttClientTlsOptions TlsOptions { get; set; } = new MqttClientTlsOptions();

    public override string ToString() => Server + ":" + this.GetPort();
  }
}
