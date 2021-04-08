using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TestApi.Models.User;

namespace TestApi.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        // GET: UserController
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new List<UserModel> {
                new UserModel
                {
                    FirstName = "Jan",
                    Surname = "Kowalski",
                    Phone = 501501501,
                    Email = "janKowalski@onet.com",
                    Age = 21
                },
                 new UserModel
                {
                    FirstName = "Adam",
                    Surname = "Śmiały",
                    Phone = 501501502,
                    Email = "jadams@onet.com",
                    Age = 31
                }
            });
        }

        // GET: UserController/Details/5
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            return Ok();
        }        

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserModel collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return Ok();
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
