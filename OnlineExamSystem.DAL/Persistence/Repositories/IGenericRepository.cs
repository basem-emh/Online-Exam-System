using OnlineExamSystem.DAL.Entities;

namespace OnlineExamSystem.DAL.Persistence.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetAllAsQueryable();
        Task<T?> GetAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
