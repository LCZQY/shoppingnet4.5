
using ZapiCore.Model;
namespace ShoppingApi.Models
{
    /// <summary>
    /// 顾客
    /// </summary>
    public class Customer : BaseField
    {
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
