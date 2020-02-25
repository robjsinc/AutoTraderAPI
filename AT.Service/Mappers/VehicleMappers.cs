using AT.Data.Models;
using System.Collections.Generic;
namespace AT.Service.Mappers
{
    public static class VehicleMappers
    {
        public static Vehicle ToStandardEquipmentList(this Vehicle vehicle)
        {
            vehicle.StandardEquipmentList = new List<string>();

            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment1);
            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment2);
            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment3);
            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment4);
            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment5);
            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment6);
            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment7);
            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment8);
            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment9);
            vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment10);
            if (vehicle.StandardEquipment.Equipment11 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment11);
            }
            if (vehicle.StandardEquipment.Equipment12 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment12);
            }
            if (vehicle.StandardEquipment.Equipment13 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment13);
            }
            if (vehicle.StandardEquipment.Equipment13 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment14);
            }
            if (vehicle.StandardEquipment.Equipment14 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment15);
            }
            if (vehicle.StandardEquipment.Equipment15 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment16);
            }
            if (vehicle.StandardEquipment.Equipment16 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment17);
            }
            if (vehicle.StandardEquipment.Equipment17 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment18);
            }
            if (vehicle.StandardEquipment.Equipment18 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment19);
            }
            if (vehicle.StandardEquipment.Equipment19 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment20);
            }
            if (vehicle.StandardEquipment.Equipment20 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment21);
            }
            if (vehicle.StandardEquipment.Equipment21 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment22);
            }
            if (vehicle.StandardEquipment.Equipment22 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment22);
            }
            if (vehicle.StandardEquipment.Equipment23 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment23);
            }
            if (vehicle.StandardEquipment.Equipment24 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment24);
            }
            if (vehicle.StandardEquipment.Equipment25 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment25);
            }
            if (vehicle.StandardEquipment.Equipment26 != null)
            {
                vehicle.StandardEquipmentList.Add(vehicle.StandardEquipment.Equipment26);
            }
            vehicle.StandardEquipment = null;

            return vehicle;
        }
    }
}
