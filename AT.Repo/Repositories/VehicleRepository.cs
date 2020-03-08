using AT.Data.Models;
using AT.Repo.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AT.Repo.Repositories
{
    public class VehicleRepository : IVehicleRepository<Vehicle>
    {
        public VehicleRepository(VehicleContext context, IConnectionFactory conn)
        {
            _vehicleContext = context;
            _conn = conn;
        }

        readonly VehicleContext _vehicleContext;
        readonly IConnectionFactory _conn;

        public IEnumerable<Vehicle> GetAll()
        {
            var engineInfo = _vehicleContext.EngineInfo.ToList();
            var dealer = _vehicleContext.Dealers.ToList();
            var vehicles = _vehicleContext.Vehicles.ToList();

            return vehicles;
        }
        public Vehicle Get(int id)
        {
            var vehicle = _vehicleContext.Vehicles.Find(id);
            vehicle.EngineInfo = _vehicleContext.EngineInfo.Find(id);
            vehicle.StandardEquipment = _vehicleContext.StandardEquipment.Find(id);
            vehicle.Dealer = _vehicleContext.Dealers.Find(vehicle.DealerID);

            vehicle.Images = new List<Image>();

            var query = (from c in _vehicleContext.Images
                                where c.VehicleId == id
                                select new { 
                                    c.ImageId,
                                    c.ImageName,
                                    c.ImagePath,
                                    c.VehicleId 
                                }).ToList();

            query.ForEach(image => vehicle.Images.Add(new Image() {
                ImageId = image.ImageId,
                ImageName = image.ImageName,
                ImagePath = image.ImagePath,
                VehicleId = image.VehicleId
            }));

            return vehicle;
        }

        public IEnumerable<Vehicle> GetVehiclesBySelectedInfo(Vehicle vehicle)
        {
            var vehicles = new List<Vehicle>() { vehicle };
            var sql = @"select * 
                          from vehicles v
                    inner join standardequipment se on se.vehicleid = v.vehicleid
                    inner join engineinfo e on e.vehicleid = v.vehicleid 
                    inner join dealers d on d.dealerid = v.dealerid
                         where ";

            if (vehicle.Make != "Any")
            {
                sql = $"{ sql } v.Make = '{ vehicle.Make }' and ";
            }
            if (vehicle.Model != "Any")
            {
                sql = $"{ sql } v.Model = '{ vehicle.Model }' and ";
            }
            if (vehicle.BodyType != "Any")
            {
                sql = $"{ sql } v.BodyType = '{ vehicle.BodyType }' and ";
            }

            sql = $"{ sql } v.Price > { vehicle.MinPrice } and v.Price < { vehicle.MaxPrice }";

            using (IDbConnection conn = _conn.Connection())
            {
                conn.Open();
                vehicles = conn.Query<Vehicle, StandardEquipment, EngineInfo, Dealer, Vehicle> (
                    sql,
                    (vehicle, standardEquipment, engineInfo, dealer) =>
                         {
                             vehicle.StandardEquipment = standardEquipment;
                             vehicle.Dealer = dealer;
                             vehicle.EngineInfo = engineInfo;
                             return vehicle;
                         },
                    splitOn: "VehicleID, DealerID").Distinct().OrderBy(x => x.Price).ToList();
            }
            return vehicles;
        }

        public IEnumerable<string> GetMakes()
        {
            var vehicleMakes = new List<string>();
            var sql = @"select distinct v.make from vehicles v;";

            using (IDbConnection conn = _conn.Connection())
            {
                conn.Open();
                vehicleMakes = conn.Query<string>(sql).OrderBy(x => x).ToList();
            }
            return vehicleMakes;
        }

        public IEnumerable<string> GetModelsByMake(string make)
        {
            var vehicleModels = new List<string>();

            var sql = $"select distinct v.model from vehicles v where v.make = '{make}';";

            using (IDbConnection conn = _conn.Connection())
            {
                conn.Open();
                vehicleModels = conn.Query<string>(sql).OrderBy(x => x).ToList();
            }

            return vehicleModels;
        }

        public IEnumerable<string> GetBodyTypes()
        {
            var bodytypes = new List<string>();

            var sql = $"select distinct v.bodytype from vehicles v";

            using (IDbConnection conn = _conn.Connection())
            {
                conn.Open();
                bodytypes = conn.Query<string>(sql).OrderBy(x => x).ToList();
            }
            return bodytypes;
        }

        public IEnumerable<string> GetInsuranceGroups()
        {
            return new List<string>() { "1-10", "11-20", "21-30", "31-40", "41-50" };
        }

        public void Add(Vehicle vehicle)
        {
            _vehicleContext.Vehicles.Add(vehicle);
            _vehicleContext.SaveChanges();
        }
        public void Update(Vehicle vehicle, Vehicle entity)
        {
            vehicle = entity;
            _vehicleContext.Vehicles.Update(vehicle);
            _vehicleContext.SaveChanges();
        }
        public void Delete(Vehicle vehicle)
        {
            _vehicleContext.Remove(vehicle);
            _vehicleContext.SaveChanges();
        }
    }
}
