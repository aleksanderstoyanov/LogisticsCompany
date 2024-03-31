namespace LogisticsCompany.Services.Contracts
{
    public interface IRoleService
    {
        public Task<int> GetIdByName(string name);
        public Task<string?> GetRoleNameById(int id);
        Task Create(string roleName);
    }
}
