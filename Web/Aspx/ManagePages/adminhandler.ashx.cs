﻿using BLL;
using Common;
using System.Linq;
using Model;

namespace System.Web.Aspx.ManagePages
{
    /// <summary>
    /// adminhandler 的摘要说明
    /// </summary>
    public class adminhandler : IHttpHandler
    {

        private AdminCustomerervice _AdminCustomerervice = new AdminCustomerervice(); //CacheControl.Get<AdminCustomerervice>();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var action = context.Request["action"].ToString();
            switch (action)
            {
                case "add":
                    AddCustomerRequest(context);
                    break;
                case "list":
                    ListCustomerRequest(context);
                    break;
                case "update":
                    UpdateCustomerRequest(context);
                    break;
                case "delete":
                    DeleteCustomerRequest(context);
                    break;
                case "search":
                    SeachCustomerRequest(context);
                    break;
                case "removelist":
                    DeleteListCustomerRequest(context);
                    break;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="context"></param>
        public void DeleteListCustomerRequest(HttpContext context)
        {
            // ?
        }




        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="context"></param>
        public void SeachCustomerRequest(HttpContext context)
        {
            var username = context.Request["name"];
            var page = context.Request.Form["page"];
            var index = context.Request.Form["limit"];
            if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
            {
                //排除超级管理员
                var list = _AdminCustomerervice.GetList().Where(y => y.Role != 1 && y.UserName.Contains(username)).ToList();
                var res = SerializeHelp.ToTableJson(list);
                context.Response.Write(res);
            }
            else
            {
                var list = _AdminCustomerervice.GetList().Where(y => y.Role != 1 && y.UserName.Contains(username));
                var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                var res = SerializeHelp.ToTableJson(list1, list.Count());
                context.Response.Write(res);
            }
        }

        /// <summary>
        ///删除管理员账号
        /// </summary>
        /// <param name="context"></param>
        public void DeleteCustomerRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                var id = context.Request["id"];
                var del = _AdminCustomerervice.Delete(id);

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
        /// 修改管理员账号
        /// </summary>
        /// <param name="context"></param>
        public void UpdateCustomerRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                string userName = context.Request.Form["UserName"];
                string role = context.Request.Form["Role"];
                string id = context.Request.Form["SuId"];

                var adminUser = new AdminUser();
                adminUser.SuId = id;
                adminUser.Role = Convert.ToInt32(role);
                adminUser.UserName = userName;
                adminUser.Pwd = "123456";

                var edi = _AdminCustomerervice.Update(adminUser);
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
                response.msg = "操作失败，请重试";
                context.Response.Write(SerializeHelp.ToJson(response));
            }

        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="context"></param>
        public void AddCustomerRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                string userName = context.Request.Form["UserName"];
                string role = context.Request.Form["Role"];

                var adminUser = new AdminUser();
                adminUser.SuId = Guid.NewGuid().ToString();
                adminUser.Role = 2;//新开账户不给予赋予权限
                adminUser.UserName = userName;
                adminUser.Pwd = "123456";
                var add = _AdminCustomerervice.Add(adminUser);
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
                response.msg = "操作失败，请重试";
                context.Response.Write(SerializeHelp.ToJson(response));
            }

        }

        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <param name="context"></param>
        public void ListCustomerRequest(HttpContext context)
        {

            var page = context.Request.Form["page"];
            var index = context.Request.Form["limit"];
            if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
            {
                //排除超级管理员
                var list = _AdminCustomerervice.GetList().Where(y => y.Role != 1).ToList();
                var res = SerializeHelp.ToTableJson(list);
                context.Response.Write(res);
            }
            else
            {
                var list = _AdminCustomerervice.GetList().Where(y => y.Role != 1);
                var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                var res = SerializeHelp.ToTableJson(list1, list.Count());
                context.Response.Write(res);
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