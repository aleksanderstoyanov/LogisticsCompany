namespace LogisticsCompany.Services.Roles.Queries
{
    public interface IRoleQueryService
    {
        public Task<int> GetIdByName(string name);
        public Task<string?> GetRoleNameById(int id);
    }
}
