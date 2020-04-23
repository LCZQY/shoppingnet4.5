//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using ShoppingApi.Common;
//using ShoppingApi.Dto.Request;
//using ShoppingApi.Dto.Response;
//using ShoppingApi.Models;
//using ShoppingApi.Stores.Interface;
//using ZapiCore;
//namespace ShoppingApi.Managers
//{


//    /// <summary>
//    /// 订单管理逻辑处理
//    /// </summary>
//    public class OrdersManager
//    {
//        private readonly IMapper _mapper;
//        private readonly IOrdersStore  _iordersStore;
//        private readonly IFilesStore _filesStore;
//        private readonly ILogger<OrdersManager> _logger;
//        private readonly ITransaction<ShoppingDbContext> _transaction;
//        public OrdersManager(IOrdersStore  ordersStore, ILogger<OrdersManager> logger, IMapper mapper, IFilesStore photoStore, ITransaction<ShoppingDbContext> transaction)
//        {
//            _iordersStore = ordersStore;
//            _filesStore = photoStore;
//            _logger = logger;
//            _mapper = mapper;
//            _transaction = transaction;
//        }


  

//        /// <summary>
//        /// 列表数据
//        /// </summary>
//        /// <param name="search"></param>
//        /// <param name="cancellationToken"></param>
//        /// <returns></returns>
//        public async Task<PagingResponseMessage<OrdersListResponse>> OrdersListAsync(SearchOrdersRequest search, CancellationToken cancellationToken)
//        {
//            var response = new PagingResponseMessage<OrdersListResponse>() { };
//            var entity = _OrdersStore.IQueryableListAsync();
//            if (!string.IsNullOrWhiteSpace(search.Name))
//            {
//                entity = entity.Where(y => y.Title.Contains(search.Name));
//            }
//            if (!string.IsNullOrWhiteSpace(search.CateId))
//            {
//                entity = entity.Where(y => y.CateId == search.CateId);
//            }
//            var list = await entity.Skip(search.PageIndex * search.PageSize).Take(search.PageSize).ToListAsync(cancellationToken);
//            var data = _mapper.Map<List<OrdersListResponse>>(list); 
//            response.PageIndex = search.PageIndex;
//            response.PageSize = search.PageSize;
//            response.Extension = data;
//            return response;
//        }


//        /// <summary>
//        /// 详情
//        /// </summary>
//        /// <param name="id"></param>
//        /// <param name="cancellationToken"></param>
//        /// <returns></returns>
//        public async Task<ResponseMessage<OrdersListResponse>> OrdersDetailsAsync(string id, CancellationToken cancellationToken)
//        {
//            var response = new ResponseMessage<OrdersListResponse>() { Extension = new OrdersListResponse { } };
//            var entity =await _iordersStore.GetAsync(id);                       
//            var data = _mapper.Map<OrdersListResponse>(entity);
//            data.Files = await _filesStore.IQueryableListAsync().Where(y => id == y.OrdersId && !y.IsDeleted).Select(y => y.Url).ToListAsync(cancellationToken);                                  
//            response.Extension = data;
//            return response;
//        }

//        /// <summary>
//        /// 新增
//        /// </summary>
//        /// <param name="editRequest"></param>
//        /// <returns></returns>
//        public async Task<ResponseMessage<bool>> OrdersAddAsync(OrdersEditRequest editRequest, CancellationToken cancellationToken)
//        {
//            var response = new ResponseMessage<bool>() { Extension = false };
//            if (editRequest == null)
//            {
//                throw new ArgumentNullException();
//            }
//            using (var transaction = await _transaction.BeginTransaction())
//            {
//                try
//                {
//                    var Orders = _mapper.Map<Orders>(editRequest);
//                    Orders.Id = Guid.NewGuid().ToString();
//                    Orders.Icon = editRequest.Files.Where(item => item.IsIcon).SingleOrDefault().Url;
//                    //新增图片
//                    var images = new List<Files> { };
//                    editRequest.Files.ForEach(img =>
//                    {
//                        images.Add(new Files
//                        {
//                            IsDeleted = false,
//                            Id = Guid.NewGuid().ToString(),
//                            Url = img.Url,
//                            IsIcon = img.IsIcon,
//                            OrdersId = Orders.Id
//                        });
//                    });
//                    await _filesStore.AddRangeEntityAsync(images);
//                    response.Extension = await _iordersStore.AddEntityAsync(Orders);
//                   await transaction.CommitAsync(cancellationToken);
//                }
//                catch (Exception e)
//                {
//                    await transaction.RollbackAsync(cancellationToken);
//                    throw e;
//                }
//            }
//            return response;
//        }



