namespace LogisticsCompany.Data.Contracts
{
    public interface IDbAdapter
    {
        Task<T> QuerySingle<T>(string query);
        Task<IEnumerable<T>> QueryAll<T>(string query);
        Task ExecuteCommand(string command, object? param = null);
    }
}
