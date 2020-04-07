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
        private readonly ICustomerStore _customerStore;
        private readonly ILogger<ProdoctManager> _logger;
        public ProdoctManager(ICustomerStore customerStore, ILogger<ProdoctManager> logger, IMapper mapper )
        {
            _customerStore = customerStore;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagingResponseMessage<ProductListResponse>> ProductListResponse(SearchCustomerRequest search, CancellationToken cancellationToken)
        {
            var response = new PagingResponseMessage<ProductListResponse>() { };
            var entity = await _customerStore.IQueryableListAsync();
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                entity.Where(y => y.Name == search.Name);
            }
            var list = await entity.Skip(search.PageIndex * search.PageSize).Take(search.PageSize).ToListAsync(cancellationToken);
            response.PageIndex = search.PageIndex;
            response.PageSize = search.PageSize;
            response.Extension = list;
            return response;
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> CustomerAddAsync(CustomerEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (editRequest == null)
            {
                throw new ArgumentNullException();
            }
            var customer = _mapper.Map<Customer>(editRequest);
            response.Extension = await _customerStore.AddEntityAsync(customer);
            return response;
        }



        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExists(string id)
        {
            if (_customerStore.IsExists(id))
                return true;
            return false;
        }



        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="editRequest"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> CustomerUpdateAsync(CustomerEditRequest editRequest)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            var customer = _mapper.Map<Customer>(editRequest);
            if (await _customerStore.PutEntityAsync(customer.Id, customer))
            {
                response.Extension = true;
            }
            return response;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> CustomerDeleteAsync(string id)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (await _customerStore.DeleteAsync(id))
            {
                response.Extension = true;
            }
            return response;
        }


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> AddRanageAsync(List<Customer> customers, CancellationToken cancellationToken)
        {
            var response = new ResponseMessage<bool>() { Extension = false };
            if (customers.Count == 0)
            {
                throw new ArgumentNullException();
            }
            response.Extension = await _customerStore.AddRangeEntityAsync(customers);
            return response;
        }
    }
}
