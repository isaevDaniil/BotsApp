using BotsApp.BLL.BusinessModels;
using BotsApp.BLL.DTO;
using BotsApp.BLL.Interfaces;
using BotsApp.Bots.Interfaces;
using BotsApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BotsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private IBotManager _botManager;

        public BotController(IBotManager botManager)
        {
            _botManager = botManager;
        }

        [HttpPost("NotifyBot")]
        public IActionResult NotifyBot([FromBody] NotifyBotModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "Некорректные данные" });
            }
            try
            {
                _botManager.EnqueueMessageDataAsync(new MessageData { BotName = model.BotName, MessageText = model.MessageText, BackUrl = model.BackUrl, ChatId = model.ChatId });
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }            
        }
    }
}
