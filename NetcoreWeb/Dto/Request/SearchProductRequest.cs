using System.ComponentModel.DataAnnotations;
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
        //[Required(ErrorMessage = "请输入商品名称")]
        public string Name { get; set; }

        /// <summary>
        /// 商品类型Id
        /// </summary>
        //[Required(ErrorMessage = "请输入商品类型Id")]
        public string CateId { get; set; }
    }
}
