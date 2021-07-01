// Decompiled with JetBrains decompiler
// Type: MQTTnet.Internal.Disposable
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;

namespace MQTTnet.Internal
{
  public abstract class Disposable : IDisposable
  {
    protected bool IsDisposed { get; private set; }

    protected void ThrowIfDisposed()
    {
      if (IsDisposed)
        throw new ObjectDisposedException(GetType().Name);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
      if (IsDisposed)
        return;
      IsDisposed = true;
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
