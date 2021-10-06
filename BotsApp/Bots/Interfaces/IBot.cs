using System.Threading.Tasks;

namespace BotsApp.Bots.Interfaces
{
    public interface IBot
    {
        public string BotName { get; }

        public string GetAnswerTheMessage(string messageText);
    }
}
