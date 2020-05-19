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
    /// 用户 API
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager _userManager;
        public UserController(ILogger<UserController> logger, UserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        /// <summary>
        /// 查询该用户拥有哪些角色
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("get/role")]
        public async Task<ResponseMessage<List<RoleListResponse>>> BindUserRole(string userid)
        {

            return await _userManager.SelectUserRoleAsync(userid);
        }


        /// <summary>
        /// 绑定用户角色关系
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>                
        [HttpPost("role/add")]
        public async Task<ResponseMessage<bool>> BindUserRole([FromBody] UserAndroleRequest request)
        {
            return await _userManager.BindUserRoleAsync(request);
        }



        /// <summary>
        /// 用户详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail")]
        public async Task<ResponseMessage<UserListResponse>> UserDetails(string id)
        {
            var response = new ResponseMessage<UserListResponse>() { Extension = new UserListResponse { } };
            try
            {
                response = await _userManager.UserDetailsAsync(id, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "用户详情查询失败，请重试";
                _logger.LogInformation($"用户详情查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }

        ///// <summary>
        ///// 用户列表
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("layui/table/mycat")]
        //public async Task<LayerTableJson> LayuiTableList()
        //{
        //    var response = new LayerTableJson();
          
        //    try
        //    {
        //        response = await _userManager.LayuiTableListAsync(search, HttpContext.RequestAborted);
        //    }
        //    catch (Exception e)
        //    {
        //        response.Code = 500;
        //        response.Msg = "用户列表查询失败，请重试";
        //        _logger.LogInformation($"用户列表查询失败异常:{JsonHelper.ToJson(e)}");
        //    }
        //    return response;
        //}





        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("layui/table/list")]
        public async Task<LayerTableJson> LayuiTableList([FromBody]SearchUserRequest search)
        {
            var response = new LayerTableJson();
            if (search.Page == 0)
            {
                throw new ZCustomizeException(ResponseCodeEnum.ModelStateInvalid, "本接口仅支持页数从1开始");
            }
            try
            {
                response = await _userManager.LayuiTableListAsync(search, HttpContext.RequestAborted);
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
        /// 增加/修改用户(传入Id如果不存在直接新增否则直接修改)        
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<ResponseMessage<bool>> UserEdit([FromBody]UserEditRequest request)
        {
            var response = new ResponseMessage<bool>() { Extension = false };

            if (!(await _userManager.IsExists(request.Id)) || string.IsNullOrWhiteSpace(request.Id))
            {
                response = await _userManager.UserAddAsync(request);
            }
            else
            {
                response = await _userManager.UserUpdateAsync(request);
            }
            return response;
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<ResponseMessage<bool>> UserDelete(string id)
        {
            var response = new ResponseMessage<bool> { Extension = false };
            try
            {
                response = await _userManager.UserDeleteAsync(id);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "删除用户失败，请重试";
                _logger.LogInformation($"删除用户失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }




    }
}