using ZapiCore.Model;
namespace ShoppingApi.Models
{

    /// <summary>
    /// 收货地址表
    /// </summary>
    public class Delivery : BaseField
    {

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Complete { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }



    }
}
