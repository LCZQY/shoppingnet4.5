using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingApi.Common;
using ShoppingApi.Dto.Request;
using ShoppingApi.Dto.Response;
using ShoppingApi.Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZapiCore;

namespace ShoppingApi.Controllers
{

    /// <summary>
    /// 商品类型 API
    /// </summary>
    [Route("api/type")]
    [ApiController]
    [Authorize]
    public class TypeController : ControllerBase
    {


        private readonly ILogger<TypeController> _logger;
        private readonly TypeManager _typeManager;
        public TypeController(ILogger<TypeController> logger, TypeManager typeManager)
        {

            _logger = logger;
            _typeManager = typeManager;
        }


        /// <summary>
        /// 商品类型树状结构
        /// </summary>
        /// <returns></returns>
        [HttpGet("tree")]
        public async Task<ResponseMessage<List<LayerTreeJson>>> CreateTypeTree()
        {
            var response = new ResponseMessage<List<LayerTreeJson>>() { Extension = new List<LayerTreeJson>() { } };
            try
            {
                response.Extension = await _typeManager.CreateTypeTreeResponseListAsync(HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "商品类型树状结构查询失败，请重试";
                _logger.LogInformation($"商品类型树状结构查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;


        }

        /// <summary>
        /// 商品类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<PagingResponseMessage<CategoryListResponse>> TypeList([FromBody]SearchTypeRequest search)
        {
            var response = new PagingResponseMessage<CategoryListResponse>() { Extension = new List<CategoryListResponse> { } };
            try
            {
                response = await _typeManager.TypeListAsync(search, HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "商品类型列表查询失败，请重试";
                _logger.LogInformation($"商品类型列表查询失败异常:{JsonHelper.ToJson(e)}");
            }
            return response;
        }





        /// <summary>
        /// 增加/修改商品类型(传入Id如果不存在直接新增否则直接修改)        
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<ResponseMessage<bool>> TypeEdit([FromBody]CategoryEditRequest request)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            try
            {
                if (!(await _typeManager.IsExists(request.Id)) || string.IsNullOrWhiteSpace(request.Id))
                {
                    response = await _typeManager.TypeAddAsync(request);
                }
                else
                {
                    response = await _typeManager.TypeUpdateAsync(request);
                }
            }
            catch (Exception e)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "编辑商品类型失败，请重试";
                _logger.LogInformation($"编辑商品类型失败异常:{JsonHelper.ToJson(e)}");
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
                response = await _typeManager.ProductDeleteAsync(id);
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
