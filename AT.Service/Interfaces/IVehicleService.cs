using AT.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AT.Service.Interfaces
{
    public interface IVehicleService<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        Task<IEnumerable<Vehicle>> GetVehiclesBySelectedInfo(Vehicle vehicle, string postcode);
        Task<List<Vehicle>> GetPostCodeData(List<Vehicle> vehicles, string postcode);
        Task<string> GetLongitudeAndLatitude(string postcode);
        IEnumerable<string> GetMakes();
        IEnumerable<string> GetModelsByMake(string make);
        IEnumerable<string> GetBodyTypes();
        IEnumerable<string> GetInsuranceGroups();

        void Add(TEntity entity);
        void Update(TEntity vehicle, TEntity entity);
        void Delete(TEntity entity);
    }
}
