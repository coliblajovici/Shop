using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Auth
{
    public class AuthUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
