using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingApi.Dto.Request;
using ShoppingApi.Dto.Response;
using ShoppingApi.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapiCore;
using ZapiCore.Layui;

namespace ShoppingApi.Controllers
{

    /// <summary>
    /// 商品 API
    /// </summary>
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {


        private readonly ILogger<ProductController> _logger;
        private readonly ProdoctManager _prodoctManager;
        public ProductController(ILogger<ProductController> logger, ProdoctManager prodoctManager)
        {

            _logger = logger;
            _prodoctManager = prodoctManager;
        }





        /// <summary>
        /// 兼容Layui表格数据结构得商品列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("layui/table/list")]
        public async Task<LayerTableJson> ProductList([FromBody]LayuiTableRequest search)
        {
            var response = new LayerTableJson() { };
            try
            {
                response = await _prodoctManager.LayuiProductListAsync(search, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Msg = "商品列表查询失败，请重试";
                _logger.LogInformation($"商品列表查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }


        /// <summary>
        /// 商品详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail")]
        public async Task<ResponseMessage<ProductListResponse>> ProductDetails(string id)
        {
            var response = new ResponseMessage<ProductListResponse>() { Extension = new ProductListResponse { } };
            try
            {
                response = await _prodoctManager.ProductDetailsAsync(id, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "商品详情查询失败，请重试";
                _logger.LogInformation($"商品详情查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
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
                response = await _prodoctManager.ProductListAsync(search, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "商品列表查询失败，请重试";
                _logger.LogInformation($"商品列表查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }



        /// <summary>
        /// 增加/修改商品(传入Id如果不存在直接新增否则直接修改)        
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<ResponseMessage<bool>> ProductEdit([FromBody]ProductEditRequest request)
        {
            var response = new ResponseMessage<bool>() { Extension = false };

            if (!(request.Files.Count(y => y.IsIcon) >= 0))
            {
                throw new ZCustomizeException(ResponseCodeEnum.ModelStateInvalid, "请设置一张封面图");
            }
            try
            {
                if (await _prodoctManager.IsExists(request.Id) || string.IsNullOrWhiteSpace(request.Id))
                {
                    response = await _prodoctManager.ProductAddAsync(request, HttpContext.RequestAborted);
                }
                else
                {
                    response = await _prodoctManager.ProductUpdateAsync(request, HttpContext.RequestAborted);
                }
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "编辑商品失败，请重试";
                _logger.LogInformation($"编辑商品失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }


        /// <summary>
        /// 删除商品
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<ResponseMessage<bool>> ProductDelete(string id)
        {
            var response = new ResponseMessage<bool> { Extension = false };
            try
            {
                response = await _prodoctManager.ProductDeleteAsync(id);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "删除商品失败，请重试";
                _logger.LogInformation($"删除商品失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }



    }
}
