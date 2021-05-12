using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TestApi.Data;
using TestApi.Models.User;
using TestApi.Extensions;
using System;
using TestApi.Models;
using Database;
using LinqToDB;
using System.Text.RegularExpressions;
using TestApi.Models.Addresses;

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
        ///        "phone": 502502502,
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
            if(string.IsNullOrEmpty(model.FirstName) || model.FirstName.Length < 3)
            {
                return BadRequest("Imię powinno mieć minimum 3 znaki");
            }
            if (string.IsNullOrEmpty(model.Surname) || model.Surname.Length < 3)
            {
                return BadRequest("Nazwisko powinno mieć minimum 3 znaki.");
            }
            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 6)
            {
                return BadRequest("Hasło powinno mieć minimum 6 znaków.");
            }


            var searchData = new List<string>();
            searchData.AddIfNotEmpty(model.FirstName);
            searchData.AddIfNotEmpty(model.Surname);
            searchData.AddIfNotEmpty(model.Phone);
            searchData.AddIfNotEmpty(model.Email);
            searchData.AddIfNotEmpty(model.Age.ToString());

            var user = new Users
            {
                FirstName = model.FirstName,
                Surname = model.Surname,
                Phone = model.Phone,
                Email = model.Email,
                Password = model.Password,
                SearchData = string.Join(" ", searchData)
            };

            if (model.Age.GetValueOrDefault() > 0)
            {
                user.Age = model.Age;
            }

            try
            {
                using var db = _dbContextFactory.Create();
                var id = db.InsertWithInt32Identity(user);
                return Ok(new { UserId = id });
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
    }
}
