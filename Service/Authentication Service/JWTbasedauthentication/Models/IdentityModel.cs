using System.ComponentModel.DataAnnotations;

namespace JWTbasedauthentication.Models
{
    public class IdentityModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
