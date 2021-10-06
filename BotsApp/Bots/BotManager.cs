using BotsApp.BLL.BusinessModels;
using BotsApp.BLL.Interfaces;
using BotsApp.Bots.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BotsApp.Bots
{
    public class BotManager : IBotManager
    {
        private List<IBot> _bots;
        private IBackgroundMessageQueue _messageQueue;

        public BotManager(IBackgroundMessageQueue messageQueue)
        {
            _bots = new List<IBot>();
            DownloadBots();
            _messageQueue = messageQueue;
        }

        private void DownloadBots()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var botTypes = assembly.GetExportedTypes().Where(t => t.IsClass).Where(t => t.GetInterface("IBot") != null);
            foreach (var botClass in botTypes)
            {
                var botToAdd = (IBot)Activator.CreateInstance(botClass);
                _bots.Add(botToAdd);
            }
        }

        public IBot GetBotInstanceByName(string botName)
        {
            return _bots.FirstOrDefault(b => b.BotName.ToLower() == botName.ToLower());
        }

        public async void EnqueueMessageDataAsync(MessageData messageData)
        {
            await _messageQueue.EnqueueMessageDataAsync(messageData);
        }
    }
}
