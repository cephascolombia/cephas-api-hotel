using System.Linq.Expressions;

namespace Hotel.Application.Interfaces.Commons
{
    public interface IGenericRepository<T> where T : class 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<(IEnumerable<T> Data, int TotalRecords)> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null 
        );
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
