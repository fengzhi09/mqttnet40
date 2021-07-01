// Decompiled with JetBrains decompiler
// Type: MQTTnet.Internal.MqttTaskTimeout
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Exceptions;

namespace MQTTnet.Internal
{
  public static class MqttTaskTimeout
  {
    public static async Task WaitAsync(
      Func<CancellationToken, Task> action,
      TimeSpan timeout,
      CancellationToken cancellationToken)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      using (var timeoutCts = new CancellationTokenSource())
      {
        timeoutCts.CancelAfter(timeout);
        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken))
        {
          try
          {
            await action(linkedCts.Token).ConfigureAwait(false);
          }
          catch (OperationCanceledException ex)
          {
            if ((!timeoutCts.IsCancellationRequested ? 0 : (!cancellationToken.IsCancellationRequested ? 1 : 0)) != 0)
              throw new MqttCommunicationTimedOutException(ex);
            throw;
          }
        }
      }
    }

    public static async Task<TResult> WaitAsync<TResult>(
      Func<CancellationToken, Task<TResult>> action,
      TimeSpan timeout,
      CancellationToken cancellationToken)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TResult result;
      using (var timeoutCts = new CancellationTokenSource())
      {
        timeoutCts.CancelAfter(timeout);
        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken))
        {
          try
          {
            result = await action(linkedCts.Token).ConfigureAwait(false);
          }
          catch (OperationCanceledException ex)
          {
            if ((!timeoutCts.IsCancellationRequested ? 0 : (!cancellationToken.IsCancellationRequested ? 1 : 0)) != 0)
              throw new MqttCommunicationTimedOutException(ex);
            throw;
          }
        }
      }
      return result;
    }
  }
}
