using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CartingService.Api.Setup
{
    internal static class SwaggerSetup
    {
        internal static IServiceCollection ConfigurSwagger(this IServiceCollection services)
        {

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Carting Service API v1.0",
                    Description = "An ASP.NET Core Web API for Cart Items",                    
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Carting Service API v2.0",
                    Description = "An ASP.NET Core Web API for Cart Items",
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }
}
