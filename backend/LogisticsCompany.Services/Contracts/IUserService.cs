using LogisticsCompany.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IUserService
    {
        Task Register(RegisterDto dto);
        Task<RegisterDto> GetUserByEmail(string email);
        Task Login();
    }
}
