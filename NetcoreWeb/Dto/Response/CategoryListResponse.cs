namespace ShoppingApi.Dto.Response
{
    /// <summary>
    /// 商品类型返回体
    /// </summary>
    public class CategoryListResponse
    {
        /// <summary>
        /// key
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 类别名
        /// </summary>
        public string CateName { get; set; }

        /// <summary>
        /// 上级类别编号
        /// </summary>
        public string ParentId { get; set; }
    }
}
