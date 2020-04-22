using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApi.Models
{
    /// <summary>
    ///商品图片表
    /// </summary>
    public class Files : BaseField
    {
        /// <summary>
        /// 是否封面
        /// </summary>
        public bool IsIcon { get; set; }
            
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url { get; set; }

    }
}
