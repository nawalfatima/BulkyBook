using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();    
        void Add(T entity); 
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
//update not included inside the generic repository because it can be different for different models.
