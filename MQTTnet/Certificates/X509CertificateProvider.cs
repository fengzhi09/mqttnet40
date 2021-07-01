// Decompiled with JetBrains decompiler
// Type: MQTTnet.Certificates.X509CertificateProvider
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Security.Cryptography.X509Certificates;

namespace MQTTnet.Certificates
{
  public class X509CertificateProvider : ICertificateProvider
  {
    private readonly X509Certificate2 _certificate;

    public X509CertificateProvider(X509Certificate2 certificate) => _certificate = certificate ?? throw new ArgumentNullException(nameof (certificate));

    public X509Certificate2 GetCertificate() => _certificate;
  }
}
