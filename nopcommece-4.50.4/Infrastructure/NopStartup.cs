

using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Customers.NopChat.Factories;
using Nop.Plugin.Customers.NopChat.Hubs;
using Nop.Plugin.Customers.NopChat.Services;



namespace Nop.Plugin.Customers.NopChat.Infrastructure
{

    public class NopStartup : INopStartup
    {
        public int Order => 100;

        public void Configure(IApplicationBuilder application)
        {
            application.UseAuthorization();
           application.UseRouting(); 
           application.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/notify");
            });
        }

        
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddSignalR();
            

            services.AddScoped<INopChatMessageService, NopChatMessageService>();
            services.AddScoped<INopChatMessageModelFactory, NopChatMessageModelFactory>();
        }
    }
}