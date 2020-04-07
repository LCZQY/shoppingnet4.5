using AutoMapper;
using ShoppingApi.Dto.Request;
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
            CreateMap<CustomerEditRequest, Customer>();
            CreateMap<Customer, CustomerEditRequest>();
        }
    }
}
