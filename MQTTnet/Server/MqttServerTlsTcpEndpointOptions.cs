// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerTlsTcpEndpointOptions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Net.Security;
using System.Security.Authentication;
using MQTTnet.Certificates;

namespace MQTTnet.Server
{
  public class MqttServerTlsTcpEndpointOptions : MqttServerTcpEndpointBaseOptions
  {
    public MqttServerTlsTcpEndpointOptions() => Port = 8883;


    public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

    public ICertificateProvider CertificateProvider { get; set; }

    public bool ClientCertificateRequired { get; set; }

    public bool CheckCertificateRevocation { get; set; }

    public SslProtocols SslProtocol { get; set; } = SslProtocols.Tls;
  }
}
