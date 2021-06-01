namespace TestApi.Models.Contacts
{
    public class CreateContactModel
    {
        public int? TypeId { get; set; }
        public string Value { get; set; }
        public int? UserId { get; set; }
    }
}
