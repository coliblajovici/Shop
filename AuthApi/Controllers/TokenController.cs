using AuthApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
