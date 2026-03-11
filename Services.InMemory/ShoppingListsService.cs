using Models;

namespace Services.InMemory
{
    public class ShoppingListsService : GenericService<ShoppingList>
    {
        public ShoppingListsService() : base([
                                                new ShoppingList { Id = 1, Name = "Groceries" },
                                                new ShoppingList { Id = 2, Name = "Hardware Store" },
                                                new ShoppingList { Id = 3, Name = "Pharmacy" }
                                                ])
        { }
    }
}
