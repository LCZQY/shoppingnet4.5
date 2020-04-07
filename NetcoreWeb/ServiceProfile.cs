using AutoMapper;
using ShoppingApi.Dto.Request;
using ShoppingApi.Dto.Response;
using ShoppingApi.Models;

namespace ShoppingApi
{

    /// <summary>
    /// autoMapper
    /// </summary>
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            #region request
            CreateMap<CustomerEditRequest, Customer>();
            CreateMap<Customer, CustomerEditRequest>();
            #endregion

            #region Resposne
            CreateMap<ProductListResponse, Product>();
            CreateMap<Product, ProductListResponse>();
            #endregion
        }
    }
}
