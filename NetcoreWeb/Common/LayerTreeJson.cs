using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Common
{

    /// <summary>
    /// layer 树形数据绑定指定返回体
    /// </summary>
    public class LayerTreeJson
    {

        /// <summary>
        /// 文本
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 兼容选择树响应数据类型
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<LayerTreeJson> Children { get; set; }

    }

    /// <summary>
    /// layer 表格数据绑定指定返回体
    /// </summary>
    public class LayerTableJson
    {
        /// <summary>
        /// 行数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = 0;
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; } = "请求成功";
        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { get; set; }

    }
}
