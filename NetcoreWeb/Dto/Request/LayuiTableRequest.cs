namespace ShoppingApi.Dto.Request
{
    /// <summary>
    /// 兼容Layui得表格分页请求参数
    /// </summary>
    public class LayuiTableRequest
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int? Limit { get; set; }


        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品类型Id
        /// </summary>
        public string CateId { get; set; }

    }
}
