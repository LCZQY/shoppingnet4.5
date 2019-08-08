using System.Collections.Generic;
using DAL;
using Model;

namespace BLL
{
    public class NewsService : IBaseServer<News>
    {
        private NewsDal _infoDal = new NewsDal(); //Common.CacheControl.Get<NewsDal>();


        public bool Add(News model)
        {
            return _infoDal.AddNews(model) > 0;
        }

        public bool Delete(string id)
        {
            return _infoDal.DeleteNews(id) > 0;
        }

        public News GetDeail(int id)
        {
           // return _infoDal.GetDeail(id);
            return null;
        }

        public List<News> GetList()
        {
            return _infoDal.GetList();
        }

        public List<News> GetList(int page, int index)
        {
            return _infoDal.GetList(page, index);
        }

        public bool Update(News model)
        {
            return _infoDal.UpdateNews(model) > 0;
        }
    }
}
