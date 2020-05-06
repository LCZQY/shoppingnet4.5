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
    public class PermissionManager
    {

        private readonly IMapper _mapper;
        private readonly IPermissionStore _permissionStore;

        private readonly IUserStore _userStore;
        private readonly ILogger<PermissionManager> _logger;
        public PermissionManager(IPermissionStore permissionStore, IUserStore userStore, ILogger<PermissionManager> logger, IMapper mapper)
        {
            _permissionStore = permissionStore;
            _userStore = userStore;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        ///  检查用户是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionCore"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> CheckPermissionAsync(string userId, string permissionCore, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            var user = await _userStore.GetAsync(userId);
            if (user is null) response.Extension = false;
            var permission = await _permissionStore.IQueryableListAsync().Where(y => y.Code == permissionCore).FirstOrDefaultAsync(cancellationToken);
            if (permission is null) response.Extension = false;
            var have = _permissionStore.Permissionitem_Expansions().Where(item => item.UserId == userId && permissionCore == item.PermissionCode);
            if (have.Any()) response.Extension = true;
            return response;
        }





        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<dynamic>> PermissionitemListAsync(SearchPermissionRequest search, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<dynamic>() { };
            var entity = _permissionStore.IQueryableListAsync();
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                entity = entity.Where(y => y.Name == search.Name);
            }
            if (!string.IsNullOrWhiteSpace(search.Code))
            {
                entity = entity.Where(y => y.Code == search.Code);
            }
            var list = await entity.Skip(search.PageIndex * search.PageSize).Take(search.PageSize).ToListAsync(cancellationToken);
         
            response.Extension = new {  list, search.PageIndex,search.PageSize};
            return response;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> PermissionitemAddAsync(PermissionEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (editRequest == null)
            {
                throw new ArgumentNullException();
            }
            var permissionitem = _mapper.Map<Permissionitem>(editRequest);
            permissionitem.Id = Guid.NewGuid().ToString();
            permissionitem.CreateTime = DateTime.Now;
            response.Extension = await _permissionStore.AddEntityAsync(permissionitem);
            return response;
        }



        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExists(string id)
        {
            if (_permissionStore.IsExists(id))
                return true;
            return false;
        }



        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> PermissionitemUpdateAsync(PermissionEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            var permissionitem = _mapper.Map<Permissionitem>(editRequest);
            permissionitem.UpdateTime = DateTime.Now;
            if (await _permissionStore.PutEntityAsync(permissionitem.Id, permissionitem))
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
        public async Task<ResponseMessage<bool>> PermissionitemDeleteAsync(string id)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (await _permissionStore.DeleteAsync(id))
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
        public async Task<ResponseMessage<bool>> AddRanageAsync(List<PermissionEditRequest> permissions, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (permissions.Count == 0)
            {
                throw new ArgumentNullException();
            }
            var list = _mapper.Map<List<Permissionitem>>(permissions);
            response.Extension = await _permissionStore.AddRangeEntityAsync(list);
            return response;
        }
    }
}
