﻿using AutoMapper;
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

            CreateMap<Category, CategoryEditRequest>();
            CreateMap<CategoryEditRequest, Category>();

            CreateMap<Product, ProductEditRequest>();
            CreateMap<ProductEditRequest, Product>();

            CreateMap<Customer, CustomerEditRequest>();
            CreateMap<CustomerEditRequest, Customer>();
            #endregion

            #region Resposne
            CreateMap<ProductListResponse, Product>();
            CreateMap<Product, ProductListResponse>();

            CreateMap<CategoryListResponse, Category>();
            CreateMap<Category, CategoryListResponse>();
            #endregion
        }
    }
}
