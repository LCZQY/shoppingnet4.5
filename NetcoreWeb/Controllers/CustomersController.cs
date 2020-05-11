
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingApi.Common;
using ShoppingApi.Dto.Request;
using ShoppingApi.Managers;
using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZapiCore;

namespace ShoppingApi.Controllers
{

    /// <summary>
    /// 客户API
    /// </summary>
    [Route("api/customers")]
   // [Authorize]
    [ApiController]
    public class CustomersController : BaseController
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly CustomerManager _customerManager;
        public CustomersController(ILogger<CustomersController> logger, CustomerManager customerManager)
        {
            _logger = logger;
            _customerManager = customerManager;
        }
        /// <summary>
        /// admin权限
        /// </summary>
        /// <returns></returns> 
        [HttpGet("check")]
        public string Check()
        {

            return "测试基于角色的权限管理 superadmin";
        }

        /// <summary>
        /// 需要身份认证的方式请求接口
        /// </summary>
        /// <returns></returns>       
        [HttpPost("admin")]
        public IActionResult Token()
        {
            return new JsonResult(User);
        }



        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>       
        [HttpPost("login")]
        public async Task<ResponseMessage<bool>> LoginJudge([FromBody]LoginRequest loginRequest)
        {
            var response = new ResponseMessage<bool>();
            try
            {
                response = await _customerManager.LoginJudgeAsync(loginRequest.Name, loginRequest.Password);
            }
            catch (Exception e)
            {

                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "用户登录失败，请重试";
                _logger.LogInformation($"用户登录发生异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }

        /// <summary>
        /// 客户列表【列表数据应该是POST 还是 Get？】
        /// </summary>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<PagingResponseMessage<Customer>> CustomerList([FromBody]SearchCustomerRequest search)
        {
            var response = new PagingResponseMessage<Customer>() { Extension = new List<Customer> { } };
            try
            {
                response = await _customerManager.CustomerListAsync(search, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "客户列表查询失败，请重试";
                _logger.LogInformation($"客户列表查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }

        /// <summary>
        /// 增加/修改客户(传入Id如果不存在直接新增否则直接修改)        
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<ResponseMessage<bool>> CustomerEdit([FromBody]CustomerEditRequest request)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            try
            {
                if (await _customerManager.IsExists(request.Id) || string.IsNullOrWhiteSpace(request.Id))
                {
                    response = await _customerManager.CustomerAddAsync(request);
                }
                else
                {
                    response = await _customerManager.CustomerUpdateAsync(request);
                }
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "编辑客户失败，请重试";
                _logger.LogInformation($"编辑客户失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }


        /// <summary>
        /// 删除客户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<ResponseMessage<bool>> CustomerDelete([FromRoute]string id)
        {
            var response = new ResponseMessage<bool> { Extension = false };
            try
            {
                response = await _customerManager.CustomerDeleteAsync(id);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "删除客户失败，请重试";
                _logger.LogInformation($"删除客户查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }



    }
}
