// Decompiled with JetBrains decompiler
// Type: MQTTnet.Extensions.UserPropertyExtension
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Linq;

namespace MQTTnet.Extensions
{
  public static class UserPropertyExtension
  {
    public static string GetUserProperty(
      this MqttApplicationMessage message,
      string propertyName,
      StringComparison comparisonType = StringComparison.Ordinal)
    {
      if (message == null)
        throw new ArgumentNullException(nameof (message));
      if (propertyName == null)
        throw new ArgumentNullException(nameof (propertyName));
      var userProperties = message.UserProperties;
      if (userProperties == null)
        return null;
      return userProperties.SingleOrDefault(up => up.Name.Equals(propertyName, comparisonType))?.Value;
    }
  }
}
