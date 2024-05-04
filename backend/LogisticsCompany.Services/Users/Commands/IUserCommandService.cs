using LogisticsCompany.Services.Users.Dto;

namespace LogisticsCompany.Services.Users.Commands
{
    public interface IUserCommandService
    {
        Task Update(UserDto userDto);
        Task Delete(int id);
    }
}
