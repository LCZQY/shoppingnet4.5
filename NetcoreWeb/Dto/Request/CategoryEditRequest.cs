namespace ShoppingApi.Dto.Request
{
    /// <summary>
    /// 商品类型编辑请求体
    /// </summary>
    public class CategoryEditRequest
    {
        /// <summary>
        /// key
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string CateName { get; set; }

        /// <summary>
        /// 上级类别编号
        /// </summary>
        public string ParentId { get; set; }


    }
}
