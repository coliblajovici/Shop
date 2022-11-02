
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CatalogService.Api.Setup
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
                    Title = "Catalog Service API",
                    Description = "An ASP.NET Core Web API for Catalogs",                    
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }
}
