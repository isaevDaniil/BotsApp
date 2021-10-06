using BotsApp.BLL.BusinessModels;
using BotsApp.BLL.DTO;
using BotsApp.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BotsApp.BLL.Services
{
    public class MessageQueueHostedService :  BackgroundService
    {
        private IServiceProvider _services;
        private SemaphoreSlim _semaphoreSlim;

        public IBackgroundMessageQueue MessagesQueue { get; }

        public MessageQueueHostedService(IBackgroundMessageQueue messagesQueue, IServiceProvider serviceProvider, int maxBotsThreadsCount)
        {
            MessagesQueue = messagesQueue;
            _services = serviceProvider;
            _semaphoreSlim = new SemaphoreSlim(maxBotsThreadsCount);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var messageData = await MessagesQueue.DequeueMessageDataAsync(stoppingToken);
                await _semaphoreSlim.WaitAsync();
                var t = Task.Run(() => HandleMessage(messageData, stoppingToken));
            }
        }

        private void HandleMessage(MessageData messageData, CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                var botServ = scope.ServiceProvider.GetRequiredService<IBotService>();
                var botAnswer = botServ.CallBotByName(new CallBotDTO { BotName = messageData.BotName, MessageText = messageData.MessageText });
                if (!String.IsNullOrEmpty(botAnswer))
                {
                    using (var httpClient = new HttpClient())
                    {
                        var httpContent = new StringContent(JsonConvert.SerializeObject(new ResponseToClient { BotName = messageData.BotName, BotAnswer = botAnswer, ChatId = messageData.ChatId }));
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        try
                        {
                            httpClient.PostAsync(messageData.BackUrl, httpContent).Wait();
                        }
                        catch (Exception)
                        {
                        }
                    }    
                }
            }
            _semaphoreSlim.Release();
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
