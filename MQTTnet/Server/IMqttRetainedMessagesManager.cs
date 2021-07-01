// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.IMqttRetainedMessagesManager
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet.Diagnostics;

namespace MQTTnet.Server
{
  public interface IMqttRetainedMessagesManager
  {
    Task Start(IMqttServerOptions options, IMqttNetLogger logger);

    Task LoadMessagesAsync();

    Task ClearMessagesAsync();

    Task HandleMessageAsync(string clientId, MqttApplicationMessage applicationMessage);

    Task<IList<MqttApplicationMessage>> GetMessagesAsync();

    Task<IList<MqttApplicationMessage>> GetSubscribedMessagesAsync(
      ICollection<MqttTopicFilter> topicFilters);
  }
}
