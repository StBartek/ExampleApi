namespace TestApi.Models.User
{
    public class UserGridParams
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string SearchData { get; set; }
        public int? Age { get; set; }
        public int CurrentPage { get; set; }
    }
}
