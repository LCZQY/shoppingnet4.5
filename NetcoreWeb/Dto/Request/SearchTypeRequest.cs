using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapiCore;
namespace ShoppingApi.Dto.Request
{
    /// <summary>
    /// 搜索
    /// </summary>
    public class SearchTypeRequest : PageCondition
    {
        /// <summary>
        /// 默认查询最顶级
        /// </summary>
        public string Parentid { get; set; } = "0";



    }
}
