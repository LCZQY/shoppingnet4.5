using Authentication.Dto.Request;
using Authentication.Dto.Response;
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
using ZapiCore.Layui;

namespace Authentication.Managers
{

    /// <summary>
    /// 角色 逻辑处理
    /// </summary>
    public class RoleManager
    {
        private readonly IMapper _mapper;
        private readonly IRoleStore _roleStore;
        private readonly IPermissionStore _permissionStore;
        private readonly IRolePermissionStore _rolePermissionStore;
        private readonly ITransaction<AuthenticationDbContext> _transaction;
        private readonly ILogger<RoleManager> _logger;
        public RoleManager(IRoleStore roleStore, ILogger<RoleManager> logger, IMapper mapper, IRolePermissionStore rolePermissionStore, ITransaction<AuthenticationDbContext> transaction, IPermissionStore permissionStore)
        {
            _permissionStore = permissionStore;
            _roleStore = roleStore;
            _rolePermissionStore = rolePermissionStore;
            _transaction = transaction;
            _logger = logger;
            _mapper = mapper;
        }



        /// <summary>
        /// 查询该角色拥有哪些权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<List<PermissionListResponse>>> SelectRolePermissionAsync(string roleid, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = new ResponseMessage<List<PermissionListResponse>>() { Extension = new List<PermissionListResponse> { } };
            if (await _roleStore.GetAsync(roleid) == null)
            {
                throw new ZCustomizeException(ResponseCodeEnum.NotAllow, "没有找到该角色，请重试");
            }
            var rolePer = await _rolePermissionStore.IQueryableListAsync().Where(t => t.RoleId == roleid).Select(y => y.PermissionId).ToListAsync(cancellationToken);
            var result = from c in await _permissionStore.EnumerableListAsync()
                         group c by c.Group into per
                         select new PermissionListResponse
                         {
                             Group = per.Key,
                             PermissionList = per.Select(u => new PermissionListResponse.ListResponse
                             {
                                 Id = u.Id,
                                 Name = u.Name,
                                 IsAuthorize = rolePer.Contains(u.Id) ? true : false
                             }).ToList()
                         };
            response.Extension = result.ToList();
            return response;
        }


        /// <summary>
        /// 角色详情（查询所有拥有的权限）
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<dynamic>> RoleDetailsAsync(string roleid, CancellationToken cancellationToken = default(CancellationToken))

        {
            var response = new ResponseMessage<dynamic>();
            var details = from per in _permissionStore.IQueryableListAsync()
                          join role_permission in _rolePermissionStore.IQueryableListAsync()
                          on per.Id equals role_permission.PermissionId into s
                          from s1 in s.DefaultIfEmpty()
                          join role in _roleStore.IQueryableListAsync()
                          on s1.RoleId equals role.Id into t
                          from t1 in t.DefaultIfEmpty()
                          where t1.Id == roleid
                          select new
                          {
                              roleid = t1.Id,
                              rolename = t1.Name,
                              permissionid = per.Id,
                              permissionname = per.Name
                          };
            response.Extension = await details.FirstOrDefaultAsync(cancellationToken);
            return response;
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
                        var list = await oldRole_Permission.ToListAsync(cancellationToken);
                        await _rolePermissionStore.DeleteRangeAsync(list);
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
                }
                catch (Exception e)
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
        public async Task<LayerTableJson> LayuiTableListAsync(SearchRoleRequest search, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = new LayerTableJson() { };
            var entity = _roleStore.IQueryableListAsync().Where(y=> !y.IsDeleted);
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                entity = entity.Where(y => y.Name.Contains(search.Name));
            }
            response.Count = await entity.CountAsync(cancellationToken);
            var list = await entity.Skip(((search.Page ?? 0) - 1) * search.Limit ?? 0).Take(search.Limit ?? 0).ToListAsync(cancellationToken);
            var info = _mapper.Map<List<RoleListResponse>>(list);
            info.ForEach(item =>
            {
                item.AuthorizeName = GetAuthorizeName(item.Id);
            });
            response.Data = info;
            return response;
        }


        /// <summary>
        /// 获取该角色的所属权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public string GetAuthorizeName(string roleid)
        {

            var rolePer = _rolePermissionStore.IQueryableListAsync().Where(y => y.RoleId == roleid).Select(u => u.PermissionId).ToList();
            var permissions = _permissionStore.IQueryableListAsync().Where(y => rolePer.Contains(y.Id)).Select(u => u.Name).ToList();

            return string.Join(",", permissions);
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="editRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> RoleAddAsync(RoleEditRequest editRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (editRequest == null)
            {
                throw new ArgumentNullException();
            }
            var isexist = await _roleStore.IQueryableListAsync().Where(item => item.Name == editRequest.Name).FirstOrDefaultAsync(cancellationToken);
            if (isexist != null)
            {
                throw new ZCustomizeException(ResponseCodeEnum.ObjectAlreadyExists, "该角色名称已存在");
            }
            var role = _mapper.Map<Role>(editRequest);
            role.CreateTime = DateTime.Now;
            role.Id = Guid.NewGuid().ToString();
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
            Role.UpdateTime = DateTime.Now;
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
