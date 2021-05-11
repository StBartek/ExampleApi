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
        /// <param name="paramsData"></param>
        /// <returns>Users list</returns>
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
        /// <param name="id"></param>
        /// <returns>User data</returns>
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
        ///     Create new user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /
        ///     {
        ///        "FirstName": "jola",
        ///        "Surname": "Nowakowska",
        ///        "Phone": 502502502,
        ///        "Email": "jola@o2.pl",
        ///        "Age": 21
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>UserId</returns>
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

            using var db = _dbContextFactory.Create();
            if (!db.Cities.Any(x => x.CityId == model.CityId))
            {
                return BadRequest("Miasto nie istnieje.");
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
                //CityId = model.CityId,
                Password = model.Password,
                SearchData = string.Join(" ", searchData)
            };

            if(model.StreetId > 0)
            {
                //user.StreetId = model.StreetId;
            }
            if (model.Age.GetValueOrDefault() > 0)
            {
                user.Age = model.Age;
            }

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

        //// POST: UserController/Edit/5
        //[HttpPost]
        //public IActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return Ok();
        //    }
        //}

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using var db = _dbContextFactory.Create();
                var result = db.Users.Delete(x => x.UserId == id);
                if (result > 0)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
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

            var userAddress = new UsersLAddresses
            {
                UserId = model.UserId,
                AddressId = model.AddressId
            };
            
            return Ok(db.Insert(userAddress) > 0);
        }

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
