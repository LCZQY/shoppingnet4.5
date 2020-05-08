using Authentication.Dto.Request;
using Authentication.Dto.Response;
using Authentication.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZapiCore;
using ZapiCore.Layui;

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
        /// 角色详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail")]
        public async Task<ResponseMessage<dynamic>> RoleDetails([FromBody]string  roleid)
        {
            try
            {
                return await _roleManager.RoleDetailsAsync(roleid, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"角色详情查询失败,错误信息： {JsonHelper.ToJson(e)}");
                throw new ZCustomizeException(ResponseCodeEnum.ServiceError, "角色详情查询失败，请重试");                
            }
        }



        /// <summary>
        /// 角色绑定权限
        /// </summary>
        /// <returns></returns>
        [HttpPost("permission/add")]
        public async Task<ResponseMessage<bool>> BindPermission([FromBody]RoleAndPermissionRequest request)
        {
            try
            {
                return await _roleManager.BindPermissionAsync(request, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"角色绑定权限失败,错误信息： {JsonHelper.ToJson(e)}");

                throw new ZCustomizeException(ResponseCodeEnum.ServiceError, "角色绑定权限失败，请重试");
            }
        }





        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("layui/table/list")]
        public async Task<LayerTableJson> LayuiTableList([FromBody]SearchRoleRequest search)
        {
            var response = new LayerTableJson();
            if (search.Page == 0)
            {
                throw new ZCustomizeException(ResponseCodeEnum.ModelStateInvalid, "本接口仅支持页数从1开始");
            }
            try
            {
                response = await _roleManager.LayuiTableListAsync(search, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Msg = "用户列表查询失败，请重试";
                _logger.LogInformation($"用户列表查询失败异常:{JsonHelper.ToJson(e)}");
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

            if (await _roleManager.IsExists(request.Id) || string.IsNullOrWhiteSpace(request.Id))
            {
                response = await _roleManager.RoleAddAsync(request);
            }
            else
            {
                response = await _roleManager.RoleUpdateAsync(request);
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


        /// <summary>
        /// 查询该角色拥有哪些权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [HttpGet("get/permission")]
        public async Task<ResponseMessage<List<PermissionListResponse>>> BindUserRole(string roleid)
        {
            return await _roleManager.SelectRolePermissionAsync(roleid);
        }


    }
}