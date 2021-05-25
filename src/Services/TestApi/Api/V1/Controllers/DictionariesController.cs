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
    public class DictionariesController : BaseController
    {
        private readonly ILogger<DictionariesController> _logger;
        private readonly IDbContextFactory _dbContextFactory;

        public DictionariesController(ILogger<DictionariesController> logger, IDbContextFactory dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        [HttpGet("contacttypes")]
        public IActionResult Index()
        {
            using var db = _dbContextFactory.Create();
            var data = db.DicContactType
                .Select(x => new SelectViewModel(x.DicContactTypeId, x.Name))
                .ToList();
            return Ok(data);
        }

    }
}
