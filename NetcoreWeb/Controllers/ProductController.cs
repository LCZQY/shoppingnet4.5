﻿using Microsoft.AspNetCore.Mvc;
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
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {

   
        private readonly ILogger<ProductController> _logger;
        private readonly ProdoctManager _prodoctManager;
        public ProductController(ILogger<ProductController> logger, ProdoctManager prodoctManager)
        {
   
            _logger = logger;
            _prodoctManager =  prodoctManager;
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
            try
            {
                if (await _prodoctManager.IsExists(request.Id) || string.IsNullOrWhiteSpace(request.Id))
                {
                    response = await _prodoctManager.ProductAddAsync(request);
                }
                else
                {
                    response = await _prodoctManager.ProductUpdateAsync(request);
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
        public async Task<ResponseMessage<bool>> ProductDelete([FromRoute]string id)
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