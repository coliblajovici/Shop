using System.ComponentModel.DataAnnotations;

namespace AuthApi.Auth
{
    public class AuthUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
