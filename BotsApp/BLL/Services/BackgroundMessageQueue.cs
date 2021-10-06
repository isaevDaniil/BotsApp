using BotsApp.BLL.BusinessModels;
using BotsApp.BLL.Interfaces;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BotsApp.BLL.Services
{
    public class BackgroundMessageQueue : IBackgroundMessageQueue
    {
        private readonly Channel<MessageData> _messageQueue;

        public BackgroundMessageQueue(int capacity)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _messageQueue = Channel.CreateBounded<MessageData>(options);
        }

        public async ValueTask<MessageData> DequeueMessageDataAsync(CancellationToken cancellationToken)
        {
            var message = await _messageQueue.Reader.ReadAsync(cancellationToken);
            return message;
        }

        public async ValueTask EnqueueMessageDataAsync(MessageData messageData)
        {
            if (messageData == null)
            {
                throw new ArgumentNullException(nameof(messageData));
            }
            await _messageQueue.Writer.WriteAsync(messageData);
        }
    }
}
