using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TestApi.Models.Contacts;
using LinqToDB;
using TestApi.Models.User;

namespace TestApi.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    public class ContactsController : BaseController
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IDbContextFactory _dbContextFactory;

        public ContactsController(ILogger<ContactsController> logger, IDbContextFactory dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] ContactGridParams paramsData)
        {
            using var db = _dbContextFactory.Create();
            var query = db.Contacts
                .LoadWith(x => x.User)
                .AsQueryable();

            if (paramsData.TypeId.HasValue)
            {
                query = query.Where(x => x.TypeId == paramsData.TypeId);
            }
            if(paramsData.UserId.HasValue)
            {
                query = query.Where(x => x.UserId == paramsData.UserId);
            }
            if (!string.IsNullOrEmpty(paramsData.Value))
            {
                query = query.Where(x => x.Value.Contains(paramsData.Value, StringComparison.InvariantCultureIgnoreCase));
            }
            return Ok(query.Select(x => new UserWithContactViewModel(x)).ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            using var db = _dbContextFactory.Create();
            var contact = db.Contacts
                .LoadWith(x => x.User)
                .FirstOrDefault(x => x.ContactId == id);

            if (contact is Contacts)
            {
                return Ok(new UserWithContactViewModel(contact));
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Create(CreateContactModel model)
        {
            using var db = _dbContextFactory.Create();

            if (string.IsNullOrEmpty(model.Value) || model.Value.Length < 5)
            {
                return BadRequest("Hasło powinno mieć minimum 5 znaków.");
            }
            if (!db.DicContactType.Any(x => x.DicContactTypeId == model.TypeId))
            {
                return BadRequest("Typ kontaktu nie istnieje.");
            }
            if (model.UserId.HasValue && !db.Users.Any(x => x.UserId == model.UserId))
            {
                return BadRequest("Użytkownik nie istnieje.");
            }

            var contact = new Contacts
            {
                Value = model.Value,
                TypeId = model.TypeId,
            };
            if (model.UserId.HasValue)
            {
                contact.UserId = model.UserId;
            }

            var id = db.InsertWithInt32Identity(contact);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateContactModel model)
        {
            using var db = _dbContextFactory.Create();

            var updateCount = db.Contacts.Where(x => x.ContactId == id)
                .Set(x => x.UserId, model.UserId)
                .Update();

            return Ok(updateCount > 0);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var db = _dbContextFactory.Create();
            var deletedCount = db.Contacts.Delete(x => x.ContactId == id);
            return Ok(deletedCount > 0);
        }
    }
}
