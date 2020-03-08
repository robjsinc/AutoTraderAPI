using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AT.Data.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public int VehicleId { get; set; }
    }
}
