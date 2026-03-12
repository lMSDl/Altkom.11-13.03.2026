using Bogus;
using Models;
using Services.InMemory;

namespace Services.Bogus
{
    public class BogusGenericService<T> : GenericService<T> where T : Entity
    {
        public BogusGenericService(Faker<T> faker) : base(faker.Generate(10))
        {
        }
    }
}
