using Auttar.Infrastructure.IoC;
using Microsoft.AspNetCore.Builder;
using System;

namespace Auttar.Web.Mvc
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940        
        }
        
        private static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterServices(services);            
        }       
    }
}
