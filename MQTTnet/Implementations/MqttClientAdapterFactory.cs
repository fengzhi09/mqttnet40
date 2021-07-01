// Decompiled with JetBrains decompiler
// Type: MQTTnet.Implementations.MqttClientAdapterFactory
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using MQTTnet.Adapter;
using MQTTnet.Client.Options;
using MQTTnet.Diagnostics;
using MQTTnet.Formatter;

namespace MQTTnet.Implementations
{
  public class MqttClientAdapterFactory : IMqttClientAdapterFactory
  {
    private readonly IMqttNetLogger _logger;

    public MqttClientAdapterFactory(IMqttNetLogger logger) => _logger = logger ?? throw new ArgumentNullException(nameof (logger));

    public IMqttChannelAdapter CreateClientAdapter(IMqttClientOptions options)
    {
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      if (options.ChannelOptions is MqttClientTcpOptions)
        return new MqttChannelAdapter(new MqttTcpChannel(options), new MqttPacketFormatterAdapter(options.ProtocolVersion, new MqttPacketWriter()), _logger);
      throw new NotSupportedException();
    }
  }
}
