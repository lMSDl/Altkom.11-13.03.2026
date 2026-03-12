using Bogus;
using Microsoft.Extensions.Options;
using Models;
using Services.InMemory;

namespace Services.Bogus
{
    public class BogusPeopleService : PeopleService
    {
        public BogusPeopleService(Faker<Models.Person> faker, IOptions<BogusConfig> options) : base(faker.Generate(options.Value.NumberOfResources))
        {
        }
    }
}
