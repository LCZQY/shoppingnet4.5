using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApi.Dto.Request
{
    /// <summary>
    /// 商品编辑请求体
    /// </summary>
    public class ProductEditRequest
    {


        /// <summary>
        /// 主键
        /// </summary>

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
        [DataType(DataType.Currency, ErrorMessage = "价格数据类型错误")]
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 本地价格（本站价格）
        /// </summary>
        [DataType(DataType.Currency, ErrorMessage = "价格数据类型错误")]
        public decimal Price { get; set; }

        /// <summary>
        /// 商品说明描述
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        [DataType(DataType.DateTime, ErrorMessage = "时间数据类型错误")]
        public DateTime PostTime { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        [DataType(DataType.Currency, ErrorMessage = "库存数量类型错误")]
        public int Stock { get; set; }


        /// <summary>
        /// 封面【商品有个多个图片，本字段记录封面图】
        /// </summary>
        public string Icon { get; set; }


        /// <summary>
        /// 图片
        /// </summary>
        public List<FileRequest> Files { get; set; }
    }



}
