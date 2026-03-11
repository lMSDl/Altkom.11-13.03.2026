using Models;
using Services.Interfaces;

namespace Services.InMemory
{
    public class GenericService<T> : IGenericService<T> where T : Entity
    {

        protected readonly ICollection<T> _entities;

        public GenericService()
        {
            _entities = new HashSet<T>();
        }

        protected GenericService(ICollection<T> entities)
        {
            _entities = entities;
        }

        public Task<int> CreateAsync(T entity)
        {
            entity.Id = _entities.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _entities.Add(entity);

            return Task.FromResult(entity.Id);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await ReadAsync(id);
            if(entity is not null)
                _entities.Remove(entity);
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult(_entities.AsEnumerable());
        }

        public Task<T?> ReadAsync(int id)
        {
            return Task.FromResult( _entities.SingleOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var existing = await ReadAsync(id);
            if (existing is null)
            {
                return;
            }
            _entities.Remove(existing);
            entity.Id = id;
            _entities.Add(entity);
        }
    }
}
