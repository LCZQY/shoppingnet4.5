using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Dto.Response
{


    public class OrdersListResponse
    {

        #region 订单详情

        /// <summary>
        /// key
        /// </summary>
        public string DetailId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrdersId { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 明细状态
        /// </summary>
        public int? DetailStates { get; set; }
        #endregion

        #region 用户
        /// <summary>
        /// key
        /// </summary>
        public string UserId { get; set; }

        ///// <summary>
        ///// 用户名
        ///// </summary>
        //public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { get; set; }

        #endregion


        #region  商品


        /// <summary>
        /// 商品名
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 图片地址
        /// </summary>
        public string Path { get; set; }
        ///// <summary>
        /////分类id
        ///// </summary>
        //public string CateId { get; set; }

        ///// <summary>
        ///// 标记价格（市场价格）
        ///// </summary>
        //public decimal MarketPrice { get; set; }

        /// <summary>
        /// 本地价格（本站价格）
        /// </summary>
        public decimal Price { get; set; }

        ///// <summary>
        ///// 商品说明描述
        ///// </summary>
        //public string Content { get; set; }

        ///// <summary>
        ///// 上架时间
        ///// </summary>
        //public DateTime PostTime { get; set; }

        ///// <summary>
        ///// 库存数量
        ///// </summary>
        //public int Stock { get; set; }

        #endregion


        #region  卖家收货信息
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Complete { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }


        #endregion
    }
}
