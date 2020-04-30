using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapiCore.Interface;
using Authentication.Models;
namespace Authentication.Stores
{
    public interface IRolePermissionStore : Baseinterface<Role_Permissionitem>
    {
    

        /// <summary>
        /// 批量删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteRangeAsync(List<string> id);


    }
}
