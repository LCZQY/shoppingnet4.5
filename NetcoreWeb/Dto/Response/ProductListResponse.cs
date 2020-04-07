using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Dto.Response
{
    /// <summary>
    /// 商品列表
    /// </summary>
    public class ProductListResponse
    {

        public string Id { get; set; }

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
        /// 封面【由于这里只是一张图片，直接把图片冗余在本表中，避免一次Sql语句的查询！】
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 商品图片【包括封面图和其他图片】
        /// </summary>
        public List<string> Files {get;set;}

    }
}
