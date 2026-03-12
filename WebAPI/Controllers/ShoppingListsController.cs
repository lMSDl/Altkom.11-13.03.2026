using FluentValidation;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    public class ShoppingListsController : GenericResourceController<Models.ShoppingList>
    {
        public ShoppingListsController(IGenericService<Models.ShoppingList> service, IValidator<ShoppingList> validator) : base(service, validator)
        {
        }
    }
}
