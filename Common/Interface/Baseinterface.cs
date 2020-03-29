using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interface
{
    /// <summary>
    /// 公用约束
    /// </summary>
    public interface Baseinterface<T>
    {

        Task<IEnumerable<T>> EnumerableList();

        Task<IQueryable<T>> IQueryableList();

        Task<T> Get(string id);


        Task<bool> PutEntity(string id, T entity);

        Task<bool> AddEntity(T entity);

       
        Task<bool> Delete(string id);
      
        bool IsExists(string id);

    }
}
