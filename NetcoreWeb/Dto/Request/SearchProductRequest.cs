using ZapiCore;

namespace ShoppingApi.Dto.Request
{
    /// <summary>
    /// 商品列表搜索
    /// </summary>
    public class SearchProductRequest : PageCondition
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
    }
}
