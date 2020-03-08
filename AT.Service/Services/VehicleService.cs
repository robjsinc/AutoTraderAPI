using AT.Data.Models;
using AT.Repo.Interfaces;
using AT.Service.Interfaces;
using AT.Service.Mappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AT.Service.Services
{
    public class VehicleService : IVehicleService<Vehicle>
    {
        private readonly IVehicleRepository<Vehicle> _vehicleRepository;
        private readonly IHttpClientFactory _clientFactory;

        public VehicleService(IVehicleRepository<Vehicle> vehicleRepository, IHttpClientFactory clientFactory)
        {
            _vehicleRepository = vehicleRepository;
            _clientFactory = clientFactory;
        }

        public IEnumerable<Vehicle> GetAll()
        {
            return _vehicleRepository.GetAll();
        }
        public Vehicle Get(int id)
        {
            return _vehicleRepository.Get(id).ToStandardEquipmentList();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesBySelectedInfo(Vehicle vehicle, string postcode)
        {
            var vehicles = _vehicleRepository.GetVehiclesBySelectedInfo(vehicle).ToList();

            if (!postcode.Equals("null"))
            {
                vehicles = await GetPostCodeData(vehicles, postcode);
            }
            if (!vehicle.InsuranceGroup.Equals("Any"))
            {
                vehicles = SortVehiclesByInsuranceGroup(vehicle, vehicles);
            }
            if (vehicle.Distance != null)
            {
                vehicles = vehicles.Where(x => Convert.ToDecimal(x.DistanceFromCustomerPostCode.Substring(0, vehicles[0].DistanceFromCustomerPostCode.Length - 3))
                                             < Convert.ToDecimal(vehicle.Distance)).ToList();
            }

            return vehicles;
        }

        public async Task<List<Vehicle>> GetPostCodeData(List<Vehicle> vehicles, string postcode)
        {
            var postCodeApiResponse = await GetLongitudeAndLatitude(postcode);
            JObject jObjectPostCode = JObject.Parse(postCodeApiResponse);
            JToken jPostCode = jObjectPostCode["result"];
            var latitude = (string)jPostCode["latitude"];
            var longitude = (string)jPostCode["longitude"];

            foreach (var vehicle in vehicles)
            {
                var dealerPostCodeApiResponse = await GetLongitudeAndLatitude(vehicle.Dealer.PostCode);
                if (dealerPostCodeApiResponse != null)
                {
                    ParseJsonValuesToVehicle(dealerPostCodeApiResponse, vehicle);

                    var request = new HttpRequestMessage(HttpMethod.Get,
                                      $"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={latitude},{longitude}&destinations={vehicle.Latitude},{vehicle.Longitute}&key=AIzaSyCpws1VlWBD0lAJMWUq02NYYn4XQnANV-M");

                    var client = _clientFactory.CreateClient();

                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseStreamDistance = await response.Content.ReadAsStringAsync();
                        var googleApiResponse = JsonConvert.DeserializeObject<Rootobject>(responseStreamDistance);
                        vehicle.DistanceFromCustomerPostCode = googleApiResponse.rows[0].elements[0].distance.text;
                        vehicle.TimeFromCustomerPostCode = googleApiResponse.rows[0].elements[0].duration.text;
                    }
                }
            }
            return vehicles;
        }

        public void ParseJsonValuesToVehicle(string dealerPostCodeApiResponse, Vehicle vehicle)
        {
            JObject jObjectVehicle = JObject.Parse(dealerPostCodeApiResponse);
            JToken jVehicle = jObjectVehicle["result"];
            vehicle.Latitude = (string)jVehicle["latitude"];
            vehicle.Longitute = (string)jVehicle["longitude"];
        }

        public async Task<string> GetLongitudeAndLatitude(string postcode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.postcodes.io/postcodes/{postcode}");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            else return null;
        }

        private static List<Vehicle> SortVehiclesByInsuranceGroup(Vehicle vehicle, List<Vehicle> vehicles)
        {
            var userSearchValueLowInsuranceGroup = Convert.ToInt32(vehicle.InsuranceGroup.Split('-')[0]);
            var userSearchValueHighInsuranceGroup = Convert.ToInt32(vehicle.InsuranceGroup.Split('-')[1]);

            var vehicleListSingleInsuranceGroup = vehicles.Where(x => x.InsuranceGroup.Length <= 2).ToList();
            var vehicleListMultipleInsuranceGroup = vehicles.Where(x => x.InsuranceGroup.Contains("-")).ToList();

            return vehicleListSingleInsuranceGroup.Where(
                   x => Convert.ToInt32(x.InsuranceGroup) >= userSearchValueLowInsuranceGroup &&
                        Convert.ToInt32(x.InsuranceGroup) <= userSearchValueHighInsuranceGroup).Concat(
                   vehicleListMultipleInsuranceGroup.Where(
                   x => Convert.ToInt32(x.InsuranceGroup.Split('-')[0]) >= userSearchValueLowInsuranceGroup &&
                        Convert.ToInt32(x.InsuranceGroup.Split('-')[0]) <= userSearchValueHighInsuranceGroup
                        ||
                        Convert.ToInt32(x.InsuranceGroup.Split('-')[1]) >= userSearchValueLowInsuranceGroup &&
                        Convert.ToInt32(x.InsuranceGroup.Split('-')[1]) <= userSearchValueHighInsuranceGroup)).ToList();
        }

        public IEnumerable<string> GetMakes()
        {
            return _vehicleRepository.GetMakes();
        }

        public IEnumerable<string> GetModelsByMake(string make)
        {
            return _vehicleRepository.GetModelsByMake(make);
        }

        public IEnumerable<string> GetBodyTypes()
        {
            return _vehicleRepository.GetBodyTypes();
        }

        public IEnumerable<string> GetInsuranceGroups()
        {
            return _vehicleRepository.GetInsuranceGroups();
        }

        public void Add(Vehicle vehicle)
        {
            _vehicleRepository.Add(vehicle);
        }

        public void Update(Vehicle vehicle, Vehicle entity)
        {
            _vehicleRepository.Update(vehicle, entity);
        }

        public void Delete(Vehicle vehicle)
        {
            _vehicleRepository.Delete(vehicle);
        }

    }
}
