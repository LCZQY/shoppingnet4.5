using System.ComponentModel.DataAnnotations;

namespace NetcoreWeb.Models
{
    /// <summary>
    /// 模型基类
    /// </summary>
    public class Base
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string Id { get; set; }

    }
}
