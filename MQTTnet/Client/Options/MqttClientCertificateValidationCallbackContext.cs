// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Options.MqttClientCertificateValidationCallbackContext
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MQTTnet.Client.Options
{
  public class MqttClientCertificateValidationCallbackContext
  {
    public X509Certificate Certificate { get; set; }

    public X509Chain Chain { get; set; }

    public SslPolicyErrors SslPolicyErrors { get; set; }

    public IMqttClientChannelOptions ClientOptions { get; set; }
  }
}
