using BotsApp.BLL.BusinessModels;

namespace BotsApp.Bots.Interfaces
{
    public interface IBotManager
    {
        public IBot GetBotInstanceByName(string botName);

        public void EnqueueMessageDataAsync(MessageData messageData);
    }
}
