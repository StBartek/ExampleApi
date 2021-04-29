using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Addresses
{
    public class CreateAddressModel
    {
        public int CityId { get; set; }
        public int? StreetId { get; set; }
        public string HouseNo { get; set; }
        public string FlatNo { get; set; }
    }
}
