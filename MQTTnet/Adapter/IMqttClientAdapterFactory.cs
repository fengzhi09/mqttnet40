// Decompiled with JetBrains decompiler
// Type: MQTTnet.Adapter.IMqttClientAdapterFactory
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Client.Options;

namespace MQTTnet.Adapter
{
  public interface IMqttClientAdapterFactory
  {
    IMqttChannelAdapter CreateClientAdapter(IMqttClientOptions options);
  }
}
