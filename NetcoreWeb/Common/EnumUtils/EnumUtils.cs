using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Common.EnumUtils
{
    /// <summary>
    /// 商品状态 （考虑中）
    /// </summary>
    public enum ProdoctType
    {

        /// <summary>
        /// 上架
        /// </summary>
        UpperShelf = 1,

        /// <summary>
        /// 下架
        /// </summary>
        LowerShelf = 2
    }
}
