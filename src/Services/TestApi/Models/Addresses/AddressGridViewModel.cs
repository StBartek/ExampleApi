using System;
using System.Linq;
using Db = Database;

namespace TestApi.Models.Addresses
{
    public class AddressGridViewModel
    {
        public AddressGridViewModel(Db.Addresses addresses)
        {
            AddressId = addresses.AddressId;
            CityId = addresses.CityId;
            CityName = addresses.City.Name;
            StreetId = addresses.StreetId;
            StreetName = addresses.Street?.Name;
            HomeNo = addresses.HouseNo;
            FlatNo = addresses.FlatNo;
            InhabitantsNumber = addresses.UsersLAddresses.Count();
        }

        public Guid AddressId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? StreetId { get; set; }
        public string StreetName { get; set; }
        public string HomeNo { get; set; }
        public string FlatNo { get; set; }
        public int InhabitantsNumber { get; set; }
    }
}
