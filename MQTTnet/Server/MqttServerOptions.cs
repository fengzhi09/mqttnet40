// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerOptions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Server
{
  public class MqttServerOptions : IMqttServerOptions
  {
    public MqttServerTcpEndpointOptions DefaultEndpointOptions { get; } = new MqttServerTcpEndpointOptions();

    public MqttServerTlsTcpEndpointOptions TlsEndpointOptions { get; } = new MqttServerTlsTcpEndpointOptions();

    public string ClientId { get; set; }

    public bool EnablePersistentSessions { get; set; }

    public int MaxPendingMessagesPerClient { get; set; } = 250;

    public MqttPendingMessagesOverflowStrategy PendingMessagesOverflowStrategy { get; set; }

    public TimeSpan DefaultCommunicationTimeout { get; set; } = TimeSpan.FromSeconds(15.0);

    public IMqttServerConnectionValidator ConnectionValidator { get; set; }

    public IMqttServerApplicationMessageInterceptor ApplicationMessageInterceptor { get; set; }

    public IMqttServerClientMessageQueueInterceptor ClientMessageQueueInterceptor { get; set; }

    public IMqttServerSubscriptionInterceptor SubscriptionInterceptor { get; set; }

    public IMqttServerUnsubscriptionInterceptor UnsubscriptionInterceptor { get; set; }

    public IMqttServerStorage Storage { get; set; }

    public IMqttRetainedMessagesManager RetainedMessagesManager { get; set; } = (IMqttRetainedMessagesManager) new MqttRetainedMessagesManager();

    public IMqttServerApplicationMessageInterceptor UndeliveredMessageInterceptor { get; set; }
  }
}
