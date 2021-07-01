// Decompiled with JetBrains decompiler
// Type: MQTTnet.TaskExtension
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTnet
{
  public static class TaskExtension
  {
    public static Task<T> FromResult<T>(T v) => Task.Factory.StartNew(() => v);

    public static Task Run(Action action) => Task.Factory.StartNew(action);

    public static Task Run(Action action, CancellationToken cancellationToken) => Task.Factory.StartNew(action, cancellationToken);

    public static Task Delay(int milliseconds, CancellationToken cancellationToken) => Run(() => Thread.Sleep(milliseconds), cancellationToken);

    public static Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken) => Run(() => Thread.Sleep(timeSpan), cancellationToken);
  }
}
