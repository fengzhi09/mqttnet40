// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttServerClientDisconnectedEventArgs
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Server
{
  public class MqttServerClientDisconnectedEventArgs : EventArgs
  {
    public MqttServerClientDisconnectedEventArgs(
      string clientId,
      MqttClientDisconnectType disconnectType)
    {
      ClientId = clientId ?? throw new ArgumentNullException(nameof (clientId));
      DisconnectType = disconnectType;
    }

    public string ClientId { get; }

    public MqttClientDisconnectType DisconnectType { get; }
  }
}
