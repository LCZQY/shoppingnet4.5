using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using Common;

namespace Web.Aspx.ManagePages
{
    /// <summary>
    /// deliveryhandler 收货地址
    /// </summary>
    public class deliveryhandler : IHttpHandler
    {

        private DeliveryService _userInfoService = new DeliveryService(); //CacheControl.Get<UserInfoService>();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var action = context.Request["action"].ToString();
            switch (action)
            {
                case "add":
                    AddDeliveryRequest(context);
                    break;
                case "list":
                    ListDeliveryRequest(context);
                    break;
                case "update":
                    UpdateDeliveryRequest(context);
                    break;
                case "delete":
                    DeleteDeliveryRequest(context);
                    break;
                    //case "search":
                    //    SeachDeliveryRequest(context);
                    //    break;
                    //case "removelist":
                    //    DeleteListDeliveryRequest(context);
                    //    break;              
            }
        }




        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="context"></param>
        //public void DeleteListDeliveryRequest(HttpContext context)
        //{
        //    // ?
        //}


        ///// <summary>
        ///// 搜索用户
        ///// </summary>
        ///// <param name="context"></param>
        //public void SeachDeliveryRequest(HttpContext context)
        //{
        //    var username = context.Request["name"];
        //    var list = _userInfoService.GetList().Where(y => y.UserName.Contains(username)).ToList();
        //    var res = SerializeHelp.ToTableJson<Delivery>(list);
        //    context.Response.Write(res);
        //}

        /// <summary>
        ///删除用户
        /// </summary>
        /// <param name="context"></param>
        public void DeleteDeliveryRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                var id = context.Request["id"];
                var del = _userInfoService.Delete(id);
                response.code = del == true ? 0 : 500;
                response.msg = "删除成功";
                context.Response.Write(SerializeHelp.ToJson(response));


            }
            catch (Exception e)
            {
                response.code = 500;
                response.msg = "删除失败";
                context.Response.Write(SerializeHelp.ToJson(response));
            }
        }



        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="context"></param>
        public void UpdateDeliveryRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                string complete = context.Request.Form["Complete"];
                string userId = context.Request.Form["UserId"];
                string consignee = context.Request.Form["Consignee"];
                string phone = context.Request.Form["Phone"];
                string deliveryId = context.Request.Form["DeliveryId"];


                Delivery Delivery = new Delivery
                {
                    DeliveryId = deliveryId,
                    Complete = complete,
                    Consignee = consignee,
                    Phone = phone,
                    UserId = userId,
                };
                var edi = _userInfoService.Update(Delivery);
                if (edi)
                {
                    response.code = 0;
                    response.msg = "修改成功";
                    context.Response.Write(SerializeHelp.ToJson(response));
                    return;
                }
                response.code = 500;
                response.msg = "修改失败";
                context.Response.Write(SerializeHelp.ToJson(response));
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
        public void AddDeliveryRequest(HttpContext context)
        {
            var response = new ResponseMessage<string>();
            try
            {
                string complete = context.Request.Form["Complete"];
                string userId = context.Request.Form["UserId"];
                string consignee = context.Request.Form["Consignee"];
                string phone = context.Request.Form["Phone"];


                Delivery Delivery = new Delivery
                {
                    DeliveryId = Guid.NewGuid().ToString(),
                    Complete = complete,
                    Consignee = consignee,
                    Phone = phone,
                    UserId = userId,
                };
                var add = _userInfoService.Add(Delivery);

                if (add)
                {
                    response.code = 0;
                    response.msg = "添加成功";
                    context.Response.Write(SerializeHelp.ToJson(response));
                    return;
                }
                response.code = 500;
                response.msg = "添加失败";
                context.Response.Write(SerializeHelp.ToJson(response));
            }
            catch (Exception e)
            {
                response.code = 500;
                response.msg = "失败，请重试";
                context.Response.Write(SerializeHelp.ToJson(response));
            }

        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="context"></param>
        public void ListDeliveryRequest(HttpContext context)
        {
            try
            {
                var id = context.Request.Form["UserId"];
                var page = context.Request.Form["page"];
                var index = context.Request.Form["limit"];
                if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
                {
                    var list = _userInfoService.GetList().Where(y => y.UserId == id).ToList();
                    var res = SerializeHelp.ToTableJson<Delivery>(list);
                    context.Response.Write(res);

                }
                else
                {
                    var list = _userInfoService.GetList().Where(y => y.UserId == id ).ToList();
                    var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                    var res = SerializeHelp.ToTableJson<Delivery>(list1, list.Count());
                    context.Response.Write(res);
                }
            }
            catch (Exception e) {

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