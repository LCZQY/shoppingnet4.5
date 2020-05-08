using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Dto.Response
{
    public class RoleListResponse
    {
        /// <summary>
        /// KEY
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否是已授权的角色
        /// </summary>
        public bool? IsAuthorize { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 用逗号隔开，权限名称
        /// </summary>
        public string AuthorizeName { get; set; }

        /// <summary>
        /// 权限项分组名称
        /// </summary>
        public string Group { get; set; }

    }




}
