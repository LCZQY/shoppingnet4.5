using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Dto.Request
{
    /// <summary>
    /// Layui表格数据请求体
    /// </summary>
    public class LayuiTableRequest
    {

        /// <summary>
        /// 页数
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int? Limit { get; set; }

    }
}
