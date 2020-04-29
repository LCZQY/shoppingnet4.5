using ZapiCore.Model;
namespace Authentication.Models
{
    /// <summary>
    ///  用户角色表
    /// </summary>
    public class User_Role : BaseField
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>

        public string RoleId { get; set; }


    }
}
