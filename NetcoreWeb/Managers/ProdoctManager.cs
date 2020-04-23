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
    public class ProdoctManager
    {
        private readonly IMapper _mapper;
        private readonly IProductStore _ProductStore;
        private readonly IFilesStore _filesStore;
        private readonly ILogger<ProdoctManager> _logger;
        private readonly ITransaction<ShoppingDbContext> _transaction;
        public ProdoctManager(IProductStore ProductStore, ILogger<ProdoctManager> logger, IMapper mapper, IFilesStore photoStore, ITransaction<ShoppingDbContext> transaction)
        {
            _ProductStore = ProductStore;
            _filesStore = photoStore;
            _logger = logger;
            _mapper = mapper;
            _transaction = transaction;
        }


        /// <summary>
        /// 兼容Layui表格数据结构得商品列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<LayerTableJson> LayuiProductListAsync(LayuiTableRequest search, CancellationToken cancellationToken)
        {
            var response = new LayerTableJson() { };
            var entity = _ProductStore.IQueryableListAsync();
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                entity = entity.Where(y => y.Title.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.CateId))
            {
                entity = entity.Where(y => y.CateId == search.CateId);
            }
            response.Count =await entity.CountAsync(cancellationToken);
            var list = await entity.Skip(((search.Page ?? 0) - 1) * search.Limit ?? 0).Take(search.Limit ?? 0).ToListAsync(cancellationToken);
            var data = _mapper.Map<List<ProductListResponse>>(list);
            var img = await _filesStore.IQueryableListAsync().Where(y => data.Select(pro => pro.Id).Contains(y.ProductId) && !y.IsDeleted).ToListAsync(cancellationToken);
            data.ForEach(item =>
            {
                if (img.Where(y => y.ProductId == item.Id).Any())
                {
                    item.Files = img.Select(y => y.Url).ToList();
                }
                else
                {
                    item.Files = new List<string>() { };
                }
            });
      
            response.Data = data;
            return response;
        }



        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagingResponseMessage<ProductListResponse>> ProductListAsync(SearchProductRequest search, CancellationToken cancellationToken)
        {
            var response = new PagingResponseMessage<ProductListResponse>() { };
            var entity = _ProductStore.IQueryableListAsync();
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                entity = entity.Where(y => y.Title.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.CateId))
            {
                entity = entity.Where(y => y.CateId == search.CateId);
            }
            var list = await entity.Skip(search.PageIndex * search.PageSize).Take(search.PageSize).ToListAsync(cancellationToken);
            var data = _mapper.Map<List<ProductListResponse>>(list);
            //var img = await _filesStore.IQueryableListAsync().Where(y => data.Select(pro => pro.Id).Contains(y.ProductId) && !y.IsDeleted).ToListAsync(cancellationToken); //>>>> 不能在循环里找到数据库
            //data.ForEach(item =>
            //{
            //    if (img.Where(y => y.ProductId == item.Id).Any())
            //    {
            //        item.Files = img.Select(y => y.Url).ToList();
            //    }
            //    else
            //    {
            //        item.Files = new List<string>() { };
            //    }
            //});
            response.PageIndex = search.PageIndex;
            response.PageSize = search.PageSize;
            response.Extension = data;
            return response;
        }


        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<ProductListResponse>> ProductDetailsAsync(string id, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<ProductListResponse>() { Extension = new ProductListResponse { } };
            var entity =await _ProductStore.GetAsync(id);                       
            var data = _mapper.Map<ProductListResponse>(entity);
            data.Files = await _filesStore.IQueryableListAsync().Where(y => id == y.ProductId && !y.IsDeleted).Select(y => y.Url).ToListAsync(cancellationToken);                                  
            response.Extension = data;
            return response;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> ProductAddAsync(ProductEditRequest editRequest, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (editRequest == null)
            {
                throw new ArgumentNullException();
            }
            using (var transaction = await _transaction.BeginTransaction())
            {
                try
                {
                    var product = _mapper.Map<Product>(editRequest);
                    product.Id = Guid.NewGuid().ToString();
                    product.Icon = editRequest.Files.Where(item => item.IsIcon).SingleOrDefault().Url;
                    //新增图片
                    var images = new List<Files> { };
                    editRequest.Files.ForEach(img =>
                    {
                        images.Add(new Files
                        {
                            IsDeleted = false,
                            Id = Guid.NewGuid().ToString(),
                            Url = img.Url,
                            IsIcon = img.IsIcon,
                            ProductId = product.Id
                        });
                    });
                    await _filesStore.AddRangeEntityAsync(images);
                    response.Extension = await _ProductStore.AddEntityAsync(product);
                   await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw e;
                }
            }
            return response;
        }



        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExists(string id)
        {
            if (_ProductStore.IsExists(id))
                return true;
            return false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> ProductUpdateAsync(ProductEditRequest editRequest, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            var Product = _mapper.Map<Product>(editRequest);
            using (var transaction = await _transaction.BeginTransaction())
            {
                
                try
                {
                    //存在修改图片 先判断原来是否有图片  存在判断是否有修改 对比异常 删除原理和新增现有数据？               
                    var oldfile = await _filesStore.IQueryableListAsync().Where(item => !item.IsDeleted && item.ProductId == editRequest.Id).Select(img => img.Url).ToListAsync();//1,5,4,3
                    var newfile = editRequest.Files.Select(y => y.Url); //1,2,3,4     
                                                                         //求差集 TODO 待测试
                    var except = oldfile.Except(newfile).ToList(); //2
                    if (except.Any())
                    {
                        var photo = new List<Files>() { };
                        except.ForEach(file =>
                           {
                               photo.Add(new Files
                               {
                                   Id = Guid.NewGuid().ToString(),
                                   IsDeleted = false,
                                   Url = file,
                                   ProductId = editRequest.Id
                               });
                           });
                        await _filesStore.AddRangeEntityAsync(photo);
                    }
                    if (await _ProductStore.PutEntityAsync(Product.Id, Product))
                    {
                        response.Extension = true;
                    }
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw e;
                }
            }
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
            if (await _ProductStore.DeleteAsync(id))
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
        public async Task<ResponseMessage<bool>> AddRanageAsync(List<Product> Products, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (Products.Count == 0)
            {
                throw new ArgumentNullException();
            }
            response.Extension = await _ProductStore.AddRangeEntityAsync(Products);
            return response;
        }
    }
}
