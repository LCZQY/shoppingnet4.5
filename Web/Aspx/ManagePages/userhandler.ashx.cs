using BLL;
using System.Collections.Generic;
using Common;
using System.Linq;
using Model;
namespace System.Web.Aspx.ManagePages
{
    /// <summary>
    /// userhandler 的摘要说明
    /// </summary>
    public class userhandler : IHttpHandler
    {
        private UserInfoService _userInfoService = new UserInfoService(); //CacheControl.Get<UserInfoService>();
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
                case "login":
                    LoginRequest(context);
                    break;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="context"></param>
        public void LoginRequest(HttpContext context)
        {
            var response = new ResponseMessage<string>();
            try
            {
                string userName = context.Request.Form["name"];
                string userPwd = context.Request.Form["pwd"];
                var model = _userInfoService.GetList().Where(y => y.UserName == userName && y.Pwd == userPwd).SingleOrDefault();
                if (model != null)
                {
                    response.code = 0;
                    response.msg = "登陆成功";
                    //response.model = model.UserId+'.'+model.Nick;
                    response.model = model.UserId;
                    context.Response.Write(SerializeHelp.ToJson(response));
                    return;
                }
                response.code = 500;
                response.msg = "用户名或密码错误";
                context.Response.Write(SerializeHelp.ToJson(response));
                return;
            } catch (Exception e)
            {
                response.code =500;
                response.msg = "失败，请重试";
                context.Response.Write(SerializeHelp.ToJson(response));
                return;
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
            var list = _userInfoService.GetList().Where(y => y.UserName.Contains(username)).ToList();
            var res = SerializeHelp.ToTableJson<Customer>(list);
            context.Response.Write(res);
        }

        /// <summary>
        ///删除用户
        /// </summary>
        /// <param name="context"></param>
        public void DeleteCustomerRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                var id = context.Request["id"];
                var del = _userInfoService.DeleteUserInfo(id);

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
        public void UpdateCustomerRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {

                string userName = context.Request.Form["UserName"];
                string userPwd = context.Request.Form["Pwd"];
                string userEmail = context.Request.Form["Email"];
                string usernick = context.Request.Form["Nick"];
                string id = context.Request.Form["UserId"];
                string deliveryId = context.Request.Form["DeliveryId"];
                Customer userInfo = new Customer();
                userInfo.UserName = userName;
                userInfo.Pwd = userPwd;
                userInfo.Email = userEmail;
                userInfo.Nick = usernick;
                userInfo.DeliveryId = deliveryId;
                userInfo.UserId = id;

                var edi = _userInfoService.UpdateUserInfo(userInfo);
                response.code = edi == true ? 0 : 500;
                response.msg = "修改成功";
                context.Response.Write(SerializeHelp.ToJson(response));
            }
            catch (Exception e)
            {
                response.code = 500;
                response.msg = "修改失败";
                context.Response.Write(SerializeHelp.ToJson(response));
            }

        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="context"></param>
        public void AddCustomerRequest(HttpContext context)
        {
            var response = new ResponseMessage<string>();
            try
            {
                string userName = context.Request.Form["name"];
                string userPwd = context.Request.Form["pwd"];
                string userEmail = context.Request.Form["email"];
                string usernick = context.Request.Form["nick"];
                Customer userInfo = new Customer();
                userInfo.UserId = Guid.NewGuid().ToString();
                userInfo.UserName = userName;
                userInfo.Pwd = userPwd;
                userInfo.Email = userEmail;
                userInfo.Nick = usernick;
                userInfo.DeliveryId = "?";
                var add = _userInfoService.AddUserInfo(userInfo);

                if (add)
                {
                    response.code = 0;
                    response.msg = "添加成功";
                    //response.model = userInfo.UserId + '.' + userInfo.Nick;
                    response.model = userInfo.UserId;
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
        /// 用户列表查询
        /// </summary>
        /// <param name="context"></param>
        public void ListCustomerRequest(HttpContext context)
        {

            var page = context.Request.Form["page"];
            var index = context.Request.Form["limit"];
            if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
            {
                List<Customer> list = _userInfoService.GetList();
                var res = SerializeHelp.ToTableJson<Customer>(list);
                context.Response.Write(res);

            }
            else
            {
                var list = _userInfoService.GetList();
                var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                var res = SerializeHelp.ToTableJson<Customer>(list1, list.Count());
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