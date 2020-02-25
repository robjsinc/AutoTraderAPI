using AT.Repo.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AT.Repo.ConnectionFactory
{
    public class ConnectionFactory : IConnectionFactory
    {
        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        readonly IConfiguration _configuration;

        public IDbConnection Connection()
        {
            return new SqlConnection(_configuration.GetConnectionString("VehicleDB"));
        }
    }
}
