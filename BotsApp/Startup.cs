using BotsApp.BLL.Interfaces;
using BotsApp.BLL.Services;
using BotsApp.Bots;
using BotsApp.Bots.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BotsApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IBotService, BotService>();
            services.AddSingleton<IBotManager, BotManager>();
            services.AddSingleton<IBackgroundMessageQueue>(ctx =>
            {
                if (!int.TryParse(Configuration["QueueCapacity"], out var queueCapacity))
                    queueCapacity = 100;
                return new BackgroundMessageQueue(queueCapacity);
            });
            services.AddHostedService<MessageQueueHostedService>(servProvider =>
            {
                if (!int.TryParse(Configuration["MaxBotsThreadsCount"], out var maxBotsThreadsCount))
                    maxBotsThreadsCount = 5;
                return new MessageQueueHostedService(servProvider.GetRequiredService<IBackgroundMessageQueue>(), servProvider, maxBotsThreadsCount);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
