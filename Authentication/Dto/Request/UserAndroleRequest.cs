using System.Collections.Generic;

namespace Authentication.Dto.Request
{

    /// <summary>
    /// 绑定用户角色请求体
    /// </summary>
    public class UserAndroleRequest
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        
            
        /// <summary>
        /// 角色id
        /// </summary>
        public List<string> RoleId { get; set; }
    }
}
