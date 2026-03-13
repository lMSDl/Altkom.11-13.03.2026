using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    //adnotacje [Route("api/[controller]")] i [ApiController] są dziedziczone z ApiControllerBase, więc nie musimy ich powtarzać w każdym kontrolerze
    
    public class ValuesController : ApiControllerBase
    {
        private readonly List<int> _values;
        private readonly IHubContext<ValuesHub> _hubContext;

        //wstrzykujemy listę wartości do kontrolera pochodzącą z kontenera DI, który został skonfigurowany w Program.cs
        public ValuesController(List<int> values, IHubContext<ValuesHub> hubContext)
        {
            _values = values;
            _hubContext = hubContext;
        }

        [HttpGet]
        public int[] Read()
        {
            return _values.ToArray();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete] //domyślny route template to "api/values", więc jeśli nie podamy żadnego, to będzie on używany
        [HttpDelete("path/{value:int}")] //jeśli podamy route template, to będzie on używany zamiast domyślnego, więc endpoint będzie api/values/path/{value}
        [HttpDelete("/wartosci/{value:int}")] //jeśli podamy route template zaczynający się od "/", to będzie on traktowany jako absolutny, więc endpoint będzie /wartosci/{value} bez prefiksu api/values
        public void Delete(int value)
        {
            _values.Remove(value);
            _hubContext.Clients.All.SendAsync("ValueRemoved", value); //powiadamiamy wszystkich klientów po usunięciu wartości, dzięki czemu mogą oni zaktualizować swoje dane bez konieczności odświeżania strony
        }

        [HttpPost("{value:int:max(50)}")] //metody "walidacyjne" określone w route template (np. max(50)) są sprawdzane przed wywołaniem metody, więc jeśli wartość przekroczy 50, to metoda nie zostanie wywołana, a klient otrzyma odpowiedź 400 Bad Request
        public void Post(int value)
        {
            _values.Add(value);
            _hubContext.Clients.All.SendAsync("ValueAdded", value); //powiadamiamy wszystkich klientów po dodaniu wartości, dzięki czemu mogą oni zaktualizować swoje dane bez konieczności odświeżania strony
        }


        [HttpPut("{oldValue:int}")]
        public void Put(int oldValue, [FromQuery] int newValue) //[FromQuery] - określa, że wartość newValue ma być pobierana z query stringa, a nie z route parameters, więc endpoint będzie api/values/{oldValue}?newValue={newValue}
        {
            _values[oldValue] = newValue;
            _hubContext.Clients.All.SendAsync("ValueUpdated", oldValue, newValue); //powiadamiamy wszystkich klientów po zaktualizowaniu wartości, dzięki czemu mogą oni zaktualizować swoje dane bez konieczności odświeżania strony
        }
    }
}
