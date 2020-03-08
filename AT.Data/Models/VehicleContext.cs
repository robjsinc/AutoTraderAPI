using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AT.Data.Models
{
    public class VehicleContext : DbContext
    {
        public VehicleContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<EngineInfo> EngineInfo { get; set; }
        public DbSet<StandardEquipment> StandardEquipment { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
                .HasOne(a => a.EngineInfo).WithOne()
                .HasForeignKey<EngineInfo>(e => e.VehicleID);

            modelBuilder.Entity<Vehicle>()
                .HasOne(a => a.StandardEquipment).WithOne()
                .HasForeignKey<StandardEquipment>(e => e.VehicleID);

            modelBuilder.Entity<Vehicle>()
                .HasOne(a => a.Dealer).WithOne()
                .HasForeignKey<Dealer>(e => e.DealerID);


            // var data = GetVehicles();

            //modelBuilder.Entity<Vehicle>().HasData(
            //    data.vehicles
            //);

            //modelBuilder.Entity<StandardEquipment>().HasData(
            //    data.standardEquipment
            //);

            //modelBuilder.Entity<EngineInfo>().HasData(
            //    data.engineInfo
            //);
        }

        public DataLists GetVehicles()
        {
            var file = File.ReadAllLines("..\\Files\\VehicleNotepad.txt").ToList();
            var vehicle = new Vehicle();
            var engineInfo = new EngineInfo();
            var standardEquipment = new StandardEquipment();
            var vehicleList = new List<Vehicle>();
            var engineinfoList = new List<EngineInfo>();
            var standardEquipmentList = new List<StandardEquipment>();
            var engineInfoList = new List<EngineInfo>();
            var equipmentList = new string[27];
            var data = new DataLists();
            var count = 0;
            var vehicleID = 1;
            var i = 0;

            for (i = 0; i < file.Count - 1; i++)
            {
                if (vehicleList.Count > count)
                {
                    vehicle = new Vehicle();
                    count++;
                }

                if (file[i] == "")
                {
                    i++;
                }

                if (file[i].StartsWith("Audi") || file[i].StartsWith("BMW") || file[i].StartsWith("Alfa Romeo") ||
                    file[i].StartsWith("Jaguar") || file[i].StartsWith("Ford") || file[i].StartsWith("Mercedes") || file[i].StartsWith("Volkswagen"))
                {
                    var splitLine = file[i].Split('(');
                    var splitLine2 = splitLine[0].Split(' ');
                    vehicle.Model = splitLine2[1];
                    vehicle.Make = splitLine[0].Split(" ")[0];
                    var splitElement = splitLine[1].Split(')');
                    vehicle.Version = splitElement[1];
                    vehicle.VehicleID = vehicleID;
                    vehicle.Year = splitLine[1].Substring(0, 4);
                }

                if (file[i].StartsWith("From"))
                {
                    vehicle.Price = Convert.ToDecimal(file[i].Split("£")[1]);
                }

                if (file[i].StartsWith("Engine\t"))
                {
                    var splitLine = file[i + 1].Split('\t').ToList();
                    splitLine.RemoveAll(x => x == "");

                    engineInfo.Engine = splitLine[0];
                    engineInfo.Power = splitLine[1];
                    engineInfo.FuelEconomy = splitLine[4];
                    vehicle.InsuranceGroup = splitLine[5];
                    vehicle.RoadTax = splitLine[6];
                    i += 2;
                }

                if (file[i].StartsWith("Performance"))
                {
                    i++;
                    i++;
                    engineInfo.TopSpeed = file[i].Split("\t")[1];
                    i++;
                    engineInfo.ZeroToSixty = file[i].Split("\t")[1];
                    i += 4;
                    engineInfo.MilesPerTank = file[i].Split("\t")[1];
                }

                if (file[i].StartsWith("Dimensions"))
                {
                    i++;
                    engineInfo.FuelCapacity = file[i].Split("\t")[1];
                    i += 7;
                }

                if (file[i].StartsWith("Engine"))
                {
                    i++;
                    engineInfo.EngineSizeCC = file[i].Split("\t")[1];
                    i++;
                    engineInfo.Cylinders = Convert.ToInt32(file[i].Split("\t")[1]);
                    i++;
                    engineInfo.Valves = Convert.ToInt32(file[i].Split("\t\t")[1]);
                    i++;
                    engineInfo.FuelType = file[i].Split("\t")[1];
                    i++;
                    engineInfo.Transmition = file[i].Split("\t")[1];
                    i++;
                    engineInfo.Gearbox = file[i].Split("\t")[1];
                    i++;
                    engineInfo.DriveTrain = file[i].Split("\t")[1];
                }

                if (file[i].StartsWith("BodyType"))
                {
                    i++;
                    vehicle.BodyType = file[i];
                }

                if (file[i].StartsWith("Standard Equipment"))
                {
                    i++;
                    var iterator = 0;
                    do
                    {
                        equipmentList[iterator] = file[i];
                        i++;
                        iterator++;
                    } while (!file[i].Equals(""));

                    standardEquipment = FillStandardEquipment(equipmentList, standardEquipment);
                }

                if (equipmentList[0] != null)
                {
                    vehicleList.Add(vehicle);
                    standardEquipment.VehicleID = vehicleID;
                    standardEquipment.StandardEquipmentID = vehicleID;
                    standardEquipmentList.Add(standardEquipment);
                    engineInfo.VehicleID = vehicleID;
                    engineInfo.EngineInfoID = vehicleID;
                    engineInfoList.Add(engineInfo);
                    equipmentList = new string[27];
                    standardEquipment = new StandardEquipment();
                    engineInfo = new EngineInfo();
                    vehicleID++;
                }
            }
            data.vehicles = vehicleList;
            data.engineInfo = engineInfoList;
            data.standardEquipment = standardEquipmentList;

            return data;
        }

        private static StandardEquipment FillStandardEquipment(string[] equipmentList, StandardEquipment standardEquipment)
        {
            standardEquipment.Equipment1 = equipmentList[0] != "" ? equipmentList[0] : string.Empty;
            standardEquipment.Equipment2 = equipmentList[1] != "" ? equipmentList[1] : string.Empty;
            standardEquipment.Equipment3 = equipmentList[2] != "" ? equipmentList[2] : string.Empty;
            standardEquipment.Equipment4 = equipmentList[3] != "" ? equipmentList[3] : string.Empty;
            standardEquipment.Equipment5 = equipmentList[4] != "" ? equipmentList[4] : string.Empty;
            standardEquipment.Equipment6 = equipmentList[5] != "" ? equipmentList[5] : string.Empty;
            standardEquipment.Equipment7 = equipmentList[6] != "" ? equipmentList[6] : string.Empty;
            standardEquipment.Equipment8 = equipmentList[7] != "" ? equipmentList[7] : string.Empty;
            standardEquipment.Equipment9 = equipmentList[8] != "" ? equipmentList[8] : string.Empty;
            standardEquipment.Equipment10 = equipmentList[9] != "" ? equipmentList[9] : string.Empty;
            standardEquipment.Equipment11 = equipmentList[10] != "" ? equipmentList[10] : string.Empty;
            standardEquipment.Equipment12 = equipmentList[11] != "" ? equipmentList[11] : string.Empty;
            standardEquipment.Equipment13 = equipmentList[12] != "" ? equipmentList[12] : string.Empty;
            standardEquipment.Equipment14 = equipmentList[13] != "" ? equipmentList[13] : string.Empty;
            standardEquipment.Equipment15 = equipmentList[14] != "" ? equipmentList[14] : string.Empty;
            standardEquipment.Equipment16 = equipmentList[15] != "" ? equipmentList[15] : string.Empty;
            standardEquipment.Equipment17 = equipmentList[16] != "" ? equipmentList[16] : string.Empty;
            standardEquipment.Equipment18 = equipmentList[17] != "" ? equipmentList[17] : string.Empty;
            standardEquipment.Equipment19 = equipmentList[18] != "" ? equipmentList[18] : string.Empty;
            standardEquipment.Equipment20 = equipmentList[19] != "" ? equipmentList[19] : string.Empty;
            standardEquipment.Equipment21 = equipmentList[20] != "" ? equipmentList[20] : string.Empty;
            standardEquipment.Equipment22 = equipmentList[21] != "" ? equipmentList[21] : string.Empty;
            standardEquipment.Equipment23 = equipmentList[22] != "" ? equipmentList[22] : string.Empty;
            standardEquipment.Equipment24 = equipmentList[23] != "" ? equipmentList[23] : string.Empty;
            standardEquipment.Equipment25 = equipmentList[24] != "" ? equipmentList[24] : string.Empty;
            standardEquipment.Equipment26 = equipmentList[25] != "" ? equipmentList[25] : string.Empty;

            return standardEquipment;
        }
    }

}
