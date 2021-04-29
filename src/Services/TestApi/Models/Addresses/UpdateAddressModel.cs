using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Addresses
{
    public class UpdateAddressModel
    {
        public Guid AddressId { get; set; }
        public string FlatNo { get; set; }
    }
}
