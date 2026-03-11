using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{

    public class PeopleController : GenericController<Person>
    {
        private readonly IPeopleService _service;
        public PeopleController(IPeopleService service) : base(service)
        {
            _service = service;
        }

        //rozbudowujemy API o endpoint do wyszukiwania osób po imieniu lub nazwisku, który będzie korzystał z metody GetByNameAsync z serwisu
        /*[HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Person>>> GetByName(string name)
        {
            var service = _service;
            var people = await service.GetByNameAsync(name);
            return Ok(people);
        }*/

        //[NonAction] - przydaje się nie tylko do blokowania niechcianych endpointów, ale także do rozwiązania konfliktów nazw metod w kontrolerze, które mogą wystąpić, gdy dziedziczymy z klasy bazowej i chcemy nadpisać tylko niektóre metody, a inne pozostawić bez zmian
        [NonAction]
        public override Task<ActionResult<IEnumerable<Person>>> Get()
        {
            return base.Get();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetByName(string search)
        {
            var service = _service;
            var people = await service.GetByNameAsync(search);
            return Ok(people);
        }


        //[NonAction] - wylączamy endpoint usuwania osób, bo nie chcemy, żeby ktoś mógł usunąć osobę z naszej bazy danych
        [NonAction]
        public override Task<ActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
