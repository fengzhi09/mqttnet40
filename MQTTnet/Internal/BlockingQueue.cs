// Decompiled with JetBrains decompiler
// Type: MQTTnet.Internal.BlockingQueue`1
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace MQTTnet.Internal
{
  public sealed class BlockingQueue<TItem> : IDisposable
  {
    private readonly object _syncRoot = new object();
    private readonly LinkedList<TItem> _items = new LinkedList<TItem>();
    private ManualResetEventSlim _gate = new ManualResetEventSlim(false);

    public int Count
    {
      get
      {
        lock (_syncRoot)
          return _items.Count;
      }
    }

    public void Enqueue(TItem item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      lock (_syncRoot)
      {
        _items.AddLast(item);
        _gate?.Set();
      }
    }

    public TItem Dequeue(CancellationToken cancellationToken = default (CancellationToken))
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        lock (_syncRoot)
        {
          if (_items.Count > 0)
          {
            var obj = _items.First.Value;
            _items.RemoveFirst();
            return obj;
          }
          if (_items.Count == 0)
            _gate?.Reset();
        }
        _gate?.Wait(cancellationToken);
      }
      throw new OperationCanceledException();
    }

    public TItem PeekAndWait(CancellationToken cancellationToken = default (CancellationToken))
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        lock (_syncRoot)
        {
          if (_items.Count > 0)
            return _items.First.Value;
          if (_items.Count == 0)
            _gate?.Reset();
        }
        _gate?.Wait(cancellationToken);
      }
      throw new OperationCanceledException();
    }

    public void RemoveFirst(Predicate<TItem> match)
    {
      if (match == null)
        throw new ArgumentNullException(nameof (match));
      lock (_syncRoot)
      {
        if (_items.Count <= 0 || !match(_items.First.Value))
          return;
        _items.RemoveFirst();
      }
    }

    public TItem RemoveFirst()
    {
      lock (_syncRoot)
      {
        var first = _items.First;
        _items.RemoveFirst();
        return first.Value;
      }
    }

    public void Clear()
    {
      lock (_syncRoot)
        _items.Clear();
    }

    public void Dispose()
    {
      _gate?.Dispose();
      _gate = null;
    }
  }
}
