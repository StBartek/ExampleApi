using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TestApi.Models.User;
using TestApi.Extensions;
using System;
using Database;
using LinqToDB;
using System.Text.RegularExpressions;
using TestApi.Models.Addresses;
using TestApi.Models;

namespace TestApi.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IDbContextFactory _dbContextFactory;

        public UsersController(ILogger<UsersController> logger, IDbContextFactory dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Get users list.
        /// </summary>
        /// <response code="200">Returns users list</response>
        /// <response code="500">If database return error</response>     
        [HttpGet]
        public IActionResult Index([FromQuery] UserGridParams paramsData)
        {
            using var db = _dbContextFactory.Create();

            var query = db.Users.AsQueryable();
            if (!string.IsNullOrEmpty(paramsData.FirstName))
            {
                query = query.Where(x => x.FirstName.Contains(paramsData.FirstName, StringComparison.InvariantCultureIgnoreCase)); 
            }
            if (!string.IsNullOrEmpty(paramsData.Surname))
            {
                query = query.Where(x => x.Surname.Contains(paramsData.Surname, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(paramsData.Email))
            {
                query = query.Where(x => x.Email.Contains(paramsData.Email, StringComparison.InvariantCultureIgnoreCase));
            }
            
            if (!string.IsNullOrEmpty(paramsData.SearchData))
            {
                query = query.Where(x => x.SearchData.Contains(paramsData.SearchData, StringComparison.InvariantCultureIgnoreCase));
            }

            if (paramsData.Age != null)
            {
                query = query.Where(x => x.Age == paramsData.Age);
            }

            var result = query.ToList();

            return Ok(result);
        }

        /// <summary>
        /// Get user by userId
        /// </summary>
        /// <param name="id">User id</param>
        /// <response code="200">Returns user data</response>
        /// <response code="404">If user not found</response>     
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            using var db = _dbContextFactory.Create();
            var user = db.Users
                .FirstOrDefault(x => x.UserId == id);

            if(user is Users)
            {
                return Ok(new UserGridViewModel(user));
            }
            return NotFound();
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /
        ///     {
        ///        "firstName": "jola",
        ///        "surname": "Nowakowska",
        ///        "email": "jola@o2.pl",
        ///        "age": 21,
        ///        "password": "alamakota"
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">Returns UserId</response>
        /// <response code="400">If one or more validation errors occurred</response>
        /// <response code="500">If something goes wrong</response> 
        [HttpPost]
        public IActionResult Create(CreateUserRequest model)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(model.Email ?? string.Empty))
            {
                return BadRequest("Nieprawidłowy adres email.");
            }

            using var db = _dbContextFactory.Create();
            if (db.Users.Any(x => x.Email.Contains(model.Email)))
            {
                return BadRequest("Uzytkownik o podanym adresie email już istnieje.");
            }
            if (string.IsNullOrEmpty(model.FirstName) || model.FirstName.Length < 3)
            {
                return BadRequest("Imię powinno mieć minimum 3 znaki.");
            }
            if (string.IsNullOrEmpty(model.Surname) || model.Surname.Length < 3)
            {
                return BadRequest("Nazwisko powinno mieć minimum 3 znaki.");
            }
            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 6)
            {
                return BadRequest("Hasło powinno mieć minimum 6 znaków.");
            }
            if(model.Age.HasValue && model.Age <= 0) 
            {
                return BadRequest("Wiek powinien być cyfrą dodatnią.");
            }

            var user = new Users
            {
                FirstName = model.FirstName,
                Surname = model.Surname,
                Email = model.Email,
                Password = model.Password,
                Age = model.Age                
            };
            SetSearchData(user);

            try
            {
                var id = db.InsertWithInt32Identity(user);
                return Ok(new { UserId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);                
            }            
        }

        private void SetSearchData(Users model)
        {
            var searchData = new List<string>();
            searchData.AddIfNotEmpty(model.FirstName);
            searchData.AddIfNotEmpty(model.Surname);
            searchData.AddIfNotEmpty(model.Email);
            searchData.AddIfNotEmpty(model.Age.ToString());
            model.SearchData = string.Join(" ", searchData);
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Patch /
        ///     {
        ///        "userId": 2,
        ///        "firstName": "jola",
        ///        "surname": "Nowakowska",
        ///        "age": 21,
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">Returns true if update is success</response>
        /// <response code="400">If one or more validation errors occurred</response>
        /// <response code="500">If something goes wrong</response> 
        [HttpPatch("{id}")]
        public IActionResult Update(int id, UpdateUserRequest model)
        {
            if(id != model.UserId)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(model.FirstName) || model.FirstName.Length < 3)
            {
                return BadRequest("Imię powinno mieć minimum 3 znaki.");
            }
            if (string.IsNullOrEmpty(model.Surname) || model.Surname.Length < 3)
            {
                return BadRequest("Nazwisko powinno mieć minimum 3 znaki.");
            }
            if (model.Age.HasValue && model.Age <= 0)
            {
                return BadRequest("Wiek powinien być cyfrą dodatnią.");
            }                     

            try
            {
                using var db = _dbContextFactory.Create();
                var dbUser = db.Users.FirstOrDefault(x => x.UserId == id);
                dbUser.FirstName = model.FirstName;
                dbUser.Surname = model.Surname;
                dbUser.Age = model.Age;
                SetSearchData(dbUser);
                db.Update(dbUser);
               return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete user by userId
        /// </summary>
        /// <param name="id">User Id</param>
        /// <response code="200">Returns true if usere is deleted</response>
        /// <response code="404">If user not found</response>     
        /// <response code="500">If something goes wrong</response> 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using var db = _dbContextFactory.Create();
                var result = db.Users.Delete(x => x.UserId == id);
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

        /// <summary>
        /// Attaches address to user
        /// </summary>
        /// <param name="id">User id</param> 
        /// <response code="200">Returns true if address is attached</response>
        /// <response code="404">If user not found</response>     
        [HttpPost("{id}/addresses")]
        public IActionResult AddAddress(int id, AddAddressRequest model)
        {
            if(id != model.UserId)
            {
                return BadRequest();
            }

            using var db = _dbContextFactory.Create();
            if (!db.Users.Any(x => x.UserId == model.UserId))
            {
                return BadRequest("Użytkownik nie istnieje.");
            }
            if(model.AddressId == Guid.Empty || !db.Addresses.Any(x => x.AddressId == model.AddressId))
            {
                return BadRequest("Adres nie istnieje.");
            }
            if (db.UsersLAddresses.Any(x => x.AddressId == model.AddressId && x.UserId == model.UserId))
            {
                return BadRequest("Użytkownik ma już przypisany taki adres.");
            }

            var userAddress = new UsersLAddresses
            {
                UserId = model.UserId,
                AddressId = model.AddressId
            };
            
            return Ok(db.Insert(userAddress) > 0);
        }

        /// <summary>
        /// Detaches address from user
        /// </summary>
        /// <param name="id">User id</param> 
        /// <response code="200">Returns true if address is detached</response>
        /// <response code="404">If user not found or user id is not existing</response>     
        [HttpDelete("{id}/addresses")]
        public IActionResult DeleteAddress(int id, DeleteAddressRequest model)
        {
            if (id != model.UserId)
            {
                return BadRequest();
            }

            using var db = _dbContextFactory.Create();
            if (!db.Users.Any(x => x.UserId == model.UserId))
            {
                return BadRequest("Użytkownik nie istnieje.");
            }

            var deletedCount = db.UsersLAddresses.Delete(x => x.UserId == model.UserId && x.AddressId == model.AddressId);
            return Ok(deletedCount > 0);
        }

        /// <summary>
        /// Return users addresses
        /// </summary>
        /// <param name="id">User id</param> 
        /// <response code="200">Returns users addresses list</response>
        [HttpGet("{id}/addresses")]
        public IActionResult GetAddresses(int id)
        {
            using var db = _dbContextFactory.Create();
            var userAddresses = db.UsersLAddresses
                .LoadWith(x => x.Address.City)
                .LoadWith(x => x.Address.Street)
                .Where(x => x.UserId == id)
                .Select(x => new BaseAddressViewModel(x.Address))
                .ToList();
            return Ok(userAddresses);
        }

        [HttpGet("select")]
        public IActionResult UserForAutocomplete()
        {
            using var db = _dbContextFactory.Create();
            var data = db.Users
                .Select(x => new SelectViewModel(x.UserId, $"{x.FirstName} {x.Surname}"))
                .ToList();
            return Ok(data);
        }
    }
}
