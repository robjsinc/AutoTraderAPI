using System.Data;

namespace AT.Repo.Interfaces
{
    public interface IConnectionFactory
    {
        IDbConnection Connection();
    }
}
