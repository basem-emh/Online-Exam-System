using OnlineExamSystem.DAL.Entities;
using OnlineExamSystem.DAL.Persistence.Repositories;

namespace OnlineExamSystem.DAL.Persistence.Unit_Of_Work
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();
    }
}
