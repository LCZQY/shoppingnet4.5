using ZapiCore.Model;
namespace Authentication.Models
{
    /// <summary>
    ///  管理员表
    /// </summary>
    public class User : BaseField
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
    }
}
