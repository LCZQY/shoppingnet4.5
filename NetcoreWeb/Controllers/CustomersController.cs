using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingApi.Managers;
using ShoppingApi.Models;
using System;
using System.Threading.Tasks;
using ZapiCore;
using ShoppingApi.Dto.Request;
using System.Collections.Generic;

namespace ShoppingApi.Controllers
{

    [Route("api/[controller]")]
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
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("login")]
        public async Task<ResponseMessage<bool>> LoginJudge(string name, string pwd)
        {
            var response = new ResponseMessage<bool>();
            try
            {
                response = await _customerManager.LoginJudgeAsync(name, pwd);
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
                response = await _customerManager.CustomerListAsync(search);
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
