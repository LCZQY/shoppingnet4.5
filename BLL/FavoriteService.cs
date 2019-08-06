﻿using System.Collections.Generic;
using DAL;
using Model;

namespace BLL
{

    /// <summary>   
    ///收藏
    /// </summary>
    public class FavoriteService : IBaseServer<Favorite>
    {
        private FavoriteDal _infoDal = Common.CacheControl.Get<FavoriteDal>();


        public bool Add(Favorite model)
        {
            return _infoDal.AddFavorite(model) > 0;
        }

        public bool Delete(string id)
        {
            return _infoDal.DeleteFavorite(id) > 0;
        }

        public Favorite GetDeail(int id)
        {
            return _infoDal.GetDeail(id);
        }

        public List<Favorite> GetList()
        {
            return _infoDal.GetList();
        }

        public List<Favorite> GetList(int page, int index)
        {
            return _infoDal.GetList(page, index);
        }

        public bool Update(Favorite model)
        {
            return _infoDal.UpdateFavorite(model) > 0;
        }
    }
}
