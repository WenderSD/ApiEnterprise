using APIEnterprise.DTOs.Authentication;
using APIEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIEnterprise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ILogger _logger;
        public AuthController(ILoginService loginService, ILogger logger)
        {
            _loginService = loginService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var loginResponse = await _loginService.LoginAsync(model);
                return Ok(loginResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError("{Error}",ex);
                return Unauthorized(ex);
            }
        }
    }
}
