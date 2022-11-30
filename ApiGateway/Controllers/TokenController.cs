using ApiGateway.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ApiGateway.Auth.ApiTokenService;

namespace ApiGateway.Controllers
{
    [Route("api/token")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<AuthToken> GetToken([FromBody] AuthUser user)
        {
            return new ApiTokenService().GenerateToken(user);
        }
    }
}
