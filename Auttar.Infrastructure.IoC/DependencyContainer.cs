using Microsoft.Extensions.DependencyInjection;

using Auttar.Application.Interfaces;
using Auttar.Application.Services;

namespace Auttar.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Application 
            services.AddScoped<IPinPadServices, PinPadServices>();
        }
    }
}
