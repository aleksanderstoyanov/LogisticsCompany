namespace LogisticsCompany.Services.PackageStatuses.Queries
{
    public interface IPackageStatusQueryService
    {
        public Task<int?> GetIdByName(string name);
    }
}
