using System;
using System.Collections.Generic;
using Common;
using DAL;
using System.Linq;
using Model;

namespace BLL
{
    public class OrderService : IBaseServer<Orders>
    {
        private OrdersDal _infoOrderDal = new OrdersDal(); 
        private UserInfoDal _infoUserDal = new UserInfoDal();
        private OrdersDetailDal _infoDetailDal = new OrdersDetailDal();
        private DeliveryService _infoDeliveryDal = new DeliveryService();
        private ProductDal _infoProductDal = new ProductDal(); 
        private PhotoDal _infoPhotoDal = new PhotoDal();
        private FavoriteDal _infoFavoriteDal = new FavoriteDal();



      


        /// <summary>
        ///  我的购物车购物车数据
        /// </summary>
        public List<OrdersDetailExtend> OrderCartList()
        {



            try
            {
                var list = from c in _infoOrderDal.GetList().ToList()
                           join b in _infoDetailDal.GetList().ToList()
                           on c.OrdersId equals b.OrdersId
                           join u in _infoProductDal.GetList().ToList()
                           on b.ProductId equals u.ProductId 
                           select new OrdersDetailExtend
                           {
                               //订单
                               OrdersId = c.OrdersId,
                               ProductId = b.ProductId,
                               Orderdate = c.Orderdate,
                               UserId = c.UserId,
                               Title = u.Title,
                               Price = u.Price,
                               Total = c.Total,
                               DeliveryDate = c.DeliveryDate,
                               States = c.States,
                               Remark = c.Remark,
                               //订单详情
                               Path = _infoPhotoDal.GetList().Where(y => y.ProductId == (b.ProductId == null ? "" : b.ProductId)).FirstOrDefault().PhotoUrl ?? "",
                               DetailId = b.DetailId,
                               Quantity = b.Quantity,
                               DetailStates = b.States,
                           };

                return list.ToList();// ?? new List<OrdersDetailExtend> { };
            }
            catch (Exception e) {
                throw;
            }
        }
            /// <summary>
            ///  连表查询订单信息
            /// </summary>
            public List<OrdersDetailExtend> OrderDetailJoinList()
        {
            try
            {
                var list = from c in _infoOrderDal.GetList().AsQueryable()
                           join b in _infoDetailDal.GetList().AsQueryable()
                           on c.OrdersId equals b.OrdersId

                           join h in _infoDeliveryDal.GetList().AsQueryable()
                           on c.DeliveryId equals h.DeliveryId

                           join y in _infoProductDal.GetList().AsQueryable()
                           on b.ProductId equals y.ProductId into b1
                           from c1 in b1.DefaultIfEmpty()

                           join j in _infoUserDal.GetList().AsQueryable()
                           on c.UserId equals j.UserId into u
                           from u1 in u.DefaultIfEmpty()
                           select new OrdersDetailExtend
                           {
                              // 订单
                               OrdersId = c.OrdersId,
                               Orderdate = c.Orderdate,

                               Total = c.Total,
                               DeliveryDate = c.DeliveryDate,
                               States = c.States,
                               Remark = c.Remark,
                               //订单详情
                               DetailId = b.DetailId,
                               Quantity = b.Quantity,
                               DetailStates = b.States,

                               //商品
                               ProductId = c1.ProductId,
                               Title = c1.Title,
                               //Price = c1.Price,

                               //用户
                               UserId = u1.UserId,
                               //UserName = u1.UserName,
                               Nick = u1.Nick,

                               //收货地址
                               Complete = h.Complete,
                               Consignee = h.Consignee,
                               DeliveryId = h.DeliveryId,
                               Phone = h.Phone
                           };
                var response = list== null ? new List<OrdersDetailExtend> { }: list.ToList();
                return response;
            } catch (Exception e)
            {

                return new List<OrdersDetailExtend> { };
                //throw;
            }
        }


        public bool Add(Orders model)
        {
            return _infoOrderDal.AddOrders(model) > 0;
        }

        public bool Delete(string id)
        {
            return _infoOrderDal.DeleteOrders(id) > 0;
        }

        public Orders GetDeail(int id)
        {
            return _infoOrderDal.GetDeail(id);
        }

        public List<Orders> GetList()
        {
            return _infoOrderDal.GetList();
        }



        public List<Orders> GetList(int page, int index)
        {
            return _infoOrderDal.GetList(page, index);
        }

        public bool Update(Orders model)
        {
            return _infoOrderDal.UpdateOrders(model) > 0;
        }
    }
}
