using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapiCore.Model;

namespace Authentication.Models
{
    /// <summary>
    /// 角色-权限表
    /// </summary>
    public class Role_Permissionitem : BaseField
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public string PermissionId { get; set; }

    }
}
