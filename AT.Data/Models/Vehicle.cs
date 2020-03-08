using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AT.Data.Models
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public string Year { get; set; }
        public decimal Price { get; set; }
        public string RoadTax { get; set; }
        public string InsuranceGroup { get; set; }
        public string BodyType { get; set; }
        public EngineInfo EngineInfo { get; set; }
        public StandardEquipment StandardEquipment { get; set; }
        public int DealerID { get; set; }
        public Dealer Dealer { get; set; }

        public List<Image> Images { get; set; }

        [NotMapped]
        public string DistanceFromCustomerPostCode { get; set; }
        [NotMapped]
        public string TimeFromCustomerPostCode { get; set; }
        [NotMapped]
        public string Longitute { get; set; }
        [NotMapped]
        public string Latitude { get; set; }
        [NotMapped]
        public List<string> StandardEquipmentList { get; set; }
        [NotMapped]
        public string MinPrice { get; set; }
        [NotMapped]
        public string MaxPrice { get; set; }
        [NotMapped]
        public string Distance { get; set; }
    }
}
