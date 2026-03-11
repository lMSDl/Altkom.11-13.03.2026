using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebAPI.Controllers
{
    public class ShoppingListController : ApiControllerBase
    {
        private readonly List<ShoppingList> _service;

        public ShoppingListController(List<ShoppingList> service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShoppingList>> GetShoppingList()
        {
            return Ok(_service);
        }

        [HttpGet("{id}")]
        public ActionResult<ShoppingList> GetShoppingListById(int id)
        {
            var entity = _service.SingleOrDefault(x => x.Id == id);
            if (entity is null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        //public ActionResult<int> AddShoppingList(ShoppingList shoppingList)
        public ActionResult<ShoppingList> AddShoppingList(ShoppingList shoppingList)
        {
            shoppingList.Id = _service.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _service.Add(shoppingList);

            //return CreatedAtAction(nameof(GetShoppingListById), new { id = shoppingList.Id }, shoppingList.Id);
            return CreatedAtAction(nameof(GetShoppingListById), new { id = shoppingList.Id }, shoppingList);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateShoppingList(int id, ShoppingList shoppingList)
        {
            var existing = _service.SingleOrDefault(x => x.Id == id);
            if (existing is null)
            {
                return NotFound();
            }
            _service.Remove(existing);
            shoppingList.Id = id;
            _service.Add(shoppingList);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var existing = _service.SingleOrDefault(x => x.Id == id);
            if (existing is null)
            {
                return NotFound();
            }
            _service.Remove(existing);
            return NoContent();
        }

    }
}
