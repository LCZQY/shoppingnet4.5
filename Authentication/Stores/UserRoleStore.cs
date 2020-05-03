using Authentication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Stores
{
    public class UserRoleStore : IUserRoleStore
    {
        private readonly AuthenticationDbContext _context;

        public UserRoleStore(AuthenticationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntityAsync(User_Role entity)
        {
            _context.Attach(entity);
            _context.User_Role.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }



        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExists(string id)
        {
            return _context.User_Role.AsNoTracking().Any(e => e.Id == id);
        }

        /// <summary>
        /// 单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User_Role> GetAsync(string id)
        {
            var User_Role = await _context.User_Role.FindAsync(id);

            if (User_Role == null)
            {
                return null;
            }
            return User_Role;
        }

        /// <summary>
        /// 列表数据 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User_Role>> EnumerableListAsync()
        {
            return await _context.User_Role.AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// 列表数据 
        /// </summary>
        /// <returns></returns>
        public IQueryable<User_Role> IQueryableListAsync()
        {
            return _context.User_Role.AsNoTracking();

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> PutEntityAsync(string id, User_Role entity)
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
            var model = await _context.User_Role.FindAsync(id);
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
        public async Task<bool> AddRangeEntityAsync(List<User_Role> listentity)
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
            var list = await IQueryableListAsync().Where(item => id.Contains(item.Id)).ToListAsync();
            _context.AttachRange(list);
            _context.RemoveRange(list); 
            return await _context.SaveChangesAsync() > 0;
        }

        
    }
}
