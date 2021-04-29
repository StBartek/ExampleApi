using System;

namespace TestApi.Models.User
{
    public class DeleteAddressRequest
    {
        public int UserId { get; set; }
        public Guid AddressId { get; set; }
    }
}
