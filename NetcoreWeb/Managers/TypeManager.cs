using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingApi.Common;
using ShoppingApi.Dto.Request;
using ShoppingApi.Dto.Response;
using ShoppingApi.Models;
using ShoppingApi.Stores.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZapiCore;
namespace ShoppingApi.Managers
{


    /// <summary>
    /// 商品类型管理逻辑处理
    /// </summary>
    public class TypeManager
    {
        private readonly IMapper _mapper;

        private readonly ITypeStore _typeStore;
        private readonly ILogger<TypeManager> _logger;
      //  private readonly ITransaction<ShoppingDbContext> _transaction;
        public TypeManager(ILogger<TypeManager> logger, IMapper mapper, ITypeStore typeStore/* ,ITransaction<ShoppingDbContext> transaction*/)
        {

            _typeStore = typeStore;
            _logger = logger;
            _mapper = mapper;
           // _transaction = transaction;
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagingResponseMessage<CategoryListResponse>> TypeListAsync(SearchTypeRequest search, CancellationToken cancellationToken)
        {
            var response = new PagingResponseMessage<CategoryListResponse>() { };   
            var list = await _typeStore.IQueryableListAsync().Where(type => type.ParentId == search.Parentid).Skip(search.PageIndex * search.PageSize).Take(search.PageSize).ToListAsync(cancellationToken);
            var data = _mapper.Map<List<CategoryListResponse>>(list);
            response.PageIndex = search.PageIndex;
            response.PageSize = search.PageSize;
            response.Extension = data;
            return response;
        }


        /// <summary>
        /// 组合商品类型树状结构(仅是支持Layui树状结构)
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<LayerTreeJson>> CreateTypeTreeResponseListAsync(CancellationToken cancellationToken,string parentId = "0")
        {
            var jsontree = new List<LayerTreeJson>();
            var data = await _typeStore.IQueryableListAsync().Where(y => y.ParentId == parentId && !y.IsDeleted ).ToListAsync(cancellationToken);
            foreach (var item in data)
            {
                jsontree.Add(new LayerTreeJson
                {
                    Id = item.Id,
                    Title = item.CateName,
                    Label =item.CateName,
                    Children = await CreateTypeTreeResponseListAsync(cancellationToken, item.Id)
                });
            }
            return jsontree;
        }



        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> TypeAddAsync(CategoryEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (editRequest == null)
            {
                throw new ArgumentNullException();
            }

            var category = _mapper.Map<Category>(editRequest);
            category.Id = Guid.NewGuid().ToString();
            response.Extension = await _typeStore.AddEntityAsync(category);
            return response;
        }




        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> TypeUpdateAsync(CategoryEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            var category = await _typeStore.GetAsync(editRequest.Id);
            category.CateName = editRequest.CateName;
            category.IsDeleted = false;
            response.Extension = await _typeStore.PutEntityAsync(editRequest.Id, category);
            return response;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> ProductDeleteAsync(string id)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (await _typeStore.DeleteAsync(id))
            {
                response.Extension = true;
            }
            return response;
        }


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="Products"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> AddRanageAsync(List<CategoryEditRequest> categoryList, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (categoryList.Count == 0)
            {
                throw new ArgumentNullException();
            }
            var list = _mapper.Map<List<Category>>(categoryList);

            response.Extension = await _typeStore.AddRangeEntityAsync(list);
            return response;
        }


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExists(string id)
        {
            if (_typeStore.IsExists(id))
                return true;
            return false;
        }
    }
}
