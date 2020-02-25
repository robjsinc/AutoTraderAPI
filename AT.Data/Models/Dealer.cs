using System;
using System.Collections.Generic;
using System.Text;

namespace AT.Data.Models
{
    public class Dealer
    {
        public int DealerID { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }
}
