using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    public class LegacyShoppingListController : BaseAreaController
    {
        private readonly IGenericService<ShoppingList> _service;

        public LegacyShoppingListController(IGenericService<ShoppingList> service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShoppingList>> GetShoppingList()
        {
            return Ok(_service);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingList>> GetShoppingListById(int id)
        {
            var entity = await _service.ReadAsync(id);
            if (entity is null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddShoppingList(ShoppingList shoppingList)
        //public async Task<ActionResult<ShoppingList>> AddShoppingList(ShoppingList shoppingList)
        {
            var id = await _service.CreateAsync(shoppingList);

            return CreatedAtAction(nameof(GetShoppingListById), new { id }, id);
            //return CreatedAtAction(nameof(GetShoppingListById), new { id = shoppingList.Id }, shoppingList);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateShoppingList(int id, ShoppingList shoppingList)
        {
            var existing = await _service.ReadAsync(id);
            if (existing is null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(id, shoppingList);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _service.ReadAsync(id);
            if (existing is null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }

    }
}
