// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.Status.MqttSessionStatus
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MQTTnet.Server.Status
{
  public class MqttSessionStatus : IMqttSessionStatus
  {
    private readonly MqttClientSession _session;
    private readonly MqttClientSessionsManager _sessionsManager;

    public MqttSessionStatus(MqttClientSession session, MqttClientSessionsManager sessionsManager)
    {
      _session = session ?? throw new ArgumentNullException(nameof (session));
      _sessionsManager = sessionsManager ?? throw new ArgumentNullException(nameof (sessionsManager));
    }

    public string ClientId { get; set; }

    public long PendingApplicationMessagesCount { get; set; }

    public DateTime CreatedTimestamp { get; set; }

    public IDictionary<object, object> Items { get; set; }

    public Task DeleteAsync() => _sessionsManager.DeleteSessionAsync(ClientId);

    public Task ClearPendingApplicationMessagesAsync()
    {
      _session.ApplicationMessagesQueue.Clear();
      return TaskExtension.FromResult(0);
    }
  }
}
