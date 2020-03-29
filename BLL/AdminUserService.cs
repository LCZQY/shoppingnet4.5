using DAL;
using Model;
using System;
using System.Collections.Generic;

/// <summary>
/// 逻辑处理层
/// </summary>
namespace BLL
{
    /// <summary>
    ///  管理员表
    /// </summary>
    public class AdminCustomerervice : IBaseServer<AdminUser>
    {
        private AdminUserDal _infoDal = new AdminUserDal();// Common.CacheControl.Get<AdminUserDal>();




        public bool Add(AdminUser model)
        {
            return _infoDal.AddAdminUser(model) > 0;
        }

        public bool Delete(string id)
        {
            return _infoDal.DeleteAdminUser(id) > 0;
        }

        public AdminUser GetDeail(int id)
        {
            throw new Exception();
        }

        public List<AdminUser> GetList()
        {
            return _infoDal.GetList();
        }

        public List<AdminUser> GetList(int page, int index)
        {
            return _infoDal.GetList(page, index);
        }

        public bool Update(AdminUser model)
        {
            return _infoDal.UpdateAdminUser(model) > 0;
        }
    }
}
