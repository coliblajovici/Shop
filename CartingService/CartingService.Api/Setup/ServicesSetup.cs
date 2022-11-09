using CartingService.Application;
using CartingService.Application.Interfaces;
using CartingService.Infrastructure.Persistance;
using LiteDB;

namespace CartingService.Api.Setup
{
    internal static class ServicesSetup
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICartService, CartService>()
                    .AddScoped<ICartRepository, CartRepository>();

            services.AddSingleton<LiteDatabase, LiteDatabase>(db => new LiteDatabase(configuration.GetConnectionString("DefaultConnection")));
            
            return services;
        }
    }
}
