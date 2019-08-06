﻿using System.Collections.Generic;
using DAL;
using Model;

/// <summary>
/// 逻辑处理层
/// </summary>
namespace BLL
{

 
    public class ProductService : IBaseServer<Product>
    {
        private ProductDal _infoDal = Common.CacheControl.Get<ProductDal>();


        

        public bool Add(Product model)
        {
            return _infoDal.AddProduct(model) > 0;
        }

        public bool Delete(string id)
        {
            return _infoDal.DeleteProduct(id) > 0;
        }

        public Product GetDeail(int id)
        {
            return _infoDal.GetDeail(id);
        }

        public List<Product> GetList()
        {
            return _infoDal.GetList();
        }

        public List<Product> GetList(int page, int index)
        {
            return _infoDal.GetList(page, index);
        }

        public bool Update(Product model)
        {
            return _infoDal.UpdateProduct(model) > 0;
        }
    }
}
