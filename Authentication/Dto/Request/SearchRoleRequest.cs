using ZapiCore;
namespace Authentication.Dto.Request
{
    public class SearchRoleRequest : LayuiTableRequest
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }


    }
}
