// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.MqttClientExtensions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.ExtendedAuthenticationExchange;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;
using MQTTnet.Protocol;

namespace MQTTnet.Client
{
  public static class MqttClientExtensions
  {
    public static IMqttClient UseConnectedHandler(
      this IMqttClient client,
      Func<MqttClientConnectedEventArgs, Task> handler)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return handler == null ? client.UseConnectedHandler((IMqttClientConnectedHandler) null) : client.UseConnectedHandler(new MqttClientConnectedHandlerDelegate(handler));
    }

    public static IMqttClient UseConnectedHandler(
      this IMqttClient client,
      Action<MqttClientConnectedEventArgs> handler)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return handler == null ? client.UseConnectedHandler((IMqttClientConnectedHandler) null) : client.UseConnectedHandler(new MqttClientConnectedHandlerDelegate(handler));
    }

    public static IMqttClient UseConnectedHandler(
      this IMqttClient client,
      IMqttClientConnectedHandler handler)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      client.ConnectedHandler = handler;
      return client;
    }

    public static IMqttClient UseDisconnectedHandler(
      this IMqttClient client,
      Func<MqttClientDisconnectedEventArgs, Task> handler)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return handler == null ? client.UseDisconnectedHandler((IMqttClientDisconnectedHandler) null) : client.UseDisconnectedHandler(new MqttClientDisconnectedHandlerDelegate(handler));
    }

    public static IMqttClient UseDisconnectedHandler(
      this IMqttClient client,
      Action<MqttClientDisconnectedEventArgs> handler)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return handler == null ? client.UseDisconnectedHandler((IMqttClientDisconnectedHandler) null) : client.UseDisconnectedHandler(new MqttClientDisconnectedHandlerDelegate(handler));
    }

    public static IMqttClient UseDisconnectedHandler(
      this IMqttClient client,
      IMqttClientDisconnectedHandler handler)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      client.DisconnectedHandler = handler;
      return client;
    }

    public static IMqttClient UseApplicationMessageReceivedHandler(
      this IMqttClient client,
      Func<MqttApplicationMessageReceivedEventArgs, Task> handler)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return handler == null ? client.UseApplicationMessageReceivedHandler((IMqttApplicationMessageReceivedHandler) null) : client.UseApplicationMessageReceivedHandler(new MqttApplicationMessageReceivedHandlerDelegate(handler));
    }

    public static IMqttClient UseApplicationMessageReceivedHandler(
      this IMqttClient client,
      Action<MqttApplicationMessageReceivedEventArgs> handler)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return handler == null ? client.UseApplicationMessageReceivedHandler((IMqttApplicationMessageReceivedHandler) null) : client.UseApplicationMessageReceivedHandler(new MqttApplicationMessageReceivedHandlerDelegate(handler));
    }

    public static IMqttClient UseApplicationMessageReceivedHandler(
      this IMqttClient client,
      IMqttApplicationMessageReceivedHandler handler)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      client.ApplicationMessageReceivedHandler = handler;
      return client;
    }

    public static Task ReconnectAsync(this IMqttClient client)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return client.Options != null ? (Task) client.ConnectAsync(client.Options) : throw new InvalidOperationException("_ReconnectAsync_ can be used only if _ConnectAsync_ was called before.");
    }

    public static Task DisconnectAsync(this IMqttClient client) => client != null ? client.DisconnectAsync(new MqttClientDisconnectOptions()) : throw new ArgumentNullException(nameof (client));

    public static Task<MqttClientSubscribeResult> SubscribeAsync(
      this IMqttClient client,
      params MqttTopicFilter[] topicFilters)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (topicFilters == null)
        throw new ArgumentNullException(nameof (topicFilters));
      var options = new MqttClientSubscribeOptions();
      options.TopicFilters.AddRange(topicFilters);
      return client.SubscribeAsync(options);
    }

    public static Task<MqttClientSubscribeResult> SubscribeAsync(
      this IMqttClient client,
      string topic,
      MqttQualityOfServiceLevel qualityOfServiceLevel)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (topic == null)
        throw new ArgumentNullException(nameof (topic));
      return client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).WithQualityOfServiceLevel(qualityOfServiceLevel).Build());
    }

    public static Task<MqttClientSubscribeResult> SubscribeAsync(
      this IMqttClient client,
      string topic)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return topic != null ? client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build()) : throw new ArgumentNullException(nameof (topic));
    }

    public static Task<MqttClientUnsubscribeResult> UnsubscribeAsync(
      this IMqttClient client,
      params string[] topicFilters)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (topicFilters == null)
        throw new ArgumentNullException(nameof (topicFilters));
      var options = new MqttClientUnsubscribeOptions();
      options.TopicFilters.AddRange(topicFilters);
      return client.UnsubscribeAsync(options);
    }

    public static Task<MqttClientAuthenticateResult> ConnectAsync(
      this IMqttClient client,
      IMqttClientOptions options)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return client.ConnectAsync(options, CancellationToken.None);
    }

    public static Task DisconnectAsync(
      this IMqttClient client,
      MqttClientDisconnectOptions options)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return client.DisconnectAsync(options, CancellationToken.None);
    }

    public static Task SendExtendedAuthenticationExchangeDataAsync(
      this IMqttClient client,
      MqttExtendedAuthenticationExchangeData data)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return client.SendExtendedAuthenticationExchangeDataAsync(data, CancellationToken.None);
    }

    public static Task<MqttClientSubscribeResult> SubscribeAsync(
      this IMqttClient client,
      MqttClientSubscribeOptions options)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return client.SubscribeAsync(options, CancellationToken.None);
    }

    public static Task<MqttClientUnsubscribeResult> UnsubscribeAsync(
      this IMqttClient client,
      MqttClientUnsubscribeOptions options)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return client.UnsubscribeAsync(options, CancellationToken.None);
    }

    public static Task<MqttClientPublishResult> PublishAsync(
      this IMqttClient client,
      MqttApplicationMessage applicationMessage)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (applicationMessage == null)
        throw new ArgumentNullException(nameof (applicationMessage));
      return client.PublishAsync(applicationMessage, CancellationToken.None);
    }

    public static async Task PublishAsync(
      this IMqttClient client,
      IEnumerable<MqttApplicationMessage> applicationMessages)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (applicationMessages == null)
        throw new ArgumentNullException(nameof (applicationMessages));
      foreach (var applicationMessage in applicationMessages)
      {
        var clientPublishResult = await client.PublishAsync(applicationMessage).ConfigureAwait(false);
      }
    }

    public static Task<MqttClientPublishResult> PublishAsync(
      this IMqttClient client,
      string topic)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return topic != null ? client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(topic).Build()) : throw new ArgumentNullException(nameof (topic));
    }

    public static Task<MqttClientPublishResult> PublishAsync(
      this IMqttClient client,
      string topic,
      IEnumerable<byte> payload)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (topic == null)
        throw new ArgumentNullException(nameof (topic));
      return client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payload).Build());
    }

    public static Task<MqttClientPublishResult> PublishAsync(
      this IMqttClient client,
      string topic,
      string payload)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (topic == null)
        throw new ArgumentNullException(nameof (topic));
      return client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payload).Build());
    }

    public static Task<MqttClientPublishResult> PublishAsync(
      this IMqttClient client,
      string topic,
      string payload,
      MqttQualityOfServiceLevel qualityOfServiceLevel)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (topic == null)
        throw new ArgumentNullException(nameof (topic));
      return client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payload).WithQualityOfServiceLevel(qualityOfServiceLevel).Build());
    }

    public static Task<MqttClientPublishResult> PublishAsync(
      this IMqttClient client,
      string topic,
      string payload,
      MqttQualityOfServiceLevel qualityOfServiceLevel,
      bool retain)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (topic == null)
        throw new ArgumentNullException(nameof (topic));
      return client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payload).WithQualityOfServiceLevel(qualityOfServiceLevel).WithRetainFlag(retain).Build());
    }

    public static Task<MqttClientPublishResult> PublishAsync(
      this IMqttClient client,
      string topic,
      string payload,
      bool retain)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      if (topic == null)
        throw new ArgumentNullException(nameof (topic));
      return client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payload).WithRetainFlag(retain).Build());
    }
  }
}
