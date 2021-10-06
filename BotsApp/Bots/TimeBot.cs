using BotsApp.Bots.Interfaces;
using System;

namespace BotsApp.Bots
{
    public class TimeBot : IBot
    {
        private string _patternString = " через ";

        public string BotName { get; } = "TimeBot";

        public string GetAnswerTheMessage(string messageText)
        {
            if (messageText.Contains("/current"))
            {
                var responseStr = "Сейчас " + DateTime.Now.ToString("HH:mm");
                return responseStr;
            }
            int index = messageText.IndexOf(_patternString);
            if (index != -1)
            {
                var tempstr = messageText.Substring(index + _patternString.Length);
                var lastindex = tempstr.IndexOf(" ");
                var enteredMinuteCountString = tempstr.Substring(0, lastindex != -1 ? lastindex : tempstr.Length);
                if (!int.TryParse(enteredMinuteCountString, out int minuteCount))
                {
                    return String.Empty;
                }
                var responseStr = "Через " + enteredMinuteCountString + " минут будет " + DateTime.Now.AddMinutes(minuteCount).ToString("HH:mm");
                return responseStr;
            }
            return String.Empty;
        }
    }
}
