// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.IMqttClientFactory
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using MQTTnet.Adapter;
using MQTTnet.Diagnostics;
using MQTTnet.LowLevelClient;

namespace MQTTnet.Client
{
  public interface IMqttClientFactory
  {
    IMqttFactory UseClientAdapterFactory(IMqttClientAdapterFactory clientAdapterFactory);

    ILowLevelMqttClient CreateLowLevelMqttClient();

    ILowLevelMqttClient CreateLowLevelMqttClient(IMqttNetLogger logger);

    ILowLevelMqttClient CreateLowLevelMqttClient(
      IMqttClientAdapterFactory clientAdapterFactory);

    ILowLevelMqttClient CreateLowLevelMqttClient(
      IMqttNetLogger logger,
      IMqttClientAdapterFactory clientAdapterFactory);

    IMqttClient CreateMqttClient();

    IMqttClient CreateMqttClient(IMqttNetLogger logger);

    IMqttClient CreateMqttClient(IMqttClientAdapterFactory adapterFactory);

    IMqttClient CreateMqttClient(
      IMqttNetLogger logger,
      IMqttClientAdapterFactory adapterFactory);
  }
}
