using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.DAL.Entities;
using OnlineExamSystem.DAL.Persistence.Data.Contexts;

namespace OnlineExamSystem.DAL.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return _dbContext.Set<T>();
        }
        
        public async Task<T?> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
            => await _dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity)
            => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);

    }
}
