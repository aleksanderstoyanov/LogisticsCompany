namespace LogisticsCompany.Services.Contracts
{
    public interface IPackageStatusService
    {
        public Task<int?> GetIdByName(string name);
    }
}
