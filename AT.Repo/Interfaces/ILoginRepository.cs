using AT.Data.Models;

namespace AT.Repo.Interfaces
{
    public interface ILoginRepository<T>
    {
        public User Get(User userinfo);
    }
}