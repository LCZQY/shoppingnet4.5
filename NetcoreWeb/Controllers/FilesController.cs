using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingApi.Dto.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZapiCore;

namespace ShoppingApi.Controllers
{

    /// <summary>
    /// 文件 API
    /// </summary>
    [Route("api/file")]
 //   [Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string _fileRootPath = "/Images/";
        public FilesController(ILogger<FilesController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        ///文件上传[计划单独一个文件服务]
        /// </summary>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<ResponseMessage<string>> FileSave(List<IFormFile> formFiles)
        {
            var response = new ResponseMessage<string>();
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            if (size <= 0)
            {
                response.Code = ResponseCodeDefines.NotAllow;
                response.Message = "不能够上传空文件";
                return response;
            }
            // long fileSize = 0;
            string filePath = $".{_fileRootPath}{DateTime.Now.ToString("yyyy-MM-dd")}";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string path = "";
            foreach (var file in files)
            {
                //如果有文件
                if (file.Length > 0)
                {
                    //fileSize = 0;
                    //fileSize = file.Length;
                    string fileExt = (file.FileName).Split('.')[1]; //文件扩展名，不含“.”                   
                    string newFileName = Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                    path = $"{filePath}/{newFileName}";
                    using (var stream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            response.Extension = path;
            return response;
        }






    }
}
