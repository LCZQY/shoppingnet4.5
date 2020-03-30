using System;

namespace ShoppingApi.Models
{
    /// <summary>
    /// 收藏夹表
    /// </summary>
    public class Favorite : BaseField
    {



        /// <summary>
        /// 商品id
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime DateTime { get; set; }


    }
}
