using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Dto.Response
{
    public class UserListResponse
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 密码：默认123456
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}
