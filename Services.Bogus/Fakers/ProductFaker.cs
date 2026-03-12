namespace Services.Bogus.Fakers
{
    public class ProductFaker : EntityFaker<Models.Product>
    {
        public ProductFaker() : base()
        {
            RuleFor(p => p.Name, f => f.Commerce.ProductName());
            RuleFor(p => p.Price, f => f.Finance.Amount(1, 1000));
            RuleFor(p => p.Quantity, f => f.Random.Int(1, 100));
            RuleFor(p => p.ShoppingListId, f => f.Random.Int(1, 10));
        }
    }
}
