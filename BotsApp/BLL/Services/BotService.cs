using BotsApp.BLL.DTO;
using BotsApp.BLL.Interfaces;
using BotsApp.Bots.Interfaces;
using System;

namespace BotsApp.BLL.Services
{
    public class BotService : IBotService
    {
        private IBotManager _botManager;

        public BotService(IBotManager botManager)
        {
            _botManager = botManager;
        }

        public string CallBotByName(CallBotDTO callBotDto)
        {
            var botInstance = _botManager.GetBotInstanceByName(callBotDto.BotName);
            if (botInstance == null) 
            {
                throw new ArgumentException("Бот с именем " + callBotDto.BotName + " не найден"); 
            }
            var botAnswer = botInstance.GetAnswerTheMessage(callBotDto.MessageText);
            return botAnswer;
        }
    }
}
