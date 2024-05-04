using LogisticsCompany.Services.Authorization.Dto;
using LogisticsCompany.Services.Users.Dto;

namespace LogisticsCompany.Services.Users.Queries
{
    public interface IUserQueryService
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task<IEnumerable<UserDto>> GetDifferentUsersFromCurrent(int id, string role);
        Task<LoginDto?> GetById(int id);
        Task<string> GetRegisterEmail(string email);
        Task<LoginDto?> GetUserByEmailAndPassword(string email, string password);
    }
}
