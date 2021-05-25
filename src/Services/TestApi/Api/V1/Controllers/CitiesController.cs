using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    public class CitiesController: BaseController
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly IDbContextFactory _dbContextFactory;

        public CitiesController(ILogger<CitiesController> logger, IDbContextFactory dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            using var db = _dbContextFactory.Create();
            return Ok(db.Cities.Select(x => new SelectViewModel
            {
                Id = x.CityId,
                Text = x.Name
            }).ToList());
        }
    }
}
