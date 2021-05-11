using Db = Database;
using System.Linq;

namespace TestApi.Models.User
{
    public class UserWithContactViewModel
    {
        public UserWithContactViewModel(Db.Users user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            Surname = user.Surname;
            var contact = user.Contacts.FirstOrDefault();
            ContactId = contact?.ContactId;
            ContactTypeId = contact?.TypeId;
            ContactValue = contact?.Value;
        }

        public UserWithContactViewModel(Db.Contacts contact)
        {
            UserId = contact.User.UserId;
            FirstName = contact.User.FirstName;
            Surname = contact.User.Surname;
            ContactId = contact.ContactId;
            ContactTypeId = contact.TypeId;
            ContactValue = contact.Value;
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int? ContactId { get; set; }
        public string ContactValue { get; set; }
        public int? ContactTypeId { get; set; }
    }
}
