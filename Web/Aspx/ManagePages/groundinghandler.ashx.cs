﻿using BLL;
using Common;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace System.Web.Aspx.ManagePages
{
    /// <summary>
    /// groundinghandler 的摘要说明
    /// </summary>
    public class groundinghandler : IHttpHandler
    {

        private ProductService _InfoService = new ProductService(); //CacheControl.Get<ProductService>();
        private PhotoService _photoInfoService = new PhotoService();// CacheControl.Get<PhotoService>();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var action = context.Request["action"].ToString();
            switch (action)
            {
                case "add":
                    AddProductRequest(context);
                    break;
                case "list":
                    ListProductRequest(context);
                    break;
                case "update":
                    UpdateProductRequest(context);
                    break;
                case "delete":
                    DeleteProductRequest(context);
                    break;
                case "search":
                    SeachProductRequest(context);
                    break;
                case "productlist":
                    GetProductList(context);
                    break;
                case "upload":
                    UploadImg(context);
                    break;
                case "seach":
                    SeachProductList(context);
                    break;

            }

        }

        /// <summary>
        /// 获取商品【前端商品列表搜索调用】
        /// </summary>
        /// <returns></returns>
        public void SeachProductList(HttpContext context)
        {
            try
            {
                var title = context.Request["Title"];
                var list1 = _InfoService.GetList().ToList();
                var list2 = _photoInfoService.GetList().ToList();
                //全关联   
                var list3 = from c in list1
                            join b in list2
                            on c.ProductId equals b.ProductId
                            select new ProductEx
                            {
                                Content = c.Content,
                                CateId = c.CateId,
                                MarketPrice = c.MarketPrice,
                                Path = b.PhotoUrl == null ? "" : b.PhotoUrl,
                                Price = c.Price,
                                ProductId = c.ProductId,
                                Title = c.Title,
                                Stock = c.Stock
                            };
                list3 = list3.Where(y => y.Title.Contains(title)).ToList();
                context.Response.Write(SerializeHelp.ToJson(list3));
            }
            catch {

            }
        }
        /// <summary>
        /// 获取商品
        /// </summary>
        /// <returns></returns>
        public void GetProductList(HttpContext context)
        {
            try
            {

                var list1 = _InfoService.GetList().ToList();
                var list2 = _photoInfoService.GetList().ToList();
                //全关联
                var list3 = from c in list1
                            join b in list2
                            on c.ProductId equals b.ProductId
                            select new ProductEx
                            {
                                Content = c.Content,
                                CateId = c.CateId,
                                MarketPrice = c.MarketPrice,
                                Path = b.PhotoUrl == null ? "" : b.PhotoUrl,
                                Price = c.Price,
                                ProductId = c.ProductId,
                                Title = c.Title,
                                Stock = c.Stock
                            };

                context.Response.Write(SerializeHelp.ToJson(list3.ToList()));
            }
            catch (Exception e)
            {

                context.Response.Write(SerializeHelp.ToJson(new List<string> { }));
            }
        }


        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="context"></param>
        public void SeachProductRequest(HttpContext context)
        {
            var title = context.Request["Title"];
            var page = context.Request.Form["page"];
            var index = context.Request.Form["limit"];
            if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
            {
                var list = _InfoService.GetList().Where(y => y.Title.Contains(title)).ToList();
                list = list ?? new List<Product> { };
                var res = SerializeHelp.ToTableJson(list);
                context.Response.Write(res);
            }
            else
            {
                var list = _InfoService.GetList().Where(y => y.Title.Contains(title)).ToList();
                list = list ?? new List<Product> { };
                var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                var res = SerializeHelp.ToTableJson(list1, list.Count());
                context.Response.Write(res);
            }

        }

        /// <summary>
        ///删除用户
        /// </summary>
        /// <param name="context"></param>
        public void DeleteProductRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                var id = context.Request["id"];
                var del = _InfoService.Delete(id);
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
        public void UpdateProductRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                string id = context.Request.Form["ProductId"];
                string title = context.Request.Form["Title"];
                string cateId = context.Request.Form["CateId"];
                string type = context.Request.Form["Content"];
                string content = context.Request.Form["Content"];
                var marketPrice = Convert.ToDecimal(context.Request.Form["MarketPrice"]);
                var price = Convert.ToDecimal(context.Request.Form["Price"]);
                var stock = Convert.ToInt32(context.Request.Form["Stock"]);
                var postTime = Convert.ToDateTime(context.Request.Form["PostTime"]);
                Product Product = new Product()
                {

                    ProductId = id,
                    Title = title,
                    CateId = cateId,
                    Content = content,
                    MarketPrice = marketPrice,
                    Stock = stock,
                    PostTime = postTime,
                    Price = price
                };
                var edi = _InfoService.Update(Product);
                if (edi)
                {
                    response.code = edi == true ? 0 : 500;
                    response.msg = "修改成功";
                    context.Response.Write(SerializeHelp.ToJson(response));
                }
                else
                {
                    response.code = 500;
                    response.msg = "修改失败";
                    context.Response.Write(SerializeHelp.ToJson(response));
                }
            }
            catch (Exception e)
            {
                response.code = 500;
                response.msg = "请重试";
                context.Response.Write(SerializeHelp.ToJson(response));
            }

        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="context"></param>
        public void AddProductRequest(HttpContext context)
        {
            var response = new ResponseMessage();
            try
            {
                string title = context.Request.Form["Title"];
                string cateId = context.Request.Form["CateId"];
                string content = context.Request.Form["Content"];
                var marketPrice = Convert.ToDecimal(context.Request.Form["MarketPrice"]);
                var price = Convert.ToDecimal(context.Request.Form["Price"]);
                var stock = Convert.ToInt32(context.Request.Form["Stock"]);
                var postTime = Convert.ToDateTime(context.Request.Form["PostTime"]);
                var path = context.Request.Form["PhotoId"];


                var Id = Guid.NewGuid().ToString();
                /*如何开启事务 ?*/
                //图片
                Photo photo = new Photo
                {
                    PhotoId = Guid.NewGuid().ToString(),
                    PhotoUrl = path,
                    ProductId = Id
                };
                _photoInfoService.Add(photo);
                //商品
                Product Product = new Product()
                {

                    ProductId = Id,
                    Title = title,
                    CateId = cateId,
                    Content = content,
                    MarketPrice = marketPrice,
                    Stock = stock,
                    PostTime = postTime,
                    Price = price,
                    Icon = path

                };
                var add = _InfoService.Add(Product);

                response.code = add == true ? 0 : 500;
                response.msg = "添加成功";
                context.Response.Write(SerializeHelp.ToJson(response));
            }
            catch (Exception e)
            {
                string eroor = e.Message;
                response.code = 500;
                response.msg = "添加失败";
                context.Response.Write(SerializeHelp.ToJson(response));
            }

        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        public void ListProductRequest(HttpContext context)
        {

            var page = context.Request.Form["page"];
            var index = context.Request.Form["limit"];
            if (string.IsNullOrWhiteSpace(page) && string.IsNullOrWhiteSpace(index))
            {
                var list = _InfoService.GetList().ToList();
                list = list ?? new List<Product> { };
                var res = SerializeHelp.ToTableJson(list);
                context.Response.Write(res);
            }
            else
            {
                var list = _InfoService.GetList().ToList();
                list = list ?? new List<Product> { };
                var list1 = list.Skip((int.Parse(page) - 1) * int.Parse(index)).Take(int.Parse(index)).ToList();
                var res = SerializeHelp.ToTableJson(list1, list.Count());
                context.Response.Write(res);
            }
        }

        public void UploadImg(HttpContext context)
        {

            if (context.Request.Files.Count > 0)
            {
                var imgSrc = SerializeHelp.UploadFile(context);
                var res = new { msg = "ok", code = 0, src = imgSrc };
                context.Response.Write(SerializeHelp.ToJson(res));
            }
            else
            {
                var res = new { msg = "no", code = 500, src = "" };
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