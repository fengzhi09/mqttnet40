// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.IMqttServer
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet.Server.Status;

namespace MQTTnet.Server
{
  public interface IMqttServer : 
    IApplicationMessageReceiver,
    IApplicationMessagePublisher,
    IDisposable
  {
    bool IsStarted { get; }

    IMqttServerStartedHandler StartedHandler { get; set; }

    IMqttServerStoppedHandler StoppedHandler { get; set; }

    IMqttServerClientConnectedHandler ClientConnectedHandler { get; set; }

    IMqttServerClientDisconnectedHandler ClientDisconnectedHandler { get; set; }

    IMqttServerClientSubscribedTopicHandler ClientSubscribedTopicHandler { get; set; }

    IMqttServerClientUnsubscribedTopicHandler ClientUnsubscribedTopicHandler { get; set; }

    IMqttServerOptions Options { get; }

    Task<IList<IMqttClientStatus>> GetClientStatusAsync();

    Task<IList<IMqttSessionStatus>> GetSessionStatusAsync();

    Task<IList<MqttApplicationMessage>> GetRetainedApplicationMessagesAsync();

    Task ClearRetainedApplicationMessagesAsync();

    Task SubscribeAsync(string clientId, ICollection<MqttTopicFilter> topicFilters);

    Task UnsubscribeAsync(string clientId, ICollection<string> topicFilters);

    Task StartAsync(IMqttServerOptions options);

    Task StopAsync();
  }
}
