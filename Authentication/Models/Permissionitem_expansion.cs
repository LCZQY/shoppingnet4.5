using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapiCore.Model;

namespace Authentication.Models
{
    /// <summary>
    /// 权限扩展表
    /// </summary>
    public class Permissionitem_expansion : BaseField
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 权限code
        /// </summary>
        public string PermissionCode { get; set; }

    }
}
