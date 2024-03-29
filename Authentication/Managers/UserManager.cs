﻿using Authentication.Dto.Request;
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
    /// 用户逻辑处理
    /// </summary>
    public class UserManager
    {
        private readonly ITransaction<AuthenticationDbContext> _transaction;
        private readonly IMapper _mapper;
        private readonly IUserStore _userStore;
        private readonly ILogger<UserManager> _logger;
        private readonly IUserRoleStore _userRoleStore;
        private readonly IRolePermissionStore _rolePermissionStore;
        private readonly IPermissionStore _permissionStore;
        private readonly IRoleStore _roleStore;



        public UserManager(IUserStore userStore, ILogger<UserManager> logger, IMapper mapper, ITransaction<AuthenticationDbContext> transaction, IRoleStore roleStore, IUserRoleStore userRoleStore, IRolePermissionStore rolePermissionStore, IPermissionStore permissionStore)


        {
            _rolePermissionStore = rolePermissionStore;
            _permissionStore = permissionStore;
            _transaction = transaction;
            _roleStore = roleStore;
            _userRoleStore = userRoleStore;
            _userStore = userStore;
            _logger = logger;
            _mapper = mapper;
        }







        /// <summary>
        /// 查询该用户拥有哪些角色
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<List<RoleListResponse>>> SelectUserRoleAsync(string userid, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = new ResponseMessage<List<RoleListResponse>>() { Extension = new List<RoleListResponse> { } };
            if (await _userStore.GetAsync(userid) == null)
            {
                throw new ZCustomizeException(ResponseCodeEnum.NotAllow, "没有找到该用户，请重试");
            }
            var roleid = await _userRoleStore.IQueryableListAsync().Where(t => t.UserId == userid).Select(y => y.RoleId).ToListAsync(cancellationToken);
            _logger.LogInformation($"roleid:{JsonHelper.ToJson(roleid)}");
            var result = await _roleStore.IQueryableListAsync().Select(role => new RoleListResponse
            {
                Id = role.Id,
                Name = role.Name,
                IsAuthorize = roleid.Contains(role.Id) ? true : false
            }).ToListAsync();
            response.Extension = result;
            return response;
        }

        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> BindUserRoleAsync(UserAndroleRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            using (var transaction = await _transaction.BeginTransaction())
            {
                try
                {
                    var oldUserRole = _userRoleStore.IQueryableListAsync().Where(item => item.UserId == request.UserId);

                    //新增用户角色和原来的不一致时，就直接删除，在重新添加
                    if (request.RoleId.Count != await oldUserRole.CountAsync(cancellationToken))
                    {
                        var oldid = await oldUserRole.Select(y => y.Id).ToListAsync(cancellationToken);
                        await _userRoleStore.DeleteRangeAsync(oldid);
                        var permissionExpansions = await _permissionStore.Permissionitem_Expansions().Where(u => u.UserId == request.UserId).ToListAsync(cancellationToken);
                        if (permissionExpansions.Any())
                            await _permissionStore.DeleteRangeAsync(permissionExpansions); //删除权限扩展表数据
                    }
                    else
                    {
                        response.Code = ResponseCodeDefines.ObjectAlreadyExists;
                        response.Extension = false;
                        response.Message = "该用户还是原来的角色";
                        return response;
                    }
                    if (request.RoleId.Any())
                    {
                        var userAndrole = new List<User_Role>() { };
                        foreach (var item in request.RoleId)
                        {
                            userAndrole.Add(new User_Role
                            {
                                CreateTime = DateTime.Now,
                                Id = Guid.NewGuid().ToString(),
                                RoleId = item,
                                UserId = request.UserId,
                            });
                        }
                        var info = await _rolePermissionStore.IQueryableListAsync().Where(c => request.RoleId.Contains(c.RoleId)).Select(u => u.PermissionId).ToListAsync();
                        if (info.Any())
                        {
                            var perExpansions = await _permissionStore.IQueryableListAsync().Where(j => info.Contains(j.Id)).Select(b => new Permissionitem_expansion
                            {
                                UserId = request.UserId,
                                CreateTime = DateTime.Now,
                                Id = Guid.NewGuid().ToString(),
                                PermissionCode = b.Code,
                            }).ToListAsync();
                            await _permissionStore.AddRangeAsync(perExpansions); // 新增权限扩展表数据            
                        }
                        await _userRoleStore.AddRangeEntityAsync(userAndrole);
                    }
                    await transaction.CommitAsync();
                    response.Extension = true;
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError($"绑定用户角色失败,错误日志:{JsonHelper.ToJson(e.ToString())}");
                    throw new ZCustomizeException(ResponseCodeEnum.ServiceError, "绑定用户角色失败，请重试");
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
        public async Task<LayerTableJson> LayuiTableListAsync(SearchUserRequest search, CancellationToken cancellationToken)
        {
            var response = new LayerTableJson();
            var entity = _userStore.IQueryableListAsync().Where(y => !y.IsDeleted);
            if (!string.IsNullOrWhiteSpace(search.RoleName))
            {
                // entity = entity.Where(y=>y.listRole.Contains(search.RoleName));            
            }
            if (!string.IsNullOrWhiteSpace(search.PhoneNumber))
            {
                entity = entity.Where(y => y.PhoneNumber.Contains(search.PhoneNumber));
            }
            if (!string.IsNullOrWhiteSpace(search.TrueName))
            {
                entity = entity.Where(y => y.TrueName.Contains(search.TrueName));
            }
            if (!string.IsNullOrWhiteSpace(search.UserName))
            {
                entity = entity.Where(y => y.UserName.Contains(search.UserName));
            }           
            response.Count = await entity.CountAsync(y=>y.Id != null,cancellationToken);
            var list = await entity.Skip(((search.Page ?? 0) - 1) * search.Limit ?? 0).Take(search.Limit ?? 0).ToListAsync(cancellationToken);
            var result = _mapper.Map<List<UserListResponse>>(list);

            result.ForEach(y =>
           {
               // 改成同步方法才不会报错
               y.RoleName = GetRoleName(y.Id);
           });
            response.Data = result;
            return response;
        }

        /// <summary>
        /// 获取用户的所有角色使用逗号隔开
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetRoleName(string userid)
        {
            var user_role = _userRoleStore.IQueryableListAsync().Where(y => y.UserId == userid).Select(y => y.RoleId).ToList();
            var roleinfo = _roleStore.IQueryableListAsync().Where(y => user_role.Contains(y.Id)).Select(y => y.Name).ToList();
            return string.Join(",", roleinfo);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> UserAddAsync(UserEditRequest editRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (editRequest == null)
            {
                throw new ArgumentNullException();
            }
            if (await _userStore.IQueryableListAsync().Where(y => y.UserName == editRequest.UserName).AnyAsync(cancellationToken))
            {
                throw new ZCustomizeException(ResponseCodeEnum.ObjectAlreadyExists, "该用户名已存在请重试");
            }
            var user = _mapper.Map<User>(editRequest);
            user.CreateTime = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            user.Password = "123456";
            response.Extension = await _userStore.AddEntityAsync(user);
            return response;
        }



        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExists(string id)
        {
            if (_userStore.IsExists(id))
                return true;
            return false;
        }






        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<UserListResponse>> UserDetailsAsync(string id, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<UserListResponse>() { Extension = new UserListResponse { } };
            var entity = await _userStore.GetAsync(id);


            var data = _mapper.Map<UserListResponse>(entity);
            response.Extension = data;
            return response;
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> UserUpdateAsync(UserEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            var user = _mapper.Map<User>(editRequest);
            user.UpdateTime = DateTime.Now;
            if (await _userStore.PutEntityAsync(user.Id, user))
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
        public async Task<ResponseMessage<bool>> UserDeleteAsync(string id)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (await _userStore.DeleteAsync(id))
            {
                response.Extension = true;
            }
            return response;
        }


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="Users"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> AddRanageAsync(List<UserEditRequest> Users, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (Users.Count == 0)
            {
                throw new ArgumentNullException();
            }
            var list = _mapper.Map<List<User>>(Users);
            response.Extension = await _userStore.AddRangeEntityAsync(list);
            return response;
        }
    }
}
