namespace LogisticsCompany.Services.Contracts
{
    public interface IRoleService
    {
        public Task<int> GetIdByName(string name);
        Task Create(string roleName);
    }
}
