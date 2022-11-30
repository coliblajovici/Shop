using ApiGateway.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("products")]
        [AllowAnonymous]
        public ActionResult<AuthToken> GetProductsAuthentication([FromBody] AuthUser user)
        {
            return new ApiTokenService().GenerateToken(user);
        }
    }
}
