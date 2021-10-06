using BotsApp.BLL.DTO;

namespace BotsApp.BLL.Interfaces
{
    public interface IBotService
    {
        public string CallBotByName(CallBotDTO callBotDto);
    }
}
