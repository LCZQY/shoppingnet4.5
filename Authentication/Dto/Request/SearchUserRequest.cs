using ZapiCore;
namespace Authentication.Dto.Request
{

    public class SearchUserRequest : PageCondition
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
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }


        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
