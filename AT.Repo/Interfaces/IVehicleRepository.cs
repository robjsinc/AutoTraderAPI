using AT.Data.Models;
using System.Collections.Generic;

namespace AT.Repo.Interfaces
{
    public interface IVehicleRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        IEnumerable<Vehicle> GetVehiclesBySelectedInfo(Vehicle vehicle);
        IEnumerable<string> GetMakes();
        IEnumerable<string> GetModelsByMake(string make);
        IEnumerable<string> GetBodyTypes();
        IEnumerable<string> GetInsuranceGroups();
        void Add(TEntity entity);
        void Update(TEntity vehicle, TEntity entity);
        void Delete(TEntity entity);
    }
}
