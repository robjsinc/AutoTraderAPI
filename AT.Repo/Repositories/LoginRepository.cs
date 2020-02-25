using AT.Data.Models;
using AT.Repo.Interfaces;
using Dapper;
using System.Data;

namespace AT.Repo.Repositories
{
    public class LoginRepository : ILoginRepository<User>
    {
        public LoginRepository(IConnectionFactory conn)
        {
            _conn = conn;
        }
        
        readonly IConnectionFactory _conn;

        public User Get(User userinfo)
        {
            User user = null;
            var sql = $"select * from users u where u.username = \'{userinfo.UserName}\' and u.password = \'{userinfo.Password}\';";

            using (IDbConnection conn = _conn.Connection())
            {
                conn.Open();
                user = conn.QueryFirstOrDefault<User>(sql);
            }

            return user;
        }
    }
}
