namespace TestApi.Models.User
{
    public class FullUserModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int CityId { get; set; }
        public int? StreetId { get; set; }
        public string HouseNo { get; set; }
        public string FlatNo { get; set; }
    }
}
