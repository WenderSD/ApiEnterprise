using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using APIEnterprise.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace APIEnterprise.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        #region Public Methods
        
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = _configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid secret key");
            
            var privateKey = Encoding.UTF8.GetBytes(key);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = GetSecurityTokenDescriptor(claims, signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(secureRandomBytes);
            var refreshToken = Convert.ToBase64String(secureRandomBytes);

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var secretKey = _configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid secret key");
            var tokenValidationParameters = GetTokenValidationParameters(secretKey);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        
        #endregion
        
        #region Private Methods

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(IEnumerable<Claim> claims, SigningCredentials signingCredentials)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),
                Audience = _configuration["JWT:ValidAudience"],
                Issuer = _configuration["JWT:ValidIssuer"],
                SigningCredentials = signingCredentials
            };

            return tokenDescriptor;
        }
        
        private TokenValidationParameters GetTokenValidationParameters(string secretKey)
        {
            return new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false,
            };
        }
        
        #endregion
    }
}
