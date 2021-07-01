// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerOptionsBuilder
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using MQTTnet.Certificates;

namespace MQTTnet.Server
{
  public class MqttServerOptionsBuilder
  {
    private readonly MqttServerOptions _options = new MqttServerOptions();

    public MqttServerOptionsBuilder WithConnectionBacklog(int value)
    {
      _options.DefaultEndpointOptions.ConnectionBacklog = value;
      _options.TlsEndpointOptions.ConnectionBacklog = value;
      return this;
    }

    public MqttServerOptionsBuilder WithMaxPendingMessagesPerClient(
      int value)
    {
      _options.MaxPendingMessagesPerClient = value;
      return this;
    }

    public MqttServerOptionsBuilder WithDefaultCommunicationTimeout(
      TimeSpan value)
    {
      _options.DefaultCommunicationTimeout = value;
      return this;
    }

    public MqttServerOptionsBuilder WithDefaultEndpoint()
    {
      _options.DefaultEndpointOptions.IsEnabled = true;
      return this;
    }

    public MqttServerOptionsBuilder WithDefaultEndpointPort(int value)
    {
      _options.DefaultEndpointOptions.Port = value;
      return this;
    }

    public MqttServerOptionsBuilder WithDefaultEndpointBoundIPAddress(
      IPAddress value)
    {
      _options.DefaultEndpointOptions.BoundInterNetworkAddress = value ?? IPAddress.Any;
      return this;
    }

    public MqttServerOptionsBuilder WithDefaultEndpointBoundIPV6Address(
      IPAddress value)
    {
      _options.DefaultEndpointOptions.BoundInterNetworkV6Address = value ?? IPAddress.Any;
      return this;
    }

    public MqttServerOptionsBuilder WithoutDefaultEndpoint()
    {
      _options.DefaultEndpointOptions.IsEnabled = false;
      return this;
    }

    public MqttServerOptionsBuilder WithEncryptedEndpoint()
    {
      _options.TlsEndpointOptions.IsEnabled = true;
      return this;
    }

    public MqttServerOptionsBuilder WithEncryptedEndpointPort(int value)
    {
      _options.TlsEndpointOptions.Port = value;
      return this;
    }

    public MqttServerOptionsBuilder WithEncryptedEndpointBoundIPAddress(
      IPAddress value)
    {
      _options.TlsEndpointOptions.BoundInterNetworkAddress = value;
      return this;
    }

    public MqttServerOptionsBuilder WithEncryptedEndpointBoundIPV6Address(
      IPAddress value)
    {
      _options.TlsEndpointOptions.BoundInterNetworkV6Address = value;
      return this;
    }

    public MqttServerOptionsBuilder WithEncryptionCertificate(
      byte[] value,
      IMqttServerCertificateCredentials credentials = null)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      _options.TlsEndpointOptions.CertificateProvider = new BlobCertificateProvider(value)
      {
        Password = credentials?.Password
      };
      return this;
    }

    public MqttServerOptionsBuilder WithEncryptionCertificate(
      X509Certificate2 certificate)
    {
      _options.TlsEndpointOptions.CertificateProvider = certificate != null ? (ICertificateProvider) new X509CertificateProvider(certificate) : throw new ArgumentNullException(nameof (certificate));
      return this;
    }

    public MqttServerOptionsBuilder WithEncryptionSslProtocol(
      SslProtocols value)
    {
      _options.TlsEndpointOptions.SslProtocol = value;
      return this;
    }

    public MqttServerOptionsBuilder WithClientCertificate(
      RemoteCertificateValidationCallback validationCallback = null,
      bool checkCertificateRevocation = false)
    {
      _options.TlsEndpointOptions.ClientCertificateRequired = true;
      _options.TlsEndpointOptions.CheckCertificateRevocation = checkCertificateRevocation;
      _options.TlsEndpointOptions.RemoteCertificateValidationCallback = validationCallback;
      return this;
    }

    public MqttServerOptionsBuilder WithoutEncryptedEndpoint()
    {
      _options.TlsEndpointOptions.IsEnabled = false;
      return this;
    }

    public MqttServerOptionsBuilder WithRemoteCertificateValidationCallback(
      RemoteCertificateValidationCallback value)
    {
      _options.TlsEndpointOptions.RemoteCertificateValidationCallback = value;
      return this;
    }

    public MqttServerOptionsBuilder WithStorage(IMqttServerStorage value)
    {
      _options.Storage = value;
      return this;
    }

    public MqttServerOptionsBuilder WithRetainedMessagesManager(
      IMqttRetainedMessagesManager value)
    {
      _options.RetainedMessagesManager = value;
      return this;
    }

    public MqttServerOptionsBuilder WithConnectionValidator(
      IMqttServerConnectionValidator value)
    {
      _options.ConnectionValidator = value;
      return this;
    }

    public MqttServerOptionsBuilder WithConnectionValidator(
      Action<MqttConnectionValidatorContext> value)
    {
      _options.ConnectionValidator = new MqttServerConnectionValidatorDelegate(value);
      return this;
    }

    public MqttServerOptionsBuilder WithApplicationMessageInterceptor(
      IMqttServerApplicationMessageInterceptor value)
    {
      _options.ApplicationMessageInterceptor = value;
      return this;
    }

    public MqttServerOptionsBuilder WithApplicationMessageInterceptor(
      Action<MqttApplicationMessageInterceptorContext> value)
    {
      _options.ApplicationMessageInterceptor = new MqttServerApplicationMessageInterceptorDelegate(value);
      return this;
    }

    public MqttServerOptionsBuilder WithSubscriptionInterceptor(
      IMqttServerSubscriptionInterceptor value)
    {
      _options.SubscriptionInterceptor = value;
      return this;
    }

    public MqttServerOptionsBuilder WithUnsubscriptionInterceptor(
      IMqttServerUnsubscriptionInterceptor value)
    {
      _options.UnsubscriptionInterceptor = value;
      return this;
    }

    public MqttServerOptionsBuilder WithSubscriptionInterceptor(
      Action<MqttSubscriptionInterceptorContext> value)
    {
      _options.SubscriptionInterceptor = new MqttServerSubscriptionInterceptorDelegate(value);
      return this;
    }

    public MqttServerOptionsBuilder WithDefaultEndpointReuseAddress()
    {
      _options.DefaultEndpointOptions.ReuseAddress = true;
      return this;
    }

    public MqttServerOptionsBuilder WithTlsEndpointReuseAddress()
    {
      _options.TlsEndpointOptions.ReuseAddress = true;
      return this;
    }

    public MqttServerOptionsBuilder WithPersistentSessions()
    {
      _options.EnablePersistentSessions = true;
      return this;
    }

    public MqttServerOptionsBuilder WithClientId(string value)
    {
      _options.ClientId = value;
      return this;
    }

    public IMqttServerOptions Build() => _options;

    public MqttServerOptionsBuilder WithUndeliveredMessageInterceptor(
      Action<MqttApplicationMessageInterceptorContext> value)
    {
      _options.UndeliveredMessageInterceptor = new MqttServerApplicationMessageInterceptorDelegate(value);
      return this;
    }
  }
}
