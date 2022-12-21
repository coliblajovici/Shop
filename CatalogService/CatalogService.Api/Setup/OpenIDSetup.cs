using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

namespace CatalogService.Api.Setup
{
    internal static class OpenIDSetup
    {
        internal static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            // Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
            services.AddMicrosoftIdentityWebApiAuthentication(configuration);

            IdentityModelEventSource.ShowPII = true;

            return services;
        }
    }
}
