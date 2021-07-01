// Decompiled with JetBrains decompiler
// Type: MQTTnet.Packets.MqttUserProperty
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Packets
{
  public class MqttUserProperty
  {
    public MqttUserProperty(string name, string value)
    {
      Name = name ?? throw new ArgumentNullException(nameof (name));
      Value = value ?? throw new ArgumentNullException(nameof (value));
    }

    public string Name { get; }

    public string Value { get; }

    public override int GetHashCode() => Name.GetHashCode() ^ Value.GetHashCode();

    public override bool Equals(object other) => Equals(other as MqttUserProperty);

    public bool Equals(MqttUserProperty other)
    {
      if (other == null)
        return false;
      if (other == this)
        return true;
      return string.Equals(Name, other.Name, StringComparison.Ordinal) && string.Equals(Value, other.Value, StringComparison.Ordinal);
    }
  }
}
