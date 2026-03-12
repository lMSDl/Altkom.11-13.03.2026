using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models;
using Services.Interfaces;
using WebAPI.Filters;

namespace WebAPI.Controllers
{

    [ServiceFilter<ConsoleFilter>]
    public class PeopleController : GenericResourceController<Person>
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
        public async Task<ActionResult<IEnumerable<Person>>> GetByName(string? search)
        {
            if (search is null)
                return await Get();

            var people = await _service.GetByNameAsync(search);
            return Ok(people);
        }


        //[NonAction] - wylączamy endpoint usuwania osób, bo nie chcemy, żeby ktoś mógł usunąć osobę z naszej bazy danych
        [NonAction]
        public override Task<ActionResult> Delete(int id)
        {
            return base.Delete(id);
        }

        public override async Task<ActionResult<int>> Add(Person entity)
        {
            //odfiltrowanie błędów walidacji związanych z właściwością Parent, ponieważ chcemy umożliwić dodawanie osób bez konieczności podawania rodzica, a jednocześnie nie chcemy, żeby błędy walidacji związane z tą właściwością blokowały dodawanie nowych osób
            ModelState.RemoveAll<Person>(x => x.Parent);

            //dodanie błędu o tym, że osoba o takim imieniu i nazwisku już istnieje, jeśli metoda GetByNameAsync zwróci jakieś osoby, które mają takie samo imię i nazwisko jak osoba, którą chcemy dodać do bazy danych. Dzięki temu użytkownik będzie wiedział, że nie może dodać osoby o takim samym imieniu i nazwisku, co już istniejąca osoba w bazie danych
            var people = await _service.GetByNameAsync(entity.FullName);
            if(people.Any())
            {
                ModelState.AddModelError(nameof(entity.FullName), "Osoba o takim imieniu i nazwisku już istnieje.");
            }


            return await base.Add(entity);
        }
    }
}
