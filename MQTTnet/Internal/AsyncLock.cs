// Decompiled with JetBrains decompiler
// Type: MQTTnet.Internal.AsyncLock
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTnet.Internal
{
  public sealed class AsyncLock : IDisposable
  {
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly Task<IDisposable> _releaser;

    public AsyncLock() => _releaser = TaskExtension.FromResult<IDisposable>(new Releaser(this));

    public Task<IDisposable> WaitAsync() => WaitAsync(CancellationToken.None);

    public Task<IDisposable> WaitAsync(CancellationToken cancellationToken)
    {
      _semaphore.Wait(cancellationToken);
      return TaskExtension.FromResult(_releaser.Result);
    }

    public void Dispose() => _semaphore?.Dispose();

    private class Releaser : IDisposable
    {
      private readonly AsyncLock _toRelease;

      internal Releaser(AsyncLock toRelease) => _toRelease = toRelease;

      public void Dispose() => _toRelease._semaphore.Release();
    }
  }
}
