using LogisticsCompany.Services.Authorization.Dto;

namespace LogisticsCompany.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task Register(RegisterDto dto);
        Task<string> Login(LoginDto dto, string issuer, string key);
    }
}
