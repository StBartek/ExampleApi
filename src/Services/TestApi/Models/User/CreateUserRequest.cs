namespace TestApi.Models.User
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public string Password { get; set; }
    }
}