﻿using LogisticsCompany.Dto;
using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Services.Contracts
{
    public interface IUserService
    {
        Task Update(UserDto userDto);
        Task Delete(int id);
        Task<IEnumerable<UserDto>> GetUsers();
        Task<LoginDto?> GetById(int id);
        Task<string> GetRegisterEmail(string email);
        Task<LoginDto?> GetUserByEmailAndPassword(string email, string password);
        Task Register(RegisterDto dto);
        Task<string> Login(LoginDto dto, string issuer, string key);
    }
}
