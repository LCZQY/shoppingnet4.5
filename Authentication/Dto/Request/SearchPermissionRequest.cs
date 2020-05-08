using ZapiCore;
namespace Authentication.Dto.Request
{
    public class SearchPermissionRequest : LayuiTableRequest
    {
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

    }
}
