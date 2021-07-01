// Decompiled with JetBrains decompiler
// Type: MQTTnet.Extensions.MqttClientOptionsBuilderExtension
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using MQTTnet.Client.Options;

namespace MQTTnet.Extensions
{
  public static class MqttClientOptionsBuilderExtension
  {
    public static MqttClientOptionsBuilder WithConnectionUri(
      this MqttClientOptionsBuilder builder,
      Uri uri)
    {
      var port = uri.IsDefaultPort ? new int?() : uri.Port;
      var lower = uri.Scheme.ToLower();
      if (!(lower == "tcp") && !(lower == "mqtt"))
      {
        if (!(lower == "mqtts"))
        {
          if (!(lower == "ws") && !(lower == "wss"))
            throw new ArgumentException("Unexpected scheme in uri.");
          builder.WithWebSocketServer(uri.ToString());
        }
        else
          builder.WithTcpServer(uri.Host, port).WithTls();
      }
      else
        builder.WithTcpServer(uri.Host, port);
      if (!string.IsNullOrEmpty(uri.UserInfo))
      {
        var strArray = uri.UserInfo.Split(':');
        var username = strArray[0];
        var password = strArray.Length > 1 ? strArray[1] : "";
        builder.WithCredentials(username, password);
      }
      return builder;
    }

    public static MqttClientOptionsBuilder WithConnectionUri(
      this MqttClientOptionsBuilder builder,
      string uri)
    {
      return builder.WithConnectionUri(new Uri(uri, UriKind.Absolute));
    }
  }
}
