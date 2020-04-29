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
        private readonly ILogger<RoleManager> _logger;
        public RoleManager(IRoleStore roleStore, ILogger<RoleManager> logger, IMapper mapper)
        {
            _roleStore = roleStore;
            _logger = logger;
            _mapper = mapper;
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
