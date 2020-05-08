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
        /// <param name="role_Role_Permissionitems"></param>
        /// <returns></returns>
        Task<bool> DeleteRangeAsync(List<Role_Permissionitem> role_Role_Permissionitems);


    }
}
