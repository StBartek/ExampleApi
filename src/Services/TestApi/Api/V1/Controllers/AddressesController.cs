using Database;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models.Addresses;

namespace TestApi.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    public class AddressesController : BaseController
    {
        private readonly ILogger<AddressesController> _logger;
        private readonly IDbContextFactory _dbContextFactory;

        public AddressesController(ILogger<AddressesController> logger, IDbContextFactory dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        [HttpGet]
        public ActionResult Index([FromQuery] AddressGridParams gridParams)
        {
            using var db = _dbContextFactory.Create();
            var query = db.Addresses.AsQueryable();
            if (gridParams.CityId.HasValue)
            {
                query = query.Where(x => x.CityId == gridParams.CityId);
            }
            if (gridParams.StreetId.HasValue)
            {
                query = query.Where(x => x.StreetId == gridParams.StreetId);
            }
            if (!string.IsNullOrEmpty(gridParams.HouseNo))
            {
                query = query.Where(x => x.HouseNo.Contains(gridParams.HouseNo));
            }
            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult Details(Guid id)
        {
            using var db = _dbContextFactory.Create();
            var address = db.Addresses.FirstOrDefault(x => x.AddressId == id);

            if (address is Addresses)
            {
                return Ok(address);
            }
            return NotFound();
        } 

        [HttpPost]
        public ActionResult Create(CreateAddressModel model)
        {
            using var db = _dbContextFactory.Create();
           
            if (!db.Cities.Any(x => x.CityId == model.CityId))
            {
                return BadRequest("Miasto nie istnieje.");
            }
            if (model.StreetId.HasValue && !db.Streets.Any(x => x.StreetId == model.StreetId))
            {
                return BadRequest("Ulica nie istnieje.");
            }
            if (string.IsNullOrEmpty(model.HouseNo))
            {
                return BadRequest("Nie podano numeru domu.");
            }

            var address = new Addresses
            {
                AddressId = Guid.NewGuid(),
                CityId = model.CityId,
                StreetId = model.StreetId,
                HouseNo = model.HouseNo,
                FlatNo = model.FlatNo
            };

            db.Insert(address);
            return Ok(address.AddressId);
        }

        [HttpPut("{id}")]
        public ActionResult Edit(Guid id, UpdateAddressModel model)
        {
            if (id != model.AddressId)
            {
                return BadRequest();
            }

            using var db = _dbContextFactory.Create();
            var result = db.Addresses.Where(x => x.AddressId == model.AddressId)
                .Set(x => x.FlatNo, model.FlatNo)
                .Update();

            if (result > 0)
            {
                return Ok(true);
            }

            return BadRequest("Adres nie istnieje.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                using var db = _dbContextFactory.Create();
                var result = db.Addresses.Delete(x => x.AddressId == id);
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
    }
}
