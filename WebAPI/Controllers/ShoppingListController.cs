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
        public IEnumerable<ShoppingList> GetShoppingList()
        {
            return _service;
        }

        [HttpGet("{id}")]
        public ShoppingList? GetShoppingListById(int id)
        {
            return _service.SingleOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public int AddShoppingList(ShoppingList shoppingList)
        {
            shoppingList.Id = _service.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _service.Add(shoppingList);

            return shoppingList.Id;
        }

        [HttpPut("{id:int}")]
        public void UpdateShoppingList(int id, ShoppingList shoppingList)
        {
            var existing = _service.SingleOrDefault(x => x.Id == id);
            if (existing is null)
            {
                return;
            }
            _service.Remove(existing);
            shoppingList.Id = id;
            _service.Add(shoppingList);
        }

    }
}
