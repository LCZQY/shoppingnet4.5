﻿using Authentication.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Stores
{
    /// <summary>
    /// 角色权限数据处理
    /// </summary>
    public class RolePermissionStore : IRolePermissionStore
    {
        private readonly AuthenticationDbContext _context;

        public RolePermissionStore(AuthenticationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntityAsync(Role_Permissionitem entity)
        {
            _context.Attach(entity);
            _context.Role_Permissionitem.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }



        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExists(string id)
        {
            return _context.Role_Permissionitem.AsNoTracking().Any(e => e.Id == id);
        }

        /// <summary>
        /// 单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Role_Permissionitem> GetAsync(string id)
        {
            var Role_Permissionitem = await _context.Role_Permissionitem.FindAsync(id);

            if (Role_Permissionitem == null)
            {
                return null;
            }
            return Role_Permissionitem;
        }

        /// <summary>
        /// 列表数据 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Role_Permissionitem>> EnumerableListAsync()
        {
            return await _context.Role_Permissionitem.AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// 列表数据 
        /// </summary>
        /// <returns></returns>
        public IQueryable<Role_Permissionitem> IQueryableListAsync()
        {
            return _context.Role_Permissionitem;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> PutEntityAsync(string id, Role_Permissionitem entity)
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
            var model = await _context.Role_Permissionitem.FindAsync(id);          
            _context.Attach(model);
            _context.Remove(model);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="listentity"></param>
        /// <returns></returns>
        public async Task<bool> AddRangeEntityAsync(List<Role_Permissionitem> listentity)
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

        /// <summary>
        /// 权限扩展表
        /// </summary>
        /// <returns></returns>
        public IQueryable<Role_Permissionitem> Role_Permissionitem_Expansions()
        {
            return _context.Role_Permissionitem;
        }

        /// <summary>
        /// 批量新增角色权限表
        /// </summary>
        /// <param name="role_Role_Permissionitems"></param>
        /// <returns></returns>
        public async Task<bool> AddRangeRolePermission(List<Role_Permissionitem> role_Role_Permissionitems)
        {
            _context.AttachRange(role_Role_Permissionitems);
            await _context.AddRangeAsync(role_Role_Permissionitems);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
