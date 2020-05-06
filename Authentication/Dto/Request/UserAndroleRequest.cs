using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage ="必填项不能为空")]
        public string UserId { get; set; }

        
            
        /// <summary>
        /// 角色id
        /// </summary>
        [Required(ErrorMessage ="必填项不能为空")]
        public List<string> RoleId { get; set; }
    }
}
