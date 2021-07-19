using Database;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestApi.Models.Addresses;
using TestApi.Models.User;

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
            var query = db.Addresses
                .LoadWith(x => x.City)
                .LoadWith(x => x.Street)
                .LoadWith(x => x.UsersLAddresses)
                .AsQueryable();

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

            var result = query.OrderBy(x => x.City.Name).Skip(gridParams.CurrentPage * 10 - 10).Take(10)
                .Select(x => new AddressGridViewModel(x))
                .ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult Details(Guid id)
        {
            using var db = _dbContextFactory.Create();
            var address = db.Addresses
                .LoadWith(x => x.City)
                .LoadWith(x => x.Street)
                .LoadWith(x => x.UsersLAddresses.FirstOrDefault().User.Contacts)
                .FirstOrDefault(x => x.AddressId == id);

            if (address is Addresses)
            {
                return Ok(new FullAddressViewModel(address));
            }
            return NotFound();
        } 

        [HttpPost]
        public ActionResult Create(CreateAddressModel model)
        {
            model.FlatNo = string.IsNullOrEmpty(model.FlatNo) ? null : model.FlatNo;

            using var db = _dbContextFactory.Create();

            if (!model.CityId.HasValue)
            {
                return BadRequest("Miejscowość nie została podana.");
            }
            if (!db.Cities.Any(x => x.CityId == model.CityId))
            {
                return BadRequest("Miejscowość nie istnieje.");
            }
            if (model.StreetId.HasValue)
            {
                var street = db.Streets.FirstOrDefault(x => x.StreetId == model.StreetId);
                if(street == null)
                {
                    return BadRequest("Ulica nie istnieje.");
                }
                if(street.CityId != model.CityId)
                {
                    return BadRequest("Wybrana ulica nie znajduje się w wybranym mieście.");
                }
            }
            else if (db.Streets.Any(x => x.CityId == model.CityId))
            {
                return BadRequest("Wybrana miejscowość posiada ulice.");
            }
            
            if (string.IsNullOrEmpty(model.HouseNo))
            {
                return BadRequest("Nie podano numeru budynku.");
            }

            var addresses = db.Addresses
                .Where(x => x.CityId == model.CityId && x.StreetId == model.StreetId && x.HouseNo == model.HouseNo)
                .ToList();

            var error = BaseValid(model, addresses);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var address = new Addresses
            {
                AddressId = Guid.NewGuid(),
                CityId = model.CityId.Value,
                StreetId = model.StreetId,
                HouseNo = model.HouseNo,
                FlatNo = model.FlatNo
            };

            db.Insert(address);
            return Ok(address.AddressId);
        }

        private string BaseValid(CreateAddressModel model, List<Addresses> addresses) 
        {
            if (string.IsNullOrEmpty(model.FlatNo))
            {
                if (addresses.Any(x => !string.IsNullOrEmpty(x.FlatNo)))
                {
                    return "Proszę podać numer lokalu, pod podanym numerem budynku istnieją adresy z podanym numerem lokalu.";
                }
                if (addresses.Any())
                {
                    return "Podany adres już istnieje.";
                }
            }
            else
            {
                if (addresses.Any(x => string.IsNullOrEmpty(x.FlatNo)))
                {
                    return "Pod podanym numerem budynku istnieje adres bez numeru lokalu.";
                }
                if (addresses.Any(x => x.FlatNo == model.FlatNo))
                {
                    return "Podany adres już istnieje.";
                }
            }

            if (string.IsNullOrEmpty(model.HouseNo))
            {
                return "Numer budynku jest wymagany.";
            }

            Regex regex = new Regex(@"[a-zA-Z0-9]+$");
            if (!regex.IsMatch(model.HouseNo))
            {
                return "Numer budynku może zawierać wyłącznie litery i cyfry.";
            }
            if (!string.IsNullOrEmpty(model.FlatNo) &&  !regex.IsMatch(model.FlatNo))
            {
                return "Numer lokalu może zawierać wyłącznie litery i cyfry";
            }
            return null;
        }

        [HttpPatch("{id}")]
        public ActionResult Edit(Guid id, UpdateAddressModel model)
        {
            if (id != model.AddressId)
            {
                return NotFound();
            }

            model.FlatNo = string.IsNullOrEmpty(model.FlatNo) ? null : model.FlatNo;

            using var db = _dbContextFactory.Create();
            var curentAddress = db.Addresses.FirstOrDefault(x => x.AddressId == model.AddressId);
            if(curentAddress == null)
            {
                return NotFound();
            }
            var addresses = db.Addresses
                .Where(x => x.AddressId != curentAddress.AddressId 
                    && x.CityId == curentAddress.CityId 
                    && x.StreetId == curentAddress.StreetId 
                    && x.HouseNo == model.HouseNo         
               ).ToList();

            var modelToValid = new CreateAddressModel 
            {
                CityId = curentAddress.CityId,
                StreetId = curentAddress.StreetId,
                HouseNo = model.HouseNo,
                FlatNo = model.FlatNo
            };

            var error = BaseValid(modelToValid, addresses);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var result = db.Addresses.Where(x => x.AddressId == model.AddressId)
                .Set(x => x.HouseNo, model.HouseNo)
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

        [HttpGet("{id}/users")]
        public IActionResult GetUsers(Guid id)
        {
            using var db = _dbContextFactory.Create();
            var userAddresses = db.UsersLAddresses
                .LoadWith(x => x.User.Contacts)
                .Where(x => x.AddressId == id)
                .Select(x => new UserWithContactViewModel(x.User))
                .ToList();

            return Ok(userAddresses);
        }
    }
}
