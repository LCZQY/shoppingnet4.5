using DAL;
using Model;
using System.Collections.Generic;

/// <summary>
/// 逻辑处理层
/// </summary>
namespace BLL
{

    public class UserInfoService
    {
        UserInfoDal UserInfoDal = new UserInfoDal();// Common.CacheControl.Get<UserInfoDal>();


        /// <summary>
        /// 返回数据列表
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetList()
        {
            return UserInfoDal.GetList();
        }

        /// <summary>
        /// 返回数据列表
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetList(int page, int index)
        {
            return UserInfoDal.GetList(page, index);
        }
        /// <summary>
        /// 返回用户信息 一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer GetDeail(int id)
        {
            return UserInfoDal.GetDeail(id);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool AddUserInfo(Customer userInfo)
        {
            return UserInfoDal.AddCustomer(userInfo) > 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(Customer userInfo)
        {
            return UserInfoDal.UpdateCustomer(userInfo) > 0;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(string id)
        {
            return UserInfoDal.DeleteCustomer(id) > 0;
        }
    }
}
