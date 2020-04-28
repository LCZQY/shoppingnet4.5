using System.ComponentModel.DataAnnotations.Schema;
/// <summary>
/// 数据模型层
/// </summary>
namespace ShoppingApi.Models
{
    /// <summary>
    /// 管理员表
    /// </summary>
    public class AdminUser : BaseField
    {
        /// <summary>
        /// 管理员账号
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public  string PhoneNumber { get; set; }

        /// <summary>
        /// 密码：默认123456
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// token
        /// </summary>
        [NotMapped]
        public string Token { get; set; }
    }


}
