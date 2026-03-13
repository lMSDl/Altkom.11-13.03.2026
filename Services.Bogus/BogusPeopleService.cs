using Bogus;
using Microsoft.Extensions.Options;
using Models;
using Services.InMemory;

namespace Services.Bogus
{
    public class BogusPeopleService : PeopleService
    {
        public BogusPeopleService(Faker<Models.Person> faker, int numberOfResources) : base(faker.Generate(numberOfResources))
        {
            for (int i = 1; i < _entities.Count - 5; i++)
            {
                int parentIndex = Random.Shared.Next(0, i);
                var child = _entities[i];
                var parent = _entities[parentIndex];
                child.Parent = parent;
                parent.Children.Add(child);
            }
        }
        public BogusPeopleService(Faker<Models.Person> faker, IOptions<BogusConfig> options) : this(faker, options.Value.NumberOfResources)
        {

            

        }
    }
}
