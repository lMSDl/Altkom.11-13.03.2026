using Bogus;
using Models;
using Services.InMemory;

namespace Services.Bogus
{
    public class BogusPeopleService : PeopleService
    {
        public BogusPeopleService(Faker<Models.Person> faker) : base(faker.Generate(10))
        {
        }
    }
}
