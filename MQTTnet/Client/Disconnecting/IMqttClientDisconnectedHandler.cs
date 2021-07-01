// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.Disconnecting.IMqttClientDisconnectedHandler
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Threading.Tasks;

namespace MQTTnet.Client.Disconnecting
{
  public interface IMqttClientDisconnectedHandler
  {
    Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs);
  }
}
