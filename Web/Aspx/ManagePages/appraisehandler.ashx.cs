using BLL;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Aspx.ManagePages
{

    /// <summary>
    /// appraisehandler 评价
    /// </summary>
    public class appraisehandler : IHttpHandler
    {
        private AppraiseService _userInfoService = new AppraiseService(); //CacheControl.Get<UserInfoService>();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var action = context.Request["action"].ToString();
            switch (action)
            {
                case "add":
                    AddAppraiseRequest(context);
                    break;
                case "list":
                    ListAppraiseRequest(context);
                    break;
                case "update":
                    UpdateAppraiseRequest(context);
                    break;
                case "delete":
                    DeleteAppraiseRequest(context);
                    break;
                //case "search":
                //    SeachAppraiseRequest(context);
                //    break;
                //case "removelist":
                //    DeleteListAppraiseRequest(context);
                //    break;              
            }
        }

       


        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="context"></param>
        //public void DeleteListAppraiseRequest(HttpContext context)
        //{
        //    // ?
        //}


        ///// <summary>
        ///// 搜索用户
        ///// </summary>
        ///// <param name="context"></param>
        //public void SeachAppraiseRequest(HttpContext context)
        //{
        //    var username = context.Request["name"];
        //    var list = _userInfoService.GetList().Where(y => y.UserName.Contains(username)).ToList();
        //    var res = SerializeHelp.ToTableJson<Appraise>(list);
        //    context.Response.Write(res);
        //}

        /// <summary>
        ///删除用户
        /// </summary>
        /// <param name="context"></param>
        public void DeleteAppraiseRequest(HttpContext context)
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
        public void UpdateAppraiseRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {

                string grade = context.Request.Form["Grade"];
                string userId = context.Request.Form["UserId"];         
                string productId = context.Request.Form["ProductId"];
                string appraiseId = context.Request.Form["AppraiseId"];
                Appraise appraise = new Appraise
                {
                    AppraiseId= appraiseId,
                    Grade =  Convert.ToInt32( grade),
                    ProductId = productId,
                    RateTime = DateTime.Now,
                    UserId = userId,
                };
                var edi = _userInfoService.Update(appraise);
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
        public void AddAppraiseRequest(HttpContext context)
        {
            var response = new ResponseMessage<string>();
            try
            {
                  string grade = context.Request.Form["Grade"];
                string userId = context.Request.Form["UserId"];         
                string productId = context.Request.Form["ProductId"];
              
                Appraise appraise = new Appraise
                {
                    AppraiseId= Guid.NewGuid().ToString(),
                    Grade =  Convert.ToInt32( grade),
                    ProductId = productId,
                    RateTime = DateTime.Now,
                    UserId = userId,
                };
                var add = _userInfoService.Add(appraise);
               
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
        public void ListAppraiseRequest(HttpContext context)
        {

            var page = context.Request.Form["page"];
            var index = context.Request.Form["limit"];
            if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
            {
                List<Appraise> list = _userInfoService.GetList();
                var res = SerializeHelp.ToTableJson<Appraise>(list);
                context.Response.Write(res);

            }
            else
            {
                var list = _userInfoService.GetList();
                var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                var res = SerializeHelp.ToTableJson<Appraise>(list1, list.Count());
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