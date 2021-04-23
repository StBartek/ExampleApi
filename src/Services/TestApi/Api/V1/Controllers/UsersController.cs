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

            //var usersTest = db.Users.ToList();
            //var temp = (from u in UsersData.Data()
            //            join c in CitiesData.Data() on u.CityId equals c.CityId
            //            join s in StreetsData.Data() on u.StreetId equals s.StreetId into ss
            //            from s2 in ss.DefaultIfEmpty()
            //            select new UserGridViewModel(u, c, s2))
            //           .ToList();

            var query = db.Users.AsQueryable();
            if (!string.IsNullOrEmpty(paramsData.FirstName))
            {
                query = query.Where(x => x.FirstName.IndexOf(paramsData.FirstName, StringComparison.InvariantCultureIgnoreCase) > -1); 
            }

            if (!string.IsNullOrEmpty(paramsData.Email))
            {
                query = query.Where(x => x.Email.IndexOf(paramsData.Email, StringComparison.InvariantCultureIgnoreCase) > -1);
            }
            
            if (!string.IsNullOrEmpty(paramsData.SearchData))
            {
                var tempArray = paramsData.SearchData.Split(" ");
                query = query.Where(x => x.SearchData.ContainsAll(tempArray));
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
            var user = db.Users.FirstOrDefault(x => x.UserId == id);
            return Ok(user);
        }

        /// <summary>
        /// Get all addresses for user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User </returns>
        /// <response code="200">Returns addresses</response>
        /// <response code="404">If addresses not found</response>     
        [HttpGet("{id}/addresses")]
        public IActionResult GetAddresses(int id)
        {
            return Ok(new List<StreetModel> {
                new StreetModel{ Name = "Maja" },
                new StreetModel{ Name = "Ugorek" }
            });
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
        public IActionResult Create(UserModel model)
        {
            if (!model.Email.Contains("@"))
            {
                return BadRequest("Błąd maila");
            }

            var searchData = new List<string>();
            searchData.AddIfNotEmpty(model.FirstName);
            searchData.AddIfNotEmpty(model.Surname);
            searchData.AddIfNotEmpty(model.Phone);
            searchData.AddIfNotEmpty(model.Email);
            searchData.AddIfNotEmpty(model.Age.ToString());

            using var db = _dbContextFactory.Create();
            var user = new Users
            {
                FirstName = model.FirstName,
                Surname = model.Surname,
                Phone = model.Phone,
                Email = model.Email,
                Age = model.Age,
                CityId = model.CityId,
                StreetId = model.StreetId,
                Password = model.Password,
                SearchData = string.Join(" ", searchData)
            };

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

        //// GET: UserController/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    return Ok();
        //}

        //// POST: UserController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
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

        // GET: UserController/Delete/5
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

        // POST: UserController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(int id, IFormCollection collection)
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
    }
}
