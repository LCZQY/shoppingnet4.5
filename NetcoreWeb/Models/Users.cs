/// <summary>
/// 数据模型层
/// </summary>
namespace NetcoreWeb.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class Users : Base
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

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
