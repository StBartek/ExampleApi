namespace TestApi.Models.Contacts
{
    public class ContactGridParams
    {
        public int? TypeId { get; set; }
        public string Value { get; set; }
        public int?  UserId { get; set; }
        public int CurrentPage { get; set; }
    }
}
