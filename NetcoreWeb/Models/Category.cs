namespace ShoppingApi.Models
{
    /// <summary>
    /// 商品类别表
    /// </summary>
    public class Category : BaseField
    {


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
