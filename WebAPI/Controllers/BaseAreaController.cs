
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Area("legacy")]
    [Route("api/[area]/[controller]")]
    public class BaseAreaController : ApiControllerBase
    {
    }
}
