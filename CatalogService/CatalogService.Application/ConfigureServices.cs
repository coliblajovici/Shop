using CatalogService.Application;
using CatalogService.Application.Common.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IProductService, ProductService>();
    }
}
