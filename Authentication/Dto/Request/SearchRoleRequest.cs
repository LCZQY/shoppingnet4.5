using ZapiCore;
namespace Authentication.Dto.Request
{
    public class SearchRoleRequest : PageCondition
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
    }
}
