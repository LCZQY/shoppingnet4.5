using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingApi.Dto.Request;
using ShoppingApi.Dto.Response;
using ShoppingApi.Managers;
using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZapiCore;

namespace ShoppingApi.Controllers
{

    /// <summary>
    /// 商品 API
    /// </summary>
    [Route("api/customers")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ShoppingDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly CustomerManager _customerManager;
        public ProductController(ShoppingDbContext context, ILogger<ProductController> logger, CustomerManager customerManager)
        {
            _context = context;
            _logger = logger;
            _customerManager = customerManager;
        }

        
        /// <summary>
        /// 商品列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<PagingResponseMessage<ProductListResponse>> ProductList([FromBody]SearchProductRequest search)
        {
            var response = new PagingResponseMessage<ProductListResponse>() { Extension = new List<ProductListResponse> { } };
            try
            {
                response = await _customerManager.ProductListAsync(search, HttpContext.RequestAborted);
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
