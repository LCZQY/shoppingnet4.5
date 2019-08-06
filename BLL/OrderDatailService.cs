using System.Collections.Generic;
using DAL;
using Model;

namespace BLL
{

    public class DetailService : IBaseServer<Detail>
    {
        private OrdersDetailDal _infoDal = new OrdersDetailDal(); //Common.CacheControl.Get<DetailDal>();

        public bool Add(Detail model)
        {
            return _infoDal.AddDetail(model) > 0;
        }

        public bool Delete(string id)
        {
            return _infoDal.DeleteDetail(id) > 0;
        }

        public Detail GetDeail(int id)
        {
            return _infoDal.GetDeail(id);
        }

        public List<Detail> GetList()
        {
            return _infoDal.GetList();
        }

        public List<Detail> GetList(int page, int index)
        {
            return _infoDal.GetList(page, index);
        }

        public bool Update(Detail model)
        {
            return _infoDal.UpdateDetail(model) > 0;
        }
    }
}
