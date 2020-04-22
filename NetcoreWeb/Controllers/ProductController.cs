﻿using AutoMapper.Configuration;
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
using System.Security.Cryptography;
using System.Text;
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

     //   public static IConfiguration _configuration;
        private readonly ILogger<ProductController> _logger;
        private readonly ProdoctManager _prodoctManager;
        public ProductController(ILogger<ProductController> logger, ProdoctManager prodoctManager)//, IConfiguration configuration)
        {
           // _configuration = configuration;
            _logger = logger;
            _prodoctManager = prodoctManager;
        }

        private  string _fileRootPath = "/uploads/files/";//_configuration["CommonSettings:FilePath"];

        /*
        * 第一步，握手传递文件大小，多少片，md5值，文件类型
        * 第二步，分片上传文件片段，存储文件
        * 第三步，合并所有文件，做md5校验
        */


        /// <summary>
        /// 请求上传文件
        /// </summary>
        /// <param name="requestFile">请求上传参数实体</param>
        /// <returns></returns>
        [HttpPost, Route("RequestUpload")]
        public async Task<ResponseMessage<object>> RequestUploadFile([FromBody]RequestFileUploadEntity requestFile)
        {
            //LogUtil.Debug($"RequestUploadFile 接收参数：{JsonConvert.SerializeObject(requestFile)}");
            var message = new ResponseMessage<object>();
            if (requestFile.size <= 0 || requestFile.count <= 0 || string.IsNullOrEmpty(requestFile.filedata))
            {
                message.Code = ResponseCodeDefines.ServiceError;
                message.Message = "参数有误";
            }
            else
            {
                //这里需要记录文件相关信息，并返回文件guid名，后续请求带上此参数
                string guidName = Guid.NewGuid().ToString("N");


                _logger.LogInformation($"请求的文件信息是：{JsonHelper.ToJson(requestFile)}");
                //前期单台服务器可以记录Cache，多台后需考虑redis或数据库
                CacheUtil.Set(guidName, requestFile, new TimeSpan(0, 10, 0), true);
                message.Code = ResponseCodeDefines.SuccessCode;
                message.Message = "";
                message.Extension = new { filename = guidName };
            }
            return message;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("Upload")]
        public async Task<ResponseMessage> FileSave()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string fileName = Request.Form["filename"];

            int fileIndex = 0;
            int.TryParse(Request.Form["fileindex"], out fileIndex);
            //LogUtil.Debug($"FileSave开始执行获取数据：{fileIndex}_{size}");
            var message = new ResponseMessage();
            if (size <= 0 || string.IsNullOrEmpty(fileName))
            {
                message.Code = ResponseCodeDefines.ServiceError;
                message.Message = "文件上传失败";
                return message;
            }
            if (!CacheUtil.Exists(fileName))
            {
                message.Code = ResponseCodeDefines.ServiceError;
                message.Message = "请重新请求上传文件";
                return message;
            }

            long fileSize = 0;
            string filePath = $".{_fileRootPath}{DateTime.Now.ToString("yyyy-MM-dd")}/{fileName}";
            string saveFileName = $"{fileName}_{fileIndex}";
            string dirPath = Path.Combine(filePath, saveFileName);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (var file in files)
            {
                //如果有文件
                if (file.Length > 0)
                {
                    fileSize = 0;
                    fileSize = file.Length;                   
                    using (var stream = new FileStream(dirPath, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            message.Code = ResponseCodeDefines.SuccessCode;
            message.Message = "文件上传成功";
            return message;
        }

        /// <summary>
        /// 文件合并
        /// </summary>
        /// <param name="fileInfo">文件参数信息[name]</param>
        /// <returns></returns>
        [HttpPost, Route("Merge")]
        public async Task<ResponseMessage> FileMerge([FromBody]Dictionary<string, object> fileInfo)
        {
            var message = new ResponseMessage();
            string fileName = string.Empty;
            if (fileInfo.ContainsKey("name"))
            {
                fileName = fileInfo["name"].ToString();
            }
            _logger.LogInformation($"请求文件保存的key值：{fileName}");
            if (string.IsNullOrEmpty(fileName))
            {
                message.Code = ResponseCodeDefines.ServiceError;
                message.Message = "文件名不能为空";
                return message;
            }

            //最终上传完成后，请求合并返回合并消息
            try
            {
                RequestFileUploadEntity requestFile = CacheUtil.Get<RequestFileUploadEntity>(fileName);
                if (requestFile == null)
                {
                    message.Code = ResponseCodeDefines.ServiceError;
                    message.Message = "合并失败";
                    return message;
                }
                string filePath = $".{_fileRootPath}{DateTime.Now.ToString("yyyy-MM-dd")}/{fileName}";
                string fileExt = requestFile.fileext;
                string fileMd5 = requestFile.filedata;
                int fileCount = requestFile.count;
                long fileSize = requestFile.size;

              //  LogUtil.Debug($"获取文件路径：{filePath}");
              //  LogUtil.Debug($"获取文件类型：{fileExt}");

                string savePath = filePath.Replace(fileName, "");
                string saveFileName = $"{fileName}{fileExt}";
                var files = Directory.GetFiles(filePath);
                string fileFinalName = Path.Combine(savePath, saveFileName);
               // LogUtil.Debug($"获取文件最终路径：{fileFinalName}");
                FileStream fs = new FileStream(fileFinalName, FileMode.Create);
               // LogUtil.Debug($"目录文件下文件总数：{files.Length}");
              //  LogUtil.Debug($"目录文件排序前：{string.Join(",", files.ToArray())}");
              //  LogUtil.Debug($"目录文件排序后：{string.Join(",", files.OrderBy(x => x.Length).ThenBy(x => x))}");
                byte[] finalBytes = new byte[fileSize];
                foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))
                {
                    var bytes = System.IO.File.ReadAllBytes(part);
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                    bytes = null;
                    System.IO.File.Delete(part);//删除分块
                }
                fs.Close();
                //这个地方会引发文件被占用异常
                fs = new FileStream(fileFinalName, FileMode.Open);
                string strMd5 = GetCryptoString(fs);
               _logger.LogInformation($"文件数据MD5：{strMd5}");
                _logger.LogInformation($"文件上传数据：{JsonHelper.ToJson(requestFile)}");
                fs.Close();
                Directory.Delete(filePath);
                //如果MD5与原MD5不匹配，提示重新上传
                if (strMd5 != requestFile.filedata)
                {
                 //   LogUtil.Debug($"上传文件md5：{requestFile.filedata},服务器保存文件md5：{strMd5}");
                    message.Code = ResponseCodeDefines.ServiceError;
                    message.Message = "MD5值不匹配";
                    return message;
                }
                CacheUtil.Remove(fileInfo["name"].ToString());
                message.Code =  ResponseCodeDefines.SuccessCode;
                message.Message = "文件合并成功 ";
            }
            catch (Exception ex)
            {
               // LogUtil.Error($"合并文件失败，文件名称：{fileName}，错误信息：{ex.Message}");
                message.Code = ResponseCodeDefines.ServiceError;
                message.Message = "合并文件失败,请重新上传";
            }
            return message;
        }

        /// <summary>
        /// 文件流加密
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        private string GetCryptoString(Stream fileStream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] cryptBytes = md5.ComputeHash(fileStream);
            return GetCryptoString(cryptBytes);
        }

        private string GetCryptoString(byte[] cryptBytes)
        {
            //加密的二进制转为string类型返回
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cryptBytes.Length; i++)
            {
                sb.Append(cryptBytes[i].ToString("x2"));
            }
            return sb.ToString();
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
