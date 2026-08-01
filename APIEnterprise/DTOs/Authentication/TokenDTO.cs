using System.ComponentModel.DataAnnotations;

namespace APIEnterprise.DTOs.Authentication
{
    public class TokenDTO
    {
        [Required]
        public string? AcessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
