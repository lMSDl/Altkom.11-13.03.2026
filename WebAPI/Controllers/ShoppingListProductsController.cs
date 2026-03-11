using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("/api/ShoppingLists/{parentId:int}/Products")]
    public class ShoppingListProductsController : GenericNestedResourceController<Product, ShoppingList>
    {
        public ShoppingListProductsController(IGenericService<ShoppingList> parentService, IGenericService<Product> service)
            : base(parentService, service)
        {
        }

        protected override ActionResult<int> CreatedAtAction(int id)
        {
            return CreatedAtAction(nameof(ProductsController.GetById), "Products", new { id }, id);
        }

        protected override int GetParentId(Product entity)
        {
            return entity.ShoppingListId;
        }

        protected override void SetParentId(Product entity, int parentId)
        {
            entity.ShoppingListId = parentId;
        }
    }
}
