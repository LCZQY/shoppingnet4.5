using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;

/// <summary>
/// 逻辑处理层
/// </summary>
namespace BLL
{

 
    public class ProductService : IBaseServer<Product>
    {
        private ProductDal _infoDal = new ProductDal { }; //Common.CacheControl.Get<ProductDal>();s
        private ProductDal _infoProductDal = new ProductDal();
        private PhotoDal _infoPhotoDal = new PhotoDal();


        /// <summary>
        ///  获取收藏商品
        /// </summary>
        public List<ProductEx> FavoriteProductList(List<string> productId)
        {


            var prolist = _infoProductDal.GetList().Where(y => productId.Contains(y.ProductId))?.ToList();
            var ex = from c in prolist
                     join b in _infoPhotoDal.GetList()?.ToList()
                      on c.ProductId equals b.ProductId
                     select new ProductEx
                     {

                         Content = c.Content,
                         CateId = c.CateId,
                         MarketPrice = c.MarketPrice,
                         Path = b.PhotoUrl == null ? "" : b.PhotoUrl,
                         Price = c.Price,
                         ProductId = c.ProductId,
                         Title = c.Title,
                         Stock = c.Stock
                     };
            return ex.ToList();

        }


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
