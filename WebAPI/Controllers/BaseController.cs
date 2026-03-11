using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")] // adres naszego kontrolera będzie api/nazwa_kontrolera
    [ApiController]// oznaczamy, że jest to kontroler API, co pozwala na automatyczne mapowanie danych z żądania HTTP do parametrów metod
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
