using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingApi.Managers;
using ShoppingApi.Models;
using System;
using System.Threading.Tasks;
using ZapiCore;
using ShoppingApi.Dto.Request;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingApi.Controllers
{

    /// <summary>
    /// 客户API
    /// </summary>
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ShoppingDbContext _context;
        private readonly ILogger<CustomersController> _logger;
        private readonly CustomerManager _customerManager;
        public CustomersController(ShoppingDbContext context, ILogger<CustomersController> logger, CustomerManager customerManager)
        {
            _context = context;
            _logger = logger;
            _customerManager = customerManager;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [Authorize]        
        [HttpGet("get")]
        public string get()
        {
            return "get";
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
            var response = new PagingResponseMessage<Customer>(){Extension  = new List<Customer> { } };
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

    }
}
