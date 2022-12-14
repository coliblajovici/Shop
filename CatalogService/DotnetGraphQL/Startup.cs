using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.SystemTextJson;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.DataLoader;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CatalogService.GraphQLSchema.Schema;

namespace DotnetGraphQL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {            
            //services.AddInfrastructureServices(Configuration);
            //services.AddApplicationServices();

            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);

            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IProductService, ProductService>();

            //services.AddSingleton<CategoryType>();

            services.AddGraphQL(b => b
                             .AddHttpMiddleware<ISchema>()
                             .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })                             
                             .AddWebSocketsHttpMiddleware<ProductsSchema>()                             
                             .AddSystemTextJson()
                             .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)                             
                             .AddSchema<ProductsSchema>()                            
                            .AddGraphTypes(typeof(ProductsSchema).Assembly));          

            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseWebSockets();           
            app.UseGraphQLWebSockets<ProductsSchema>();          
            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground();
        }
    }
}
