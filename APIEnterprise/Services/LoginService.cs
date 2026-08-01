using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using APIEnterprise.DTOs.Authentication;
using APIEnterprise.Models;
using APIEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace APIEnterprise.Services
{
    public class LoginService : ILoginService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public LoginService(ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);
            
            await ValidateUserAsync(user!, model);
            
            var authClaims = await GetUserClaimsAsync(user!);
            var token = _tokenService.GenerateAccessToken(authClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityMinutes"], out var refreshTokenValidityMinutes);

            user!.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityMinutes);

            await _userManager.UpdateAsync(user);

            return GetLoginResponse(token, refreshToken);
        }

        private async Task ValidateUserAsync(ApplicationUser user, LoginDTO model)
        {
            if (user == null)
                throw new UnauthorizedAccessException("User unauthorized.");
            
            var passwordIsValid = await _userManager.CheckPasswordAsync(user, model.Password!);

            if (!passwordIsValid)
                throw new UnauthorizedAccessException("User unauthorized.");
        }
        
        private async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            var userClaims = new List<Claim> {
                new (ClaimTypes.Name, user.UserName!),
                new (ClaimTypes.Email, user.Email!),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            
            return userClaims;
        }

        private LoginResponse GetLoginResponse(JwtSecurityToken token, string refreshToken)
        {
            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };
        }
    }
}
