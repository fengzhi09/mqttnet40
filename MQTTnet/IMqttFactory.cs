// Decompiled with JetBrains decompiler
// Type: MQTTnet.IMqttFactory
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System.Collections.Generic;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Server;

namespace MQTTnet
{
  public interface IMqttFactory : IMqttClientFactory, IMqttServerFactory
  {
    IMqttNetLogger DefaultLogger { get; }

    IDictionary<object, object> Properties { get; }
  }
}
