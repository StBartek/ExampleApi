using System;

namespace TestApi.Models.User
{
    public class AddAddressRequest
    {
        public int UserId { get; set; }
        public Guid AddressId { get; set; }
    }
}
