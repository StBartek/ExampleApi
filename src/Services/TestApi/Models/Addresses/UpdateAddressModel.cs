using System;

namespace TestApi.Models.Addresses
{
    public class UpdateAddressModel
    {
        public Guid AddressId { get; set; }
        public string HouseNo { get; set; }
        public string FlatNo { get; set; }
    }
}
