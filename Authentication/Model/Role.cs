using ZapiCore.Model;
namespace Authentication
{
    /// <summary>
    ///  管理员表
    /// </summary>
    public class Role : BaseField
    {

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
