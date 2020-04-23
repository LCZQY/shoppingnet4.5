//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using ShoppingApi.Common;
//using ShoppingApi.Dto.Request;
//using ShoppingApi.Dto.Response;
//using ShoppingApi.Managers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ZapiCore;

//namespace ShoppingApi.Controllers
//{

//    /// <summary>
//    /// 订单 API
//    /// </summary>
//    [Route("api/order")]
//    [ApiController]
//    public class OrdersController : ControllerBase
//    {


//        private readonly ILogger<OrdersController> _logger;
//        private readonly OrdersManager _ordersManager;

//        public OrdersController(ILogger<OrdersController> logger, OrdersManager  ordersManager)
//        {

//            _logger = logger;
//            _ordersManager = ordersManager;
//        }


//        /// <summary>
//        /// 订单详情
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet("detail")]
//        public async Task<ResponseMessage<OrdersListResponse>> OrdersDetails(string id)
//        {
//            var response = new ResponseMessage<OrdersListResponse>() { Extension = new OrdersListResponse { } };
//            try
//            {
//                response = await _ordersManager.OrdersDetailsAsync(id, HttpContext.RequestAborted);
//            }
//            catch (Exception e)
//            {
//                response.Code = ResponseCodeDefines.ServiceError;
//                response.Message = "订单详情查询失败，请重试";
//                _logger.LogInformation($"订单详情查询失败异常:{JsonHelper.ToJson(e)}");
//            }
//            return response;
//        }

//        /// <summary>
//        /// 订单列表
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("list")]
//        public async Task<PagingResponseMessage<OrdersListResponse>> OrdersList([FromBody]SearchOrdersRequest search)
//        {
//            var response = new PagingResponseMessage<OrdersListResponse>() { Extension = new List<OrdersListResponse> { } };
//            try
//            {
//                response = await _ordersManager.OrdersListAsync(search, HttpContext.RequestAborted);
//            }
//            catch (Exception e)
//            {
//                response.Code = ResponseCodeDefines.ServiceError;
//                response.Message = "订单列表查询失败，请重试";
//                _logger.LogInformation($"订单列表查询失败异常:{JsonHelper.ToJson(e)}");
//            }
//            return response;
//        }



//        /// <summary>
//        /// 增加/修改订单(传入Id如果不存在直接新增否则直接修改)        
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("edit")]
//        public async Task<ResponseMessage<bool>> OrdersEdit([FromBody]OrdersEditRequest request)
//        {
//            var response = new ResponseMessage<bool>() { Extension = false };

//            if (!(request.Files.Count(y => y.IsIcon) >= 0))
//            {
//                throw new ZCustomizeException(ResponseCodeEnum.ModelStateInvalid, "请设置一张封面图");
//            }
//            try
//            {
//                if (await _ordersManager.IsExists(request.Id) || string.IsNullOrWhiteSpace(request.Id))
//                {
//                    response = await _ordersManager.OrdersAddAsync(request, HttpContext.RequestAborted);
//                }
//                else
//                {
//                    response = await _ordersManager.OrdersUpdateAsync(request, HttpContext.RequestAborted);
//                }
//            }
//            catch (Exception e)
//            {
//                response.Code = ResponseCodeDefines.ServiceError;
//                response.Message = "编辑订单失败，请重试";
//                _logger.LogInformation($"编辑订单失败异常:{JsonHelper.ToJson(e)}");
//            }
//            return response;
//        }


//        /// <summary>
//        /// 删除订单
//        /// </summary>
//        /// <returns></returns>
//        [HttpDelete("delete")]
//        public async Task<ResponseMessage<bool>> OrdersDelete(string id)
//        {
//            var response = new ResponseMessage<bool> { Extension = false };
//            try
//            {
//                response = await _ordersManager.OrdersDeleteAsync(id);
//            }
//            catch (Exception e)
//            {
//                response.Code = ResponseCodeDefines.ServiceError;
//                response.Message = "删除订单失败，请重试";
//                _logger.LogInformation($"删除订单失败异常:{JsonHelper.ToJson(e)}");
//            }
//            return response;
//        }



//    }
//}
