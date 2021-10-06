using BotsApp.Bots.Interfaces;
using System;
using System.Collections.Generic;

namespace BotsApp.Bots
{
    public class AnecdoteBot : IBot
    {
        private List<string> _keywords;

        public string BotName { get; } = "AnecdoteBot";

        public AnecdoteBot()
        {
            _keywords = new List<string> { "скучно", "грустно" };
        }

        public string GetAnswerTheMessage(string messageText)
        {
            foreach (var keyword in _keywords)
            {
                if (messageText.Contains(keyword))
                {
                    return "Рассказал анектдот";
                }
            }
            return String.Empty;
        }
    }
}
