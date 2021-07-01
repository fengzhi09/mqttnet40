// Decompiled with JetBrains decompiler
// Type: MQTTnet.Internal.AsyncQueue`1
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTnet.Internal
{
    public sealed class AsyncQueue<TItem> : IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);
        private ConcurrentQueue<TItem> _queue = new ConcurrentQueue<TItem>();

        public int Count => _queue.Count;

        public void Enqueue(TItem item)
        {
            _queue.Enqueue(item);
            _semaphore.Release();
        }

        public async Task<AsyncQueueDequeueResult<TItem>> TryDequeueAsync(
            CancellationToken cancellationToken)
        {
            var result = default(TItem);
            var success = false;
            await TaskExtension.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        if (_semaphore == null)
                        {
                            result = default;
                            success = false;
                            break;
                        }

                        _semaphore.Wait(cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException)
                    {
                        result = default;
                        success = false;
                        break;
                    }

                    if (_queue.TryDequeue(out var item))
                    {
                        result = item;
                        success = true;
                        break;
                    }
                }
            });
            return new AsyncQueueDequeueResult<TItem>(success, result);
        }

        public AsyncQueueDequeueResult<TItem> TryDequeue()
        {
            return _queue.TryDequeue(out var result)
                ? new AsyncQueueDequeueResult<TItem>(true, result)
                : new AsyncQueueDequeueResult<TItem>(false, default(TItem));
        }

        public void Clear() => Interlocked.Exchange(ref _queue, new ConcurrentQueue<TItem>());

        public void Dispose() => _semaphore?.Dispose();
    }
}