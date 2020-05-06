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
    /// 权限 API
    /// </summary>
    [Route("api/permissionitem")]
    [ApiController]
    public class PermissionitemController : Controller
    {
        private readonly ILogger<PermissionitemController> _logger;
        private readonly PermissionManager _permissionManager;
        public PermissionitemController(ILogger<PermissionitemController> logger, PermissionManager permissionManager)
        {
            _logger = logger;
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// 检查用户是否有权限
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="permissionid">权限id</param>
        /// <returns></returns>
        [HttpGet("check")]
        public async Task<ResponseMessage<bool>> CheckPermission(string userid, string permissionid)
        {
            return await _permissionManager.CheckPermissionAsync(userid, permissionid, HttpContext.RequestAborted);
        }

        /// <summary>
        /// 权限列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ResponseMessage<dynamic>> PermissionList([FromBody]SearchPermissionRequest search)
        {
            var response = new ResponseMessage<dynamic>() { };
            try
            {
                response = await _permissionManager.PermissionitemListAsync(search, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "权限列表查询失败，请重试";
                _logger.LogInformation($"权限列表查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }



        /// <summary>
        /// 增加/修改权限(传入Id如果不存在直接新增否则直接修改)        
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<ResponseMessage<bool>> PermissionEdit([FromBody]PermissionEditRequest request)
        {
            var response = new ResponseMessage<bool>() { Extension = false };


            try
            {
                if (await _permissionManager.IsExists(request.Id) || string.IsNullOrWhiteSpace(request.Id))
                {
                    response = await _permissionManager.PermissionitemAddAsync(request);
                }
                else
                {
                    response = await _permissionManager.PermissionitemUpdateAsync(request);
                }
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "编辑权限失败，请重试";
                _logger.LogInformation($"编辑权限失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }


        /// <summary>
        /// 删除权限
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<ResponseMessage<bool>> PermissionDelete(string id)
        {
            var response = new ResponseMessage<bool> { Extension = false };
            try
            {
                response = await _permissionManager.PermissionitemDeleteAsync(id);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "删除权限失败，请重试";
                _logger.LogInformation($"删除权限失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }



    }
}