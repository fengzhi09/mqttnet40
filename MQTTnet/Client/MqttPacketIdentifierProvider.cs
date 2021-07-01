// Decompiled with JetBrains decompiler
// Type: MQTTnet.Client.MqttPacketIdentifierProvider
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

namespace MQTTnet.Client
{
  public class MqttPacketIdentifierProvider
  {
    private readonly object _syncRoot = new object();
    private ushort _value;

    public void Reset()
    {
      lock (_syncRoot)
        _value = 0;
    }

    public ushort GetNextPacketIdentifier()
    {
      lock (_syncRoot)
      {
        ++_value;
        if (_value == 0)
          _value = 1;
        return _value;
      }
    }
  }
}
