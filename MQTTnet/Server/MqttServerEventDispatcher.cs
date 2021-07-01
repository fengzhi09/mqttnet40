// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerEventDispatcher
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading.Tasks;
using MQTTnet.Client.Receiving;
using MQTTnet.Diagnostics;

namespace MQTTnet.Server
{
  public sealed class MqttServerEventDispatcher
  {
    private readonly IMqttNetScopedLogger _logger;

    public MqttServerEventDispatcher(IMqttNetLogger logger) => _logger = logger != null ? logger.CreateScopedLogger(nameof (MqttServerEventDispatcher)) : throw new ArgumentNullException(nameof (logger));

    public IMqttServerClientConnectedHandler ClientConnectedHandler { get; set; }

    public IMqttServerClientDisconnectedHandler ClientDisconnectedHandler { get; set; }

    public IMqttServerClientSubscribedTopicHandler ClientSubscribedTopicHandler { get; set; }

    public IMqttServerClientUnsubscribedTopicHandler ClientUnsubscribedTopicHandler { get; set; }

    public IMqttApplicationMessageReceivedHandler ApplicationMessageReceivedHandler { get; set; }

    public async Task SafeNotifyClientConnectedAsync(string clientId)
    {
      try
      {
        var connectedHandler = ClientConnectedHandler;
        if (connectedHandler == null)
          return;
        await connectedHandler.HandleClientConnectedAsync(new MqttServerClientConnectedEventArgs(clientId)).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error while handling custom 'ClientConnected' event.");
      }
    }

    public async Task SafeNotifyClientDisconnectedAsync(
      string clientId,
      MqttClientDisconnectType disconnectType)
    {
      try
      {
        var disconnectedHandler = ClientDisconnectedHandler;
        if (disconnectedHandler == null)
          return;
        await disconnectedHandler.HandleClientDisconnectedAsync(new MqttServerClientDisconnectedEventArgs(clientId, disconnectType)).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error while handling custom 'ClientDisconnected' event.");
      }
    }

    public async Task SafeNotifyClientSubscribedTopicAsync(
      string clientId,
      MqttTopicFilter topicFilter)
    {
      try
      {
        var subscribedTopicHandler = ClientSubscribedTopicHandler;
        if (subscribedTopicHandler == null)
          return;
        await subscribedTopicHandler.HandleClientSubscribedTopicAsync(new MqttServerClientSubscribedTopicEventArgs(clientId, topicFilter)).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error while handling custom 'ClientSubscribedTopic' event.");
      }
    }

    public async Task SafeNotifyClientUnsubscribedTopicAsync(
      string clientId,
      string topicFilter)
    {
      try
      {
        var unsubscribedTopicHandler = ClientUnsubscribedTopicHandler;
        if (unsubscribedTopicHandler == null)
          return;
        await unsubscribedTopicHandler.HandleClientUnsubscribedTopicAsync(new MqttServerClientUnsubscribedTopicEventArgs(clientId, topicFilter)).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error while handling custom 'ClientUnsubscribedTopic' event.");
      }
    }

    public async Task SafeNotifyApplicationMessageReceivedAsync(
      string senderClientId,
      MqttApplicationMessage applicationMessage)
    {
      try
      {
        var messageReceivedHandler = ApplicationMessageReceivedHandler;
        if (messageReceivedHandler == null)
          return;
        await messageReceivedHandler.HandleApplicationMessageReceivedAsync(new MqttApplicationMessageReceivedEventArgs(senderClientId, applicationMessage)).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error while handling custom 'ApplicationMessageReceived' event.");
      }
    }
  }
}
