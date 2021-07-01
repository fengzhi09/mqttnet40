// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.IMqttClient
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.ExtendedAuthenticationExchange;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;

namespace MQTTnet.Client
{
  public interface IMqttClient : 
    IApplicationMessageReceiver,
    IApplicationMessagePublisher,
    IDisposable
  {
    bool IsConnected { get; }

    IMqttClientOptions Options { get; }

    IMqttClientConnectedHandler ConnectedHandler { get; set; }

    IMqttClientDisconnectedHandler DisconnectedHandler { get; set; }

    Task<MqttClientAuthenticateResult> ConnectAsync(
      IMqttClientOptions options,
      CancellationToken cancellationToken);

    Task DisconnectAsync(
      MqttClientDisconnectOptions options,
      CancellationToken cancellationToken);

    Task PingAsync(CancellationToken cancellationToken);

    Task SendExtendedAuthenticationExchangeDataAsync(
      MqttExtendedAuthenticationExchangeData data,
      CancellationToken cancellationToken);

    Task<MqttClientSubscribeResult> SubscribeAsync(
      MqttClientSubscribeOptions options,
      CancellationToken cancellationToken);

    Task<MqttClientUnsubscribeResult> UnsubscribeAsync(
      MqttClientUnsubscribeOptions options,
      CancellationToken cancellationToken);
  }
}
