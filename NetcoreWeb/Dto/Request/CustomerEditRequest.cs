using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Dto.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerEditRequest
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { get; set; }


        /// <summary>
        /// 用户收获地址ID
        /// </summary>
        public string DeliveryId { get; set; }
    }
}
