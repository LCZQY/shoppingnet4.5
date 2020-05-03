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
    /// 用户逻辑处理
    /// </summary>
    public class UserManager
    {
        private readonly ITransaction<AuthenticationDbContext> _transaction;
        private readonly IMapper _mapper;
        private readonly IUserStore _userStore;
        private readonly ILogger<UserManager> _logger;
        private readonly IUserRoleStore _userRoleStore;
        private readonly IRoleStore _roleStore;
        public UserManager(IUserStore userStore, ILogger<UserManager> logger, IMapper mapper, ITransaction<AuthenticationDbContext> transaction, IRoleStore roleStore, IUserRoleStore userRoleStore)


        {
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
            if (await _userStore.GetAsync(userid) == null) {
                throw new ZCustomizeException(ResponseCodeEnum.NotAllow,"没有找到该用户，请重试");
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
                    if (request.RoleId.Count != await oldUserRole.CountAsync())
                    {
                        var oldid = await oldUserRole.Select(y => y.Id).ToListAsync();
                        await _userRoleStore.DeleteRangeAsync(oldid);

                    }
                    else
                    {
                        response.Code = ResponseCodeDefines.ObjectAlreadyExists;
                        response.Extension = false;
                        response.Message = "该用户还是原来的角色";
                        return response;
                    }
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
                    await _userRoleStore.AddRangeEntityAsync(userAndrole);
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
           // var entity =  _userStore.IQueryableListAsync();
            var entity = from user in _userStore.IQueryableListAsync()
                         join user_role in _userRoleStore.IQueryableListAsync()
                         on user.Id equals user_role.UserId 
                         // from s1 in s.DefaultIfEmpty()
                         join role in _roleStore.IQueryableListAsync()
                         on user_role.RoleId equals role.Id 
                         //from t1 in t.DefaultIfEmpty()
                         select new
                         {
                             UserId = user.Id,
                             user.TrueName,
                             user.UserName,

                             //RoleName = !(t.Select(y => y.Name).Any()) ? null : string.Join(',',t.Select(y=>y.Name).ToList()??null),
                             user.PhoneNumber,
                           
                             RoleName = role.Name
                         };
            entity = entity.GroupBy(y => y.UserId).Select(item => new
            {
                UserId = item.Key,
                item.FirstOrDefault().TrueName,
                item.FirstOrDefault().UserName,

                item.FirstOrDefault().PhoneNumber,
                //   RoleId = role.Id,
                RoleName = string.Join(",", item.Select(y => y.RoleName).ToArray())
            });

            //  _logger.LogInformation($"用户信息：{JsonHelper.ToJson(entity)}");
            if (!string.IsNullOrWhiteSpace(search.UserName))
            {
                entity = entity.Where(y => y.UserName.Contains(search.UserName));
            }
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
            response.Count = await entity.CountAsync(cancellationToken);
            var list = await entity.Skip(((search.Page ?? 0) - 1) * search.Limit ?? 0).Take(search.Limit ?? 0).ToListAsync(cancellationToken);
            var result = _mapper.Map<List<UserListResponse>>(list);
          
            response.Data = result;
            return response;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> UserAddAsync(UserEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (editRequest == null)
            {
                throw new ArgumentNullException();
            }
            var User = _mapper.Map<User>(editRequest);
            response.Extension = await _userStore.AddEntityAsync(User);
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
            var User = _mapper.Map<User>(editRequest);
            if (await _userStore.PutEntityAsync(User.Id, User))
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
