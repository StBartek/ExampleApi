namespace TestApi.Models.User
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int CityId { get; set; }
        public int StreetId { get; set; }
        public string Password { get; set; }
    }
}
