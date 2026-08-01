using APIEnterprise.DTOs.Authentication;
using APIEnterprise.Models;

namespace APIEnterprise.Services.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponse> LoginAsync(LoginDTO model);
    }
}
