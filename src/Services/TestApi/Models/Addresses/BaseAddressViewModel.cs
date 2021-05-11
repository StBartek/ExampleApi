using System;
using Db = Database;

namespace TestApi.Models.Addresses
{
    public class BaseAddressViewModel
    {
        public BaseAddressViewModel(Db.Addresses address)
        {
            AddressId = address.AddressId;
            CityId = address.CityId;
            CityName = address.City.Name;
            StreetId = address.StreetId;
            StreetName = address.Street?.Name;
            HouseNo = address.HouseNo;
            FlatNo = address.FlatNo;
        }

        public Guid AddressId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? StreetId { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string FlatNo { get; set; }
    }
}
