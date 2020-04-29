namespace Authentication.Dto.Request
{
    public class RoleEditRequest
    {

        /// <summary>
        /// KEY
        /// </summary>
        public string Id { get; set; }

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
