using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly IPhotoStore _photoStore;
        private readonly ILogger<ProdoctManager> _logger;
        private readonly ITransaction<ShoppingDbContext> _transaction;
        public ProdoctManager(IProductStore ProductStore, ILogger<ProdoctManager> logger, IMapper mapper, IPhotoStore photoStore, ITransaction<ShoppingDbContext> transaction)
        {
            _ProductStore = ProductStore;
            _photoStore = photoStore;
            _logger = logger;
            _mapper = mapper;
            _transaction = transaction;
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
            var entity = await _ProductStore.IQueryableListAsync();
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                entity.Where(y => y.Title.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.CateId))
            {
                entity.Where(y => y.CateId == search.CateId);
            }
            var list = await entity.Skip(search.PageIndex * search.PageSize).Take(search.PageSize).ToListAsync(cancellationToken);
            var data = _mapper.Map<List<ProductListResponse>>(list);
            data.ForEach(async item =>
            {
                var img = await _photoStore.IQueryableListAsync();
                item.Files = await img.Where(y => y.ProductId == item.Id && !y.IsDeleted).Select(y => y.PhotoUrl).ToListAsync(cancellationToken);
            });
            response.PageIndex = search.PageIndex;
            response.PageSize = search.PageSize;
            response.Extension = data;
            return response;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> ProductAddAsync(ProductEditRequest editRequest)
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
                    var Product = _mapper.Map<Product>(editRequest);
                    //新增图片
                    var images = new List<Photo> { };
                    editRequest.Files.ForEach(img =>
                    {
                        images.Add(new Photo
                        {
                            IsDeleted = false,
                            Id = Guid.NewGuid().ToString(),
                            PhotoUrl = img,
                            ProductId = editRequest.Id
                        });
                    });
                    await _photoStore.AddRangeEntityAsync(images);
                    response.Extension = await _ProductStore.AddEntityAsync(Product);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
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
        public async Task<ResponseMessage<bool>> ProductUpdateAsync(ProductEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            var Product = _mapper.Map<Product>(editRequest);
            using (var transaction = await _transaction.BeginTransaction())
            {
                try
                {
                    //存在修改图片 先判断原来是否有图片  存在判断是否有修改 对比异常 删除原理和新增现有数据？
                    var photos = await _photoStore.IQueryableListAsync();
                    var oldfile = await photos.Where(item => !item.IsDeleted && item.ProductId == editRequest.Id).Select(img => img.PhotoUrl).ToListAsync();//1,5,4,3
                    var newfile = editRequest.Files; //1,2,3,4     
                     //求差集 TODO 待测试
                    var except = oldfile.Except(newfile).ToList(); //2
                    if (except.Any())
                    {
                        var photo = new List<Photo>() { };
                        except.ForEach(file =>
                           {
                               photo.Add(new Photo
                               {
                                   Id = Guid.NewGuid().ToString(),
                                   IsDeleted = false,
                                   PhotoUrl = file,
                                   ProductId = editRequest.Id
                               });
                           });
                        await _photoStore.AddRangeEntityAsync(photo);
                    }
                    if (await _ProductStore.PutEntityAsync(Product.Id, Product))
                    {
                        response.Extension = true;
                    }
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
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
