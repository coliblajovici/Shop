using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CatalogService.GraphQL.IntegrationTests
{
    public class GraphQLApplicationFactory : WebApplicationFactory<DotnetGraphQL.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // builder.ConfigureServices(services =>
                //    services.AddScoped(sp =>)
            });
            base.ConfigureWebHost(builder);
        }
    }
}
