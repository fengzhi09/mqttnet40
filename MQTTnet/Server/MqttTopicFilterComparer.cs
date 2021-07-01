// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttTopicFilterComparer
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Server
{
  public static class MqttTopicFilterComparer
  {
    private const char LevelSeparator = '/';
    private const char MultiLevelWildcard = '#';
    private const char SingleLevelWildcard = '+';

    public static bool IsMatch(string topic, string filter)
    {
      if (string.IsNullOrEmpty(topic))
        throw new ArgumentNullException(nameof (topic));
      if (string.IsNullOrEmpty(filter))
        throw new ArgumentNullException(nameof (filter));
      var index1 = 0;
      var length1 = filter.Length;
      var index2 = 0;
      var length2 = topic.Length;
      while (index1 < length1 && index2 < length2)
      {
        if (filter[index1] == topic[index2])
        {
          if (index2 == length2 - 1 && index1 == length1 - 3 && (filter[index1 + 1] == '/' && filter[index1 + 2] == '#'))
            return true;
          ++index1;
          ++index2;
          if (index1 == length1 && index2 == length2)
            return true;
          if (index2 == length2 && index1 == length1 - 1 && filter[index1] == '+')
            return index1 <= 0 || filter[index1 - 1] == '/';
        }
        else if (filter[index1] == '+')
        {
          if (index1 > 0 && filter[index1 - 1] != '/' || index1 < length1 - 1 && filter[index1 + 1] != '/')
            return false;
          ++index1;
          while (index2 < length2 && topic[index2] != '/')
            ++index2;
          if (index2 == length2 && index1 == length1)
            return true;
        }
        else
          return filter[index1] == '#' ? (index1 <= 0 || filter[index1 - 1] == '/') && index1 + 1 == length1 : index1 > 0 && index1 + 2 == length1 && (index2 == length2 && filter[index1 - 1] == '+') && (filter[index1] == '/' && filter[index1 + 1] == '#');
      }
      return false;
    }
  }
}
