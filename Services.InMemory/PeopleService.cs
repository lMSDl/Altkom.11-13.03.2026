using Models;
using Services.Interfaces;

namespace Services.InMemory
{
    public class PeopleService : GenericService<Person>, IPeopleService
    {
        public PeopleService() { }
        public PeopleService(ICollection<Person> initialData) : base(initialData) { }

        public Task<IEnumerable<Person>> GetByNameAsync(string name)
        {
            var people = _entities.Where(p => $"{p.FirstName} {p.LastName}".Contains(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(people.AsEnumerable());
        }
    }
}
