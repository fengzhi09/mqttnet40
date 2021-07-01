// Decompiled with JetBrains decompiler
// Type: MQTTnet.MqttFactory
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Linq;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Implementations;
using MQTTnet.LowLevelClient;
using MQTTnet.Server;

namespace MQTTnet
{
  public sealed class MqttFactory : IMqttFactory, IMqttClientFactory, IMqttServerFactory
  {
    private IMqttClientAdapterFactory _clientAdapterFactory;

    public MqttFactory()
      : this(new MqttNetLogger())
    {
    }

    public MqttFactory(IMqttNetLogger logger)
    {
      DefaultLogger = logger ?? throw new ArgumentNullException(nameof (logger));
      _clientAdapterFactory = new MqttClientAdapterFactory(logger);
    }

    public IMqttNetLogger DefaultLogger { get; }

    public IList<Func<IMqttFactory, IMqttServerAdapter>> DefaultServerAdapters { get; } = (IList<Func<IMqttFactory, IMqttServerAdapter>>) new List<Func<IMqttFactory, IMqttServerAdapter>>
    {
      factory => (IMqttServerAdapter) new MqttTcpServerAdapter(factory.DefaultLogger)
    };

    public IDictionary<object, object> Properties { get; } = (IDictionary<object, object>) new Dictionary<object, object>();

    public IMqttFactory UseClientAdapterFactory(
      IMqttClientAdapterFactory clientAdapterFactory)
    {
      _clientAdapterFactory = clientAdapterFactory ?? throw new ArgumentNullException(nameof (clientAdapterFactory));
      return this;
    }

    public ILowLevelMqttClient CreateLowLevelMqttClient() => CreateLowLevelMqttClient(DefaultLogger);

    public ILowLevelMqttClient CreateLowLevelMqttClient(IMqttNetLogger logger) => logger != null ? (ILowLevelMqttClient) new LowLevelMqttClient(_clientAdapterFactory, logger) : throw new ArgumentNullException(nameof (logger));

    public ILowLevelMqttClient CreateLowLevelMqttClient(
      IMqttClientAdapterFactory clientAdapterFactory)
    {
      if (clientAdapterFactory == null)
        throw new ArgumentNullException(nameof (clientAdapterFactory));
      return new LowLevelMqttClient(_clientAdapterFactory, DefaultLogger);
    }

    public ILowLevelMqttClient CreateLowLevelMqttClient(
      IMqttNetLogger logger,
      IMqttClientAdapterFactory clientAdapterFactory)
    {
      if (logger == null)
        throw new ArgumentNullException(nameof (logger));
      if (clientAdapterFactory == null)
        throw new ArgumentNullException(nameof (clientAdapterFactory));
      return new LowLevelMqttClient(_clientAdapterFactory, logger);
    }

    public IMqttClient CreateMqttClient() => CreateMqttClient(DefaultLogger);

    public IMqttClient CreateMqttClient(IMqttNetLogger logger) => logger != null ? (IMqttClient) new MqttClient(_clientAdapterFactory, logger) : throw new ArgumentNullException(nameof (logger));

    public IMqttClient CreateMqttClient(IMqttClientAdapterFactory clientAdapterFactory) => clientAdapterFactory != null ? (IMqttClient) new MqttClient(clientAdapterFactory, DefaultLogger) : throw new ArgumentNullException(nameof (clientAdapterFactory));

    public IMqttClient CreateMqttClient(
      IMqttNetLogger logger,
      IMqttClientAdapterFactory clientAdapterFactory)
    {
      if (logger == null)
        throw new ArgumentNullException(nameof (logger));
      return clientAdapterFactory != null ? (IMqttClient) new MqttClient(clientAdapterFactory, logger) : throw new ArgumentNullException(nameof (clientAdapterFactory));
    }

    public IMqttServer CreateMqttServer() => CreateMqttServer(DefaultLogger);

    public IMqttServer CreateMqttServer(IMqttNetLogger logger)
    {
      if (logger == null)
        throw new ArgumentNullException(nameof (logger));
      return CreateMqttServer(DefaultServerAdapters.Select(a => a(this)), logger);
    }

    public IMqttServer CreateMqttServer(
      IEnumerable<IMqttServerAdapter> serverAdapters,
      IMqttNetLogger logger)
    {
      if (serverAdapters == null)
        throw new ArgumentNullException(nameof (serverAdapters));
      return logger != null ? (IMqttServer) new MqttServer(serverAdapters, logger) : throw new ArgumentNullException(nameof (logger));
    }

    public IMqttServer CreateMqttServer(IEnumerable<IMqttServerAdapter> serverAdapters) => serverAdapters != null ? (IMqttServer) new MqttServer(serverAdapters, DefaultLogger) : throw new ArgumentNullException(nameof (serverAdapters));
  }
}
