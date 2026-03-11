namespace Services.Interfaces
{
    public interface IGenericService<T>
    {
        Task<int> CreateAsync(T entity);
        Task<IEnumerable<T>> ReadAsync(Func<T, bool> func);
        Task<IEnumerable<T>> ReadAsync();
        Task<T?> ReadAsync(int id);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
    
    }
}
