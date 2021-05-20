
namespace TestApi.Models.User
{
    public class UpdateUserRequest
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int? Age { get; set; }
    }
}