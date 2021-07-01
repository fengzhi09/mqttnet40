// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerTcpEndpointBaseOptions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Net;

namespace MQTTnet.Server
{
  public abstract class MqttServerTcpEndpointBaseOptions
  {
    public bool IsEnabled { get; set; }

    public int Port { get; set; }

    public int ConnectionBacklog { get; set; } = 10;

    public bool NoDelay { get; set; } = true;

    public IPAddress BoundInterNetworkAddress { get; set; } = IPAddress.Any;

    public IPAddress BoundInterNetworkV6Address { get; set; } = IPAddress.IPv6Any;

    public bool ReuseAddress { get; set; }
  }
}
