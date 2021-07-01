// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.IMqttServerOptions
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Server
{
  public interface IMqttServerOptions
  {
    string ClientId { get; set; }

    bool EnablePersistentSessions { get; }

    int MaxPendingMessagesPerClient { get; }

    MqttPendingMessagesOverflowStrategy PendingMessagesOverflowStrategy { get; }

    TimeSpan DefaultCommunicationTimeout { get; }

    IMqttServerConnectionValidator ConnectionValidator { get; }

    IMqttServerSubscriptionInterceptor SubscriptionInterceptor { get; }

    IMqttServerUnsubscriptionInterceptor UnsubscriptionInterceptor { get; }

    IMqttServerApplicationMessageInterceptor ApplicationMessageInterceptor { get; }

    IMqttServerClientMessageQueueInterceptor ClientMessageQueueInterceptor { get; }

    MqttServerTcpEndpointOptions DefaultEndpointOptions { get; }

    MqttServerTlsTcpEndpointOptions TlsEndpointOptions { get; }

    IMqttServerStorage Storage { get; }

    IMqttRetainedMessagesManager RetainedMessagesManager { get; }

    IMqttServerApplicationMessageInterceptor UndeliveredMessageInterceptor { get; set; }
  }
}
