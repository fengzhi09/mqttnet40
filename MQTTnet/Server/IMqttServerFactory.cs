// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.IMqttServerFactory
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using MQTTnet.Adapter;
using MQTTnet.Diagnostics;

namespace MQTTnet.Server
{
  public interface IMqttServerFactory
  {
    IList<Func<IMqttFactory, IMqttServerAdapter>> DefaultServerAdapters { get; }

    IMqttServer CreateMqttServer();

    IMqttServer CreateMqttServer(IMqttNetLogger logger);

    IMqttServer CreateMqttServer(IEnumerable<IMqttServerAdapter> adapters);

    IMqttServer CreateMqttServer(
      IEnumerable<IMqttServerAdapter> adapters,
      IMqttNetLogger logger);
  }
}
