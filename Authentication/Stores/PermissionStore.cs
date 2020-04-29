using Authentication.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Stores
{
    public class PermissionitemStore : IPermissionStore
    {
        private readonly AuthenticationDbContext _context;

        public PermissionitemStore(AuthenticationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntityAsync(Permissionitem entity)
        {
            _context.Attach(entity);
            _context.Permissionitem.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }



        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExists(string id)
        {
            return _context.Permissionitem.AsNoTracking().Any(e => e.Id == id);
        }

        /// <summary>
        /// 单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Permissionitem> GetAsync(string id)
        {
            var Permissionitem = await _context.Permissionitem.FindAsync(id);

            if (Permissionitem == null)
            {
                return null;
            }
            return Permissionitem;
        }

        /// <summary>
        /// 列表数据 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Permissionitem>> EnumerableListAsync()
        {
            return await _context.Permissionitem.AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// 列表数据 
        /// </summary>
        /// <returns></returns>
        public IQueryable<Permissionitem> IQueryableListAsync()
        {
            return _context.Permissionitem;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> PutEntityAsync(string id, Permissionitem entity)
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
            var model = await _context.Permissionitem.FindAsync(id);          
            _context.Attach(model);
            _context.Remove(model);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="listentity"></param>
        /// <returns></returns>
        public async Task<bool> AddRangeEntityAsync(List<Permissionitem> listentity)
        {
            _context.AttachRange(listentity);
            await _context.AddRangeAsync(listentity);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRangeAsync(List<string> id)
        {         
            var list =await IQueryableListAsync().Where(item => id.Contains(item.Id)).ToListAsync();
            _context.AttachRange(list);
            _context.RemoveRange(list);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
