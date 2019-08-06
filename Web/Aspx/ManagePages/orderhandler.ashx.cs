﻿using BLL;
using Common;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace System.Web.Aspx.ManagePages
{
    /// <summary>
    /// orderhandler 的摘要说明
    /// </summary>
    public class orderhandler : IHttpHandler
    {

        private OrderService _infoService = new OrderService();
        private DetailService _infodetailService = new DetailService();
        private DeliveryService _infoDeliveryDal = new DeliveryService();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var action = context.Request["action"].ToString();
            switch (action)
            {
                case "send":
                    EditOrderStatusRequest(context);
                    break;
                case "list":
                    ListOrderRequest(context);
                    break;
                case "update":
                    //UpdateOrderRequest(context);
                    break;
                case "delete":
                    //DeleteOrderRequest(context);
                    break;
                case "search":
                    SeachOrderRequest(context);
                    break;
                case "removelist":
                    //DeleteListOrderRequest(context);
                    break;
                case "cart":
                    SeachCartListRequest(context);
                    break;
            }
        }

        /// <summary>
        /// 订单详情列表查询
        /// </summary>
        /// <param name="context"></param>
        public void ListOrderRequest(HttpContext context)
        {

            var page = context.Request.Form["page"];
            var index = context.Request.Form["limit"];
            if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
            {

                var list = _infoService.OrderDetailJoinList();
                list = list ?? new List<OrdersDetailExtend> { };
                var res = SerializeHelp.ToTableJson(list);
                context.Response.Write(res);

            }
            else
            {
                var list = _infoService.OrderDetailJoinList();
                list = list ?? new List<OrdersDetailExtend> { };
                var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                var res = SerializeHelp.ToTableJson(list1, list.Count());
                context.Response.Write(res);
            }
        }

        /// <summary>
        /// 编辑订单状态
        /// </summary>
        /// <param name="context"></param>
        public void EditOrderStatusRequest(HttpContext context)
        {
            var response = new ResponseMessage();

            var id = context.Request.Form["id"];
            var list = _infoService.GetList().Where(y => y.OrdersId == id).SingleOrDefault();
            if (list == null)
            {
                response.code = 101;
                response.msg = "发货失败，不存在该订单信息";
                context.Response.Write(SerializeHelp.ToJson(response));
                return;
            }
            list.States = 2;
            //考虑商品库存 ?
            var update = _infoService.Update(list);
            if (update)
            {
                response.code = 0;
                response.msg = "发货成功";
                context.Response.Write(SerializeHelp.ToJson(response));
            }
            response.code = 500;
            response.msg = "发货失败";
            context.Response.Write(SerializeHelp.ToJson(response));
        }




        /// <summary>
        /// 购物车列表
        /// </summary>
        /// <param name="context"></param>
        public void SeachCartListRequest(HttpContext context)
        {
            var userId = context.Request["UserId"];
            var page = context.Request.Form["page"];
            var index = context.Request.Form["limit"];
            if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
            {
                var list = _infoService.OrderCartList().Where(y => y.UserId == userId)?.ToList();
                list = list ?? new List<OrdersDetailExtend> { };
                var res = SerializeHelp.ToTableJson(list);
                context.Response.Write(res);

            }
            else
            {
                var list = _infoService.OrderCartList().Where(y => y.UserId == userId)?.ToList();
                list = list ?? new List<OrdersDetailExtend> { };
                var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                var res = SerializeHelp.ToTableJson(list1, list.Count());
                context.Response.Write(res);
            }
        }





        /// <summary>
        /// 后台订单详细连表查询
        /// </summary>
        public void SeachOrderRequest(HttpContext context)
        {
            var id = context.Request["id"];
            var page = context.Request.Form["page"];
            var index = context.Request.Form["limit"];
            if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
            {
                var list = _infoService.OrderDetailJoinList().Where(y => y.OrdersId.Contains(id))?.ToList();
                list = list ?? new List<OrdersDetailExtend> { };
                var res = SerializeHelp.ToTableJson(list);
                context.Response.Write(res);
            }
            else
            {
                var list = _infoService.OrderDetailJoinList().Where(y => y.OrdersId.Contains(id))?.ToList();
                list = list ?? new List<OrdersDetailExtend> { };
                var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                var res = SerializeHelp.ToTableJson(list1, list.Count());
                context.Response.Write(res);
            }
        }



        ///// <summary>
        /////删除用户
        ///// </summary>
        ///// <param name="context"></param>
        //public void DeleteOrderRequest(HttpContext context)
        //{
        //    var response = new ResponseMessage();
        //    try
        //    {
        //        var id = context.Request["id"];
        //        var del = _infoService.Delete(id);

        //        if (del)
        //        {
        //            response.code = 0;
        //            response.msg = "删除成功";
        //            context.Response.Write(SerializeHelp.ToJson(response));
        //        }
        //        response.code = 500;
        //        response.msg = "删除失败";
        //        context.Response.Write(SerializeHelp.ToJson(response));
        //    }
        //    catch (Exception e)
        //    {
        //        response.code = 500;
        //        response.msg = "操作失败，请重试";
        //        context.Response.Write(SerializeHelp.ToJson(response));
        //    }
        //}



        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="context"></param>
        public void UpdateOrderRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                string productId = context.Request.Form["ProductId"];
                string quantity = context.Request.Form["Quantity"];
                string detailId = context.Request.Form["DetailId"];

                string ordersId = context.Request.Form["OrdersId"];
                string total = context.Request.Form["Total"];
                string remark = context.Request.Form["Remark"];
                string userId = context.Request.Form["UserId"];
                string deliveryId = context.Request.Form["DeliveryId"];
                string deliveryDate = context.Request.Form["DeliveryDate"];

                //订单
                Orders order = new Orders();
                order.UserId = userId;
                order.Total = Convert.ToDecimal(total);
                order.Remark = remark;
                order.OrdersId = ordersId;
                order.Orderdate = DateTime.Now;
                order.States = 0; //加入购物车未付款
                order.DeliveryId = deliveryId;
                order.DeliveryDate = Convert.ToDateTime(deliveryDate);

                //订单详情
                Detail detail = new Detail
                {
                    DetailId = detailId,
                    OrdersId = order.OrdersId,
                    ProductId = productId,
                    Quantity = Convert.ToInt32(quantity),
                    States = 0
                };

                var add1 = _infoService.Update(order);
                var add2 = _infodetailService.Update(detail);
                if (add1 && add2)
                {
                    response.code = 0;
                    response.msg = "修改成功";
                    context.Response.Write(SerializeHelp.ToJson(response));
                    return;
                }
                response.code = 0;
                response.msg = "修改失败";
                context.Response.Write(SerializeHelp.ToJson(response));
                return;
            }
            catch (Exception e)
            {
                response.code = 500;
                response.msg = "失败，请重试";
                context.Response.Write(SerializeHelp.ToJson(response));
            }

        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        public void AddOrderRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {

                string productId = context.Request.Form["ProductId"];
                string quantity = context.Request.Form["Quantity"];

                string total = context.Request.Form["Total"];
                string remark = context.Request.Form["Remark"];
                string userId = context.Request.Form["UserId"];
                string deliveryId = context.Request.Form["DeliveryId"];
                string deliveryDate = context.Request.Form["DeliveryDate"];

                //订单
                Orders order = new Orders();
                order.UserId = userId;
                order.Total = Convert.ToDecimal(total);
                order.Remark = remark;
                order.OrdersId = Guid.NewGuid().ToString();
                order.Orderdate = DateTime.Now;
                order.States = 0; //加入购物车未付款
                order.DeliveryId = deliveryId;
                order.DeliveryDate = Convert.ToDateTime(deliveryDate);

                //订单详情
                Detail detail = new Detail
                {
                    DetailId = Guid.NewGuid().ToString(),
                    OrdersId = order.OrdersId,
                    ProductId = productId,
                    Quantity = Convert.ToInt32(quantity),
                    States = 0
                };

                var add1 = _infoService.Add(order);
                var add2 = _infodetailService.Add(detail);
                if (add1 && add2)
                {
                    response.code = 0;
                    response.msg = "添加成功";
                    context.Response.Write(SerializeHelp.ToJson(response));
                    return;
                }
                response.code = 0;
                response.msg = "添加失败";
                context.Response.Write(SerializeHelp.ToJson(response));
                return;
            }
            catch (Exception e)
            {
                response.code = 500;
                response.msg = "失败,请重试";
                context.Response.Write(SerializeHelp.ToJson(response));
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}