using System;
using ZapiCore.Model;
namespace ShoppingApi.Models
{
    /// <summary>
    /// 商品表
    /// </summary>
    public class Product : BaseField
    {

        /// <summary>
        /// 商品类型名称
        /// </summary>
        public string CateName { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///分类id
        /// </summary>
        public string CateId { get; set; }

        /// <summary>
        /// 标记价格（市场价格）
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 本地价格（本站价格）
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品说明描述
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime PostTime { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int Stock { get; set; }
        

        /// <summary>
        /// 封面【商品有个多个图片，本字段记录封面图】
        /// </summary>
        public string Icon { get; set; }


    }

    public class ProductEx : Product
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 收藏时间 / 评价时间
        /// </summary>
        public DateTime FavoriDate { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        public string Content { get; set; }


    }
}
