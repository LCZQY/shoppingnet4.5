using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Dto.Request
{
    /// <summary>
    ///  角色绑定权限请求体
    /// </summary>
    public class RoleAndPermissionRequest
    {

        /// <summary>
        /// 角色Id        
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public List<string> ListPermissionId { get; set; }

    }
}
