using System.ComponentModel.DataAnnotations.Schema;

namespace AT.Data.Models
{
    public class User
    {
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string Password { get; set; }
    }
}
