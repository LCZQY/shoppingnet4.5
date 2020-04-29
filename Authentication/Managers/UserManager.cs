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

namespace Authentication.Managers
{
    /// <summary>
    /// 权限 逻辑处理
    /// </summary>
    public class UserManager
    {

        private readonly IMapper _mapper;
        private readonly IUserStore  _userStore;
        private readonly ILogger<UserManager> _logger;
        public UserManager(IUserStore userStore, ILogger<UserManager> logger, IMapper mapper)
        {
            _userStore = userStore;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<dynamic>> UserListAsync(SearchUserRequest search, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<dynamic>() { };
            var entity = _userStore.IQueryableListAsync();
            if (!string.IsNullOrWhiteSpace(search.UserName))
            {

            }
            if (!string.IsNullOrWhiteSpace(search.RoleName))
            {

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
            var list = await entity.Skip(search.PageIndex * search.PageSize).Take(search.PageSize).ToListAsync(cancellationToken);

            response.Extension = new { list, search.PageIndex, search.PageSize };
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
