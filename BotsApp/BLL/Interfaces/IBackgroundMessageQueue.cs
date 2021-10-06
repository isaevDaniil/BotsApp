using BotsApp.BLL.BusinessModels;
using System.Threading;
using System.Threading.Tasks;

namespace BotsApp.BLL.Interfaces
{
    public interface IBackgroundMessageQueue
    {
        ValueTask EnqueueMessageDataAsync(MessageData messageData);

        ValueTask<MessageData> DequeueMessageDataAsync(CancellationToken cancellationToken);
    }
}
