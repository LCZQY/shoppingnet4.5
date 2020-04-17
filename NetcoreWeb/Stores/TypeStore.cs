using Microsoft.EntityFrameworkCore;
using ShoppingApi.Models;
using ShoppingApi.Stores.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ShoppingApi.Stores
{
    /// <summary>
    /// 商品类型表数据处理
    /// </summary>
    public class TypeStore : ITypeStore
    {
        private readonly ShoppingDbContext _context;

        public TypeStore(ShoppingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntityAsync(Category entity)
        {
            _context.Attach(entity);
            _context.Category.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }



        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExists(string id)
        {
            return _context.Category.AsNoTracking().Any(e => e.Id == id);
        }

        /// <summary>
        /// 单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetAsync(string id)
        {
            var Category = await _context.Category.FindAsync(id);

            if (Category == null)
            {
                return null;
            }
            return Category;
        }

        /// <summary>
        /// 列表数据 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> EnumerableListAsync()
        {
            return await _context.Category.AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// 列表数据 
        /// </summary>
        /// <returns></returns>
        public IQueryable<Category>  IQueryableListAsync()
        {
            return _context.Category.AsNoTracking();

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> PutEntityAsync(string id, Category entity)
        {
            if (!IsExists(id))
            {
                return false;
            }

            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;

        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            if (!IsExists(id))
            {
                return false;
            }
            var model = await _context.Category.FindAsync(id);
            model.IsDeleted = true;
            _context.Attach(model);
            var entity = _context.Entry(model);
            entity.Property(y => y.IsDeleted).IsModified = true;
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="listentity"></param>
        /// <returns></returns>
        public async Task<bool> AddRangeEntityAsync(List<Category> listentity)
        {
            _context.AttachRange(listentity);
            await _context.AddRangeAsync(listentity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
