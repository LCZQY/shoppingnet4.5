using Authentication.Dto.Request;
using Authentication.Models;
using Authentication.Stores;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZapiCore;

namespace Authentication.Managers
{

    /// <summary>
    /// 角色 逻辑处理
    /// </summary>
    public class RoleManager
    {
        private readonly IMapper _mapper;
        private readonly IRoleStore _roleStore;
        private readonly IRolePermissionStore  _rolePermissionStore;
        private readonly ITransaction<AuthenticationDbContext> _transaction;
        private readonly ILogger<RoleManager> _logger;
        public RoleManager(IRoleStore roleStore, ILogger<RoleManager> logger, IMapper mapper, IRolePermissionStore rolePermissionStore, ITransaction<AuthenticationDbContext> transaction)
        {
            _roleStore = roleStore;
            _rolePermissionStore = rolePermissionStore;
            _transaction = transaction;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 角色权限绑定与移除
        /// </summary>
        /// <param name="condtion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> BindPermissionAsync(RoleAndPermissionRequest condtion, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (condtion is null)
            {
                response.Extension = false;
            }
            using (var transaction = await _transaction.BeginTransaction())
            {
                try
                {
                    var roleperssion = new List<Role_Permissionitem>() { };
                    //原来的角色所有权限
                    var oldRole_Permission = _rolePermissionStore.IQueryableListAsync().Where(y => y.RoleId == condtion.RoleId);
                    if (await oldRole_Permission.AnyAsync(cancellationToken))
                    {
                        //直接清空该角色在权限数据 ， 再新增提交保存权限
                        //var oldPermission = await oldRole_Permission.Select(y => y.PermissionId).Distinct().ToListAsync(); // 1 2 3 
                        //var newPermission = condtion.ListPermissionId; // 1                
                        var rolePermisid = await oldRole_Permission.Select(y => y.Id).ToListAsync(cancellationToken);
                        await _rolePermissionStore.DeleteRangeAsync(rolePermisid);                      
                    }
                    foreach (var pid in condtion.ListPermissionId)
                    {
                        roleperssion.Add(new Role_Permissionitem
                        {
                            Id = Guid.NewGuid().ToString(),
                            PermissionId = pid,
                            CreateTime = DateTime.Now,
                            RoleId = condtion.RoleId,
                        });
                    }
                    response.Extension = await _rolePermissionStore.AddRangeEntityAsync(roleperssion);
                    await transaction.CommitAsync(cancellationToken);
                }catch(Exception e)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    response.Code = ResponseCodeDefines.ServiceError;
                    response.Message = "绑定角色权限失败，请重试";

                }
            }
            return response;
        }




        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<dynamic>> RoleListAsync(SearchRoleRequest search, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<dynamic>() { };
            var entity = _roleStore.IQueryableListAsync();
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                entity = entity.Where(y => y.Name == search.Name);
            }
            var list = await entity.Skip(search.PageIndex * search.PageSize).Take(search.PageSize).ToListAsync(cancellationToken);
            response.Extension = new { list, search.PageIndex, search.PageSize };
            return response;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> RoleAddAsync(RoleEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (editRequest == null)
            {
                throw new ArgumentNullException();
            }
            var role = _mapper.Map<Role>(editRequest);
            response.Extension = await _roleStore.AddEntityAsync(role);
            return response;
        }



        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExists(string id)
        {
            if (_roleStore.IsExists(id))
                return true;
            return false;
        }



        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> RoleUpdateAsync(RoleEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            var Role = _mapper.Map<Role>(editRequest);
            if (await _roleStore.PutEntityAsync(Role.Id, Role))
            {
                response.Extension = true;
            }
            return response;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> RoleDeleteAsync(string id)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (await _roleStore.DeleteAsync(id))
            {
                response.Extension = true;
            }
            return response;
        }


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> AddRanageAsync(List<RoleEditRequest> permissions, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (permissions.Count == 0)
            {
                throw new ArgumentNullException();
            }
            var list = _mapper.Map<List<Role>>(permissions);
            response.Extension = await _roleStore.AddRangeEntityAsync(list);
            return response;
        }
    }
}
