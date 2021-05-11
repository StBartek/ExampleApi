using Database;

namespace TestApi.Models.User
{
    public class UserGridViewModel
    {
        public UserGridViewModel(FullUserModel u, CityModel c, StreetModel s)
        {
            UserId = u.UserId;
            FirstName = u.FirstName;
            Surname = u.Surname;
            //Phone = u.Phone;
            Email = u.Email;
            Age = u.Age;
            CityId = u.CityId;
            CityName = c.Name;
            StreetId = u.StreetId;
            StreetName = s?.Name;
            HouseNo = u.HouseNo;
            FlatNo = u.FlatNo;
            PostCode = s != null ? s.PostCode : c.PostCode;
        }

        public UserGridViewModel(Users user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            Surname = user.Surname;
            Phone = user.Phone;
            Email = user.Email;
            Age = user.Age;
            //CityId = user.CityId;
            //CityName = user.City.Name;
            //StreetId = user.StreetId;
            //StreetName = user.Street?.Name;
            HouseNo = user.HouseNo;
            FlatNo = user.FlatNo;
            //PostCode = user.Street is Streets ? user.Street.PostCode : user.City.PostCode;
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? StreetId { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string FlatNo { get; set; }
        public string PostCode { get; set; }
        public string SearchData { get; set; }       
    }
}
