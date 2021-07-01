// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Disconnecting.MqttClientDisconnectedEventArgs
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using MQTTnet.Client.Connecting;

namespace MQTTnet.Client.Disconnecting
{
  public class MqttClientDisconnectedEventArgs : EventArgs
  {
    public MqttClientDisconnectedEventArgs(
      bool clientWasConnected,
      Exception exception,
      MqttClientAuthenticateResult authenticateResult,
      MqttClientDisconnectReason reasonCode)
    {
      ClientWasConnected = clientWasConnected;
      Exception = exception;
      AuthenticateResult = authenticateResult;
      ReasonCode = reasonCode;
    }

    public bool ClientWasConnected { get; }

    public Exception Exception { get; }

    public MqttClientAuthenticateResult AuthenticateResult { get; }

    public MqttClientDisconnectReason ReasonCode { get; set; }
  }
}
