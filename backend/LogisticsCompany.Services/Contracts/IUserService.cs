using LogisticsCompany.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IUserService
    {
        Task Register(RegisterDto dto);
        Task Login();
    }
}
