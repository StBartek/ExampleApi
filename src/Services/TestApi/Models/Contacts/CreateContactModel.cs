using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Contacts
{
    public class CreateContactModel
    {
        public int TypeId { get; set; }
        public string Value { get; set; }
        public int? UserId { get; set; }
    }
}
