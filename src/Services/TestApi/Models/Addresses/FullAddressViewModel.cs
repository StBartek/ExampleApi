using System;
using System.Collections.Generic;
using System.Linq;
using TestApi.Models.User;
using Db = Database;

namespace TestApi.Models.Addresses
{
    public class FullAddressViewModel
    {
        public FullAddressViewModel(Db.Addresses addresses)
        {
            AddressId = addresses.AddressId;
            CityId = addresses.CityId;
            CityName = addresses.City.Name;
            StreetId = addresses.StreetId;
            StreetName = addresses.Street?.Name;
            HouseNo = addresses.HouseNo;
            FlatNo = addresses.FlatNo;
            Users = addresses.UsersLAddresses.Select(x => new UserWithContactViewModel(x.User)).ToList();
        }

        public Guid AddressId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? StreetId { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string FlatNo { get; set; }
        public List<UserWithContactViewModel> Users { get; set; }
    }
}
