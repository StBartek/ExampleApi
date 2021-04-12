using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TestApi.Data;
using TestApi.Models.User;
using TestApi.Extensions;
using System;

namespace TestApi.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        // GET: UserController
        [HttpGet]
        public IActionResult Index([FromQuery] UserGridParams paramsData)
        {
            var temp = (from u in UsersData.Data()
                        join c in CitiesData.Data() on u.CityId equals c.CityId
                        join s in StreetsData.Data() on u.StreetId equals s.StreetId into ss
                        from s2 in ss.DefaultIfEmpty()
                        select new UserGridViewModel(u, c, s2))
                       .ToList();

            var query = temp.AsQueryable();
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

        // GET: UserController/Details/5
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            return Ok(new FullUserModel
            {
                UserId = 1,
                FirstName = "Kamila",
                Phone = 502502504,
                Age = 31,
                HouseNo = "21",
                FlatNo = "19",
                CityId = 1,
                StreetId = 1
            });
        }        

        // POST: UserController/Create
        [HttpPost]
        public IActionResult Create(UserModel model)
        {
            if (!model.Email.Contains("@"))
            {
                return BadRequest("Błąd maila");
            }
            return Ok(new { UserId = 31 });
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
            return Ok();
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
