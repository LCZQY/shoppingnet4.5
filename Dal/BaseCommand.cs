using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DAL
{
    /// <summary>
    /// 基础封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface BaseCommand<T> where T : class
    {

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        List<T> GetList(string sql);



        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <returns></returns>
        List<T> GetList(string sql, int page, int index);

        /// <summary>
        /// 获取一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetDeail(string sql, int id);


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        int AddCustomer(T model, string sql);


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        int UpdateCustomer(T model);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteCustomer(string sql, string id);

    }


}
