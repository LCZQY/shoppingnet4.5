using Authentication.Dto.Request;
using Authentication.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ZapiCore;

namespace Authentication.Controllers
{

    /// <summary>
    /// 角色 API
    /// </summary>
    [Route("api/role")]
    [ApiController]
    public class RoleController : Controller
    {

        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager _roleManager;
        public RoleController(ILogger<RoleController> logger, RoleManager roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }



        /// <summary>
        /// 角色绑定权限
        /// </summary>
        /// <returns></returns>
        [HttpPost("permission/add")]
        public async Task<ResponseMessage<bool>> BindPermission([FromBody]RoleAndPermissionRequest request)
        {
            return await _roleManager.BindPermissionAsync(request, HttpContext.RequestAborted);            
        }



        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ResponseMessage<dynamic>> RoleList([FromBody]SearchRoleRequest search)
        {
            var response = new ResponseMessage<dynamic>() { };
            try
            {
                response = await _roleManager.RoleListAsync(search, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "角色列表查询失败，请重试";
                _logger.LogInformation($"角色列表查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }



        /// <summary>
        /// 增加/修改角色(传入Id如果不存在直接新增否则直接修改)        
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<ResponseMessage<bool>> RoleEdit([FromBody]RoleEditRequest request)
        {
            var response = new ResponseMessage<bool>() { Extension = false };

            try
            {
                if (await _roleManager.IsExists(request.Id) || string.IsNullOrWhiteSpace(request.Id))
                {
                    response = await _roleManager.RoleAddAsync(request);
                }
                else
                {
                    response = await _roleManager.RoleUpdateAsync(request);
                }
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "编辑角色失败，请重试";
                _logger.LogInformation($"编辑角色失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<ResponseMessage<bool>> RoleDelete(string id)
        {
            var response = new ResponseMessage<bool> { Extension = false };
            try
            {
                response = await _roleManager.RoleDeleteAsync(id);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "删除角色失败，请重试";
                _logger.LogInformation($"删除角色失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }



    }
}