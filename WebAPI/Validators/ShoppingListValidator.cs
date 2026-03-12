using FluentValidation;
using Models;
using Services.Interfaces;

namespace WebAPI.Validators
{
    public class ShoppingListValidator : AbstractValidator<Models.ShoppingList>
    {
        public ShoppingListValidator(IGenericService<ShoppingList> service)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.")
                .MinimumLength(5)
                .WithMessage("Name must be at least 5 characters long.")
                .WithName("alamakota");

            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.Now);
            //.WithMessage("CreatedAt cannot be in the future.");


            //RuleFor(x => x).MustAsync(async (item, cancellationToken) =>
            RuleFor(x => x.Name).MustAsync(async (item, name, cancellationToken) =>
            {
                var items = await service.ReadAsync();
                return !items.Any(xx => xx.Name == name);

            }).WithMessage("Nie przyjmujemy duplikatów");
        }
    }
}
