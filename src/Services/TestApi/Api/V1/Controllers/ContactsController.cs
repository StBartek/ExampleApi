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

        /// <summary>
        /// Get contacts list.
        /// </summary>
        /// <response code="200">Returns contacts list</response>
        /// <response code="500">If database return error</response>     
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
            if (paramsData.UserId.HasValue)
            {
                query = query.Where(x => x.UserId == paramsData.UserId);
            }
            if (!string.IsNullOrEmpty(paramsData.Value))
            {
                query = query.Where(x => x.Value.Contains(paramsData.Value, StringComparison.InvariantCultureIgnoreCase));
            }
            return Ok(query.Select(x => new UserWithContactViewModel(x)).ToList());
        }

        /// <summary>
        /// Get contact by contactId
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <response code="200">Returns contact data</response>
        /// <response code="404">If contact not found</response>     
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

        /// <summary>
        /// Create new contact
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /
        ///     {
        ///        "typeId": "1",
        ///        "value": 502502502,
        ///        "userId": "1"
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Create contact model</param>
        /// <response code="200">Returns ContactId</response>
        /// <response code="400">If one or more validation errors occurred</response>
        /// <response code="500">If something goes wrong</response> 
        [HttpPost]
        public IActionResult Create(CreateContactModel model)
        {
            using var db = _dbContextFactory.Create();

            if (string.IsNullOrEmpty(model.Value))
            {
                return BadRequest("Wartość kontaktu nie została uzupełniona");
            }
            if (!model.TypeId.HasValue)
            {
                return BadRequest("Typ kontaktu nie został podany");
            }
            if (!db.DicContactType.Any(x => x.DicContactTypeId == model.TypeId))
            {
                return BadRequest("Typ kontaktu nie istnieje.");
            }
            if (model.UserId.HasValue && !db.Users.Any(x => x.UserId == model.UserId))
            {
                return BadRequest("Użytkownik nie istnieje.");
            }
            if (db.Contacts.Any(x => x.UserId == model.UserId && x.TypeId == model.TypeId))
            {
                return BadRequest("Użytkownik posiada już kontakt takiego typu.");
            }

            var contact = new Contacts
            {
                Value = model.Value,
                TypeId = model.TypeId.Value,
            };
            if (model.UserId.HasValue)
            {
                contact.UserId = model.UserId;
            }

            var id = db.InsertWithInt32Identity(contact);
            return Ok(id);
        }

        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Patch /
        ///     {
        ///        "contactId": 2,
        ///        "userId": "1"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Contact id</param>
        /// <param name="model">Update contact model</param>
        /// <response code="200">Returns true if contact is updated</response>
        /// <response code="400">If one or more validation errors occurred</response>
        /// <response code="500">If something goes wrong</response> 
        [HttpPatch("{id}")]
        public IActionResult Update(int id, UpdateContactModel model)
        {
            if(id != model.ContactId)
            {
                return NotFound();
            }

            using var db = _dbContextFactory.Create();
            if (model.UserId.HasValue)
            {
                if (!db.Users.Any(x => x.UserId == model.UserId))
                {
                    return BadRequest("Użytkownik nie istnieje.");
                }
                var contact = db.Contacts.FirstOrDefault(x => x.ContactId == id);
                if (db.Contacts.Any(x => x.ContactId != id && x.UserId == model.UserId && x.TypeId == contact.TypeId))
                {
                    return BadRequest("Użytkownik posiada już kontakt takiego typu.");
                }
            }

            var updateCount = db.Contacts.Where(x => x.ContactId == id)
                .Set(x => x.UserId, model.UserId)
                .Update();

            return Ok(updateCount > 0);
        }

        /// <summary>
        /// Delete contact by contactId
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <response code="200">Returns true if contact is deleted</response>
        /// <response code="404">If contact not found</response>     
        /// <response code="500">If something goes wrong</response> 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using var db = _dbContextFactory.Create();
                var result = db.Contacts.Delete(x => x.ContactId == id);
                if (result > 0)
                {
                    return Ok(true);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
