using Microsoft.AspNetCore.Mvc;

namespace TestApi.Api
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class BaseController : ControllerBase { }
}
