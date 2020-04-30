using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapiCore.Interface;
using Authentication.Models;
namespace Authentication.Stores
{
    public interface IPermissionStore : Baseinterface<Permissionitem>
    {
    

        /// <summary>
        /// 批量删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteRangeAsync(List<string> id);

        /// <summary>
        /// 获取权限扩展表
        /// </summary>
        /// <returns></returns>

        IQueryable<Permissionitem_expansion> Permissionitem_Expansions();

    }
}
