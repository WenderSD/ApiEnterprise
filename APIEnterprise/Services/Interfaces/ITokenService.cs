using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using APIEnterprise.DTOs.Authentication;

namespace APIEnterprise.Services.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