//        /// <summary>
//        /// 是否存在
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public async Task<bool> IsExists(string id)
//        {
//            if (_iordersStore.IsExists(id))
//                return true;
//            return false;
//        }

//        /// <summary>
//        /// 修改
//        /// </summary>
//        /// <param name="editRequest"></param>
//        /// <returns></returns>
//        public async Task<ResponseMessage<bool>> OrdersUpdateAsync(OrdersEditRequest editRequest, CancellationToken cancellationToken)
//        {
//            var response = new ResponseMessage<bool>() { Extension = false };
//            var Orders = _mapper.Map<Orders>(editRequest);
//            using (var transaction = await _transaction.BeginTransaction())
//            {
                
//                try
//                {
//                    //存在修改图片 先判断原来是否有图片  存在判断是否有修改 对比异常 删除原理和新增现有数据？               
//                    var oldfile = await _filesStore.IQueryableListAsync().Where(item => !item.IsDeleted && item.OrdersId == editRequest.Id).Select(img => img.Url).ToListAsync();//1,5,4,3
//                    var newfile = editRequest.Files.Select(y => y.Url); //1,2,3,4     
//                                                                         //求差集 TODO 待测试
//                    var except = oldfile.Except(newfile).ToList(); //2
//                    if (except.Any())
//                    {
//                        var photo = new List<Files>() { };
//                        except.ForEach(file =>
//                           {
//                               photo.Add(new Files
//                               {
//                                   Id = Guid.NewGuid().ToString(),
//                                   IsDeleted = false,
//                                   Url = file,
//                                   OrdersId = editRequest.Id
//                               });
//                           });
//                        await _filesStore.AddRangeEntityAsync(photo);
//                    }
//                    if (await _iordersStore.PutEntityAsync(Orders.Id, Orders))
//                    {
//                        response.Extension = true;
//                    }
//                    await transaction.CommitAsync(cancellationToken);
//                }
//                catch (Exception e)
//                {
//                    await transaction.RollbackAsync(cancellationToken);
//                    throw e;
//                }
//            }
//            return response;
//        }

//        /// <summary>
//        /// 删除
//        /// </summary>
//        /// <param name="id"></param>        
//        /// <returns></returns>
//        public async Task<ResponseMessage<bool>> OrdersDeleteAsync(string id)
//        {
//            var response = new ResponseMessage<bool>() { Extension = false };
//            if (await _iordersStore.DeleteAsync(id))
//            {
//                response.Extension = true;
//            }
//            return response;
//        }


//        /// <summary>
//        /// 批量新增
//        /// </summary>
//        /// <param name="Orderss"></param>
//        /// <param name="cancellationToken"></param>
//        /// <returns></returns>
//        public async Task<ResponseMessage<bool>> AddRanageAsync(List<Orders> Orderss, CancellationToken cancellationToken)
//        {
//            var response = new ResponseMessage<bool>() { Extension = false };
//            if (Orderss.Count == 0)
//            {
//                throw new ArgumentNullException();
//            }
//            response.Extension = await _iordersStore.AddRangeEntityAsync(Orderss);
//            return response;
//        }
//    }
//}
