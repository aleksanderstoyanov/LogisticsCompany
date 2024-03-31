using LogisticsCompany.Dto;
using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IUserService
    {
        Task Register(RegisterDto dto);
        Task<string> Login(LoginDto dto, string issuer, string key);
        Task<string> GetRegisterEmail(string email);
        Task<LoginDto?> GetUserByEmailAndPassword(string email, string password);
        Task Login();
    }
}
