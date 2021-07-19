using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Models.Addresses;

namespace TestApi.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    public class StreetsController: BaseController
    {
        private readonly ILogger<StreetsController> _logger;
        private readonly IDbContextFactory _dbContextFactory;

        public StreetsController(ILogger<StreetsController> logger, IDbContextFactory dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] AddressGridParams streetParams)
        {
            using var db = _dbContextFactory.Create();
            return Ok(db.Streets.Where(x => x.CityId == streetParams.CityId)
                .Select(x => new SelectViewModel { 
                    Id = x.StreetId,
                    Text = x.Name
                })
                .ToList());
        }
    }
}
