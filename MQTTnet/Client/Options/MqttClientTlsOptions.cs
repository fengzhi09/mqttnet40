// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Options.MqttClientTlsOptions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace MQTTnet.Client.Options
{
  public class MqttClientTlsOptions
  {
    public bool UseTls { get; set; }

    public bool IgnoreCertificateRevocationErrors { get; set; }

    public bool IgnoreCertificateChainErrors { get; set; }

    public bool AllowUntrustedCertificates { get; set; }

    public List<X509Certificate> Certificates { get; set; }

    public SslProtocols SslProtocol { get; set; } = SslProtocols.Tls;

    [Obsolete("This property will be removed soon. Use CertificateValidationHandler instead.")]
    public Func<X509Certificate, X509Chain, SslPolicyErrors, IMqttClientOptions, bool> CertificateValidationCallback { get; set; }

    public Func<MqttClientCertificateValidationCallbackContext, bool> CertificateValidationHandler { get; set; }
  }
}
