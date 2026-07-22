namespace Order.API.Repository
{
    public interface IGenericRepository<T>
      where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task AddAsync(T entity);

        void Update(T entity);

        void Remove(T entity);
    }
}
