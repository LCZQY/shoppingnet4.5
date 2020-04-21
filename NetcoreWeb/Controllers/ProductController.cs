using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingApi.Common;
using ShoppingApi.Dto.Request;
using ShoppingApi.Dto.Response;
using ShoppingApi.Managers;
using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <summary>
        /// 文件存放的根路径
        /// </summary>
        private readonly string _baseUploadDir;


        private readonly ILogger<ProductController> _logger;
        private readonly ProdoctManager _prodoctManager;
        public ProductController(ILogger<ProductController> logger, ProdoctManager prodoctManager)
        {
   
            _logger = logger;
            _prodoctManager =  prodoctManager;
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



        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
           

            foreach (var formFile in files)
            {
                
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size });
        }

        /// <summary>
        /// 分片上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("slice/file")]
        public async Task<ActionResult> UploadFile()
        {//https://www.cnblogs.com/fger/p/11293277.html 总体思路
            var data = Request.Form.Files["data"]; 
            string lastModified = Request.Form["lastModified"].ToString();
            var total = Request.Form["total"]; //
            var fileName = Request.Form["fileName"];
            var index = Request.Form["index"];

            string temporary = Path.Combine(@"D:\浏览器", lastModified);//临时保存分块的目录
            try
            {
                if (!Directory.Exists(temporary))
                    Directory.CreateDirectory(temporary);
                string filePath = Path.Combine(temporary, index.ToString());
                if (!Convert.IsDBNull(data))
                {
                    await Task.Run(() => {
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        data.CopyTo(fs);
                    });
                }
                bool mergeOk = false;
                if (total == index)
                {
                    mergeOk = await FileMerge(lastModified, fileName);
                }

                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("number", index);
                result.Add("mergeOk", mergeOk);
                return new JsonResult(result);

            }
            catch (Exception ex)
            {
                Directory.Delete(temporary);//删除文件夹
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastModified"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<bool> FileMerge(string lastModified, string fileName)
        {
            bool ok = false;
            try
            {
                var temporary = Path.Combine(@"D:\浏览器", lastModified);//临时文件夹
                fileName = Request.Form["fileName"];//文件名
                string fileExt = Path.GetExtension(fileName);//获取文件后缀
                var files = Directory.GetFiles(temporary);//获得下面的所有文件
                var finalPath = Path.Combine(@"D:\浏览器", DateTime.Now.ToString("yyMMddHHmmss")+ fileExt);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）
                var fs = new FileStream(finalPath, FileMode.Create);
                foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
                {
                    var bytes = System.IO.File.ReadAllBytes(part);
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                    bytes = null;
                    System.IO.File.Delete(part);//删除分块
                }
                fs.Close();
                Directory.Delete(temporary);//删除文件夹
                ok = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ok;
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
