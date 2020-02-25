using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AT.Data.Models
{
    public class StandardEquipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int StandardEquipmentID { get; set; }
        public int VehicleID { get; set; }
        public string Equipment1 { get; set; }
        public string Equipment2 { get; set; }
        public string Equipment3 { get; set; }
        public string Equipment4 { get; set; }
        public string Equipment5 { get; set; }
        public string Equipment6 { get; set; }
        public string Equipment7 { get; set; }
        public string Equipment8 { get; set; }
        public string Equipment9 { get; set; }
        public string Equipment10 { get; set; }
        public string Equipment11 { get; set; }
        public string Equipment12 { get; set; }
        public string Equipment13 { get; set; }
        public string Equipment14 { get; set; }
        public string Equipment15 { get; set; }
        public string Equipment16 { get; set; }
        public string Equipment17 { get; set; }
        public string Equipment18 { get; set; }
        public string Equipment19 { get; set; }
        public string Equipment20 { get; set; }
        public string Equipment21 { get; set; }
        public string Equipment22 { get; set; }
        public string Equipment23 { get; set; }
        public string Equipment24 { get; set; }
        public string Equipment25 { get; set; }
        public string Equipment26 { get; set; }
    }
}
