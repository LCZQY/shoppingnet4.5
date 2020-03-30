using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingApi.Dto.Request;
using ShoppingApi.Models;
using ShoppingApi.Stores.Interface;
using ZapiCore;
namespace ShoppingApi.Managers
{
    /// <summary>
    /// 客户管理逻辑处理
    /// </summary>
    public class CustomerManager
    {
        private readonly ICustomerStore _customerStore;
        private readonly ILogger<CustomerManager> _logger;
        public CustomerManager(ICustomerStore customerStore, ILogger<CustomerManager> logger)
        {
            _customerStore = customerStore;
            _logger = logger;        
        }



        /// <summary>
        ///  登陆【之后统一采用权限认证方式， IdentityServer4 】
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<bool>> LoginJudgeAsync(string name, string pwd)
        {
            var response = new ResponseMessage<bool>() { Extension = false};
            var customer = await _customerStore.IQueryableListAsync();
            if (customer.Where(y => y.Name == name && y.Pwd == pwd).Any())
            {
                response.Extension = true;               
            }
            return response;
        }
        
        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PagingResponseMessage<Customer>> CustomerListAsync(SearchCustomerRequest  search)
        {
            var response = new PagingResponseMessage<Customer>() { };
            var entity =await _customerStore.IQueryableListAsync();
            if(!string.IsNullOrWhiteSpace(search.Name))
            {
                entity.Where(y => y.Name == search.Name);
            }
            var list = await entity.Skip(search.PageIndex * search.PageSize).Take(search.PageSize).ToListAsync();
            response.PageIndex = search.PageIndex;
            response.PageSize = search.PageSize;
            response.Extension = list;
            return response;
        }


    }
}
