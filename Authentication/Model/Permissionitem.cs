using ZapiCore.Model;
namespace Authentication
{
    /// <summary>
    ///  管理员表
    /// </summary>
    public class Permissionitem : BaseField
    {
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


    }
}
