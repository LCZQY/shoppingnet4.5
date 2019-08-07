using DAL;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class AppraiseService : IBaseServer<Appraise>
    {
        private AppraiseDal _infoDal = new AppraiseDal(); //Common.CacheControl.Get<AppraiseDal>();


        public bool Add(Appraise model)
        {
            return _infoDal.AddAppraise(model) > 0;
        }

        public bool Delete(string id)
        {
            return _infoDal.DeleteAppraise(id) > 0;
        }

        public Appraise GetDeail(int id)
        {
            return _infoDal.GetDeail(id);
        }

        public List<Appraise> GetList()
        {
            return _infoDal.GetList();
        }

        public List<Appraise> GetList(int page, int index)
        {
            return _infoDal.GetList(page, index);
        }

        public bool Update(Appraise model)
        {
            return _infoDal.UpdateAppraise(model) > 0;
        }
    }
}
