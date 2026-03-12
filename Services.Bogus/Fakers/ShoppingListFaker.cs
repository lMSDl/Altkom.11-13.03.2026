using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Bogus.Fakers
{
    public class ShoppingListFaker : EntityFaker<Models.ShoppingList>
    {
        public ShoppingListFaker() : base()
        { 
        
            RuleFor(x => x.Name, f => f.Commerce.ProductName());
            RuleFor(x => x.CreatedAt, f => f.Date.Past(1));

        }
    }
}
