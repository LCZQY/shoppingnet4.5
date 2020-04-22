using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingApi.Common;
using ShoppingApi.Dto.Request;
using ShoppingApi.Dto.Response;
using ShoppingApi.Models;
using ShoppingApi.Stores.Interface;
using ZapiCore;
namespace ShoppingApi.Managers
{


    /// <summary>
    /// 客户管理逻辑处理
    /// </summary>
    public class FilesManager
    {
        private readonly IMapper _mapper;
        private readonly IFilesStore _filesStore;
        private readonly ILogger<ProdoctManager> _logger;
        private readonly ITransaction<ShoppingDbContext> _transaction;
        public FilesManager(IProductStore ProductStore, ILogger<ProdoctManager> logger, IMapper mapper, IFilesStore photoStore, ITransaction<ShoppingDbContext> transaction)
        {

            _filesStore = photoStore;
            _logger = logger;
            _mapper = mapper;
            _transaction = transaction;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="fileRequests"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> AddRanageAsync(List<FileRequest> fileRequests, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (fileRequests.Any())
            {
                throw new ZCustomizeException(ResponseCodeEnum.NotAllow);
            }
            var files = _mapper.Map<List<Files>>(fileRequests);
            response.Extension = await _filesStore.AddRangeEntityAsync(files);
            return response;
        }


    }
}
