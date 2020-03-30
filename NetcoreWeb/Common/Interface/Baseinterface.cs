using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Common.Interface
{
    /// <summary>
    /// 公用约束
    /// </summary>
    public interface Baseinterface<T>
    {

        Task<IEnumerable<T>> EnumerableListAsync();

        Task<IQueryable<T>> IQueryableListAsync();

        Task<T> GetAsync(string id);


        Task<bool> PutEntityAsync(string id, T entity);

        Task<bool> AddEntityAsync(T entity);

       
        Task<bool> DeleteAsync(string id);
      
        bool IsExists(string id);

    }
}
