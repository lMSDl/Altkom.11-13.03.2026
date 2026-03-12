using Bogus;
using Models;

namespace Services.Bogus.Fakers
{
    public class EntityFaker<T> : Faker<T> where T : Entity
    {
        public EntityFaker(string lang) : base(lang)
        {
            RuleFor(e => e.Id, f => f.IndexFaker + 1);
        }

    }
}
