// Decompiled with JetBrains decompiler
// Type: MQTTnet.Server.MqttClientSessionApplicationMessagesQueue
// Assembly: MQTTnet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A57D64C8-A58A-4661-AABB-22ABAFCAAE1A
// Assembly location: C:\Users\ace12\Documents\xinchengbio\code\xc_client\DllMerge\dlls\MQTTnet.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Internal;
using MQTTnet.Protocol;

namespace MQTTnet.Server
{
  public class MqttClientSessionApplicationMessagesQueue : Disposable
  {
    private readonly AsyncQueue<MqttQueuedApplicationMessage> _messageQueue = new AsyncQueue<MqttQueuedApplicationMessage>();
    private readonly IMqttServerOptions _options;

    public MqttClientSessionApplicationMessagesQueue(IMqttServerOptions options) => _options = options ?? throw new ArgumentNullException(nameof (options));

    public int Count => _messageQueue.Count;

    public void Enqueue(
      MqttApplicationMessage applicationMessage,
      string senderClientId,
      MqttQualityOfServiceLevel qualityOfServiceLevel,
      bool isRetainedMessage)
    {
      if (applicationMessage == null)
        throw new ArgumentNullException(nameof (applicationMessage));
      Enqueue(new MqttQueuedApplicationMessage
      {
        ApplicationMessage = applicationMessage,
        SenderClientId = senderClientId,
        QualityOfServiceLevel = qualityOfServiceLevel,
        IsRetainedMessage = isRetainedMessage
      });
    }

    public void Clear() => _messageQueue.Clear();

    public async Task<MqttQueuedApplicationMessage> TakeAsync(
      CancellationToken cancellationToken)
    {
      var queueDequeueResult = await _messageQueue.TryDequeueAsync(cancellationToken).ConfigureAwait(false);
      return queueDequeueResult.IsSuccess ? queueDequeueResult.Item : null;
    }

    public void Enqueue(
      MqttQueuedApplicationMessage queuedApplicationMessage)
    {
      if (queuedApplicationMessage == null)
        throw new ArgumentNullException(nameof (queuedApplicationMessage));
      lock (_messageQueue)
      {
        if (_messageQueue.Count >= _options.MaxPendingMessagesPerClient)
        {
          if (_options.PendingMessagesOverflowStrategy == MqttPendingMessagesOverflowStrategy.DropNewMessage)
            return;
          if (_options.PendingMessagesOverflowStrategy == MqttPendingMessagesOverflowStrategy.DropOldestQueuedMessage)
            _messageQueue.TryDequeue();
        }
        _messageQueue.Enqueue(queuedApplicationMessage);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        _messageQueue.Dispose();
      base.Dispose(disposing);
    }
  }
}
