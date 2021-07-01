// Decompiled with JetBrains decompiler
// Type: MQTTnet.Certificates.BlobCertificateProvider
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Security.Cryptography.X509Certificates;

namespace MQTTnet.Certificates
{
  public class BlobCertificateProvider : ICertificateProvider
  {
    public BlobCertificateProvider(byte[] blob) => Blob = blob ?? throw new ArgumentNullException(nameof (blob));

    public byte[] Blob { get; }

    public string Password { get; set; }

    public X509Certificate2 GetCertificate() => string.IsNullOrEmpty(Password) ? new X509Certificate2(Blob) : new X509Certificate2(Blob, Password);
  }
}
