using System.Collections.Generic;

namespace Authentication.Dto.Response
{
    /// <summary>
    /// 权限列表返回提
    /// </summary>
    public class PermissionListResponse
    {


        /// <summary>
        /// 权限项分组名称
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 权限分组下的权限项
        /// </summary>
        public List<ListResponse> PermissionList { get; set; }

        /// <summary>
        /// 权限项
        /// </summary>
        public class ListResponse

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


        }
    }
}
