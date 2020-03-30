using System;

/// <summary>
/// 数据模型层
/// </summary>
namespace ShoppingApi.Models
{
    /// <summary>
    /// 商品评价表
    /// </summary>
    public class Appraise : BaseField
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 评价等级 【0：很好，1：好，2：不好】
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 评价时间
        /// </summary>
        public DateTime RateTime { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

    }

    /// <summary>
    /// 评价等级
    /// </summary>
    public enum EnumGrade
    {
        /// <summary>
        /// 很好
        /// </summary>
        VeryGood = 0,
        /// <summary>
        /// 好
        /// </summary>
        Good = 1,
        /// <summary>
        /// 不好
        /// </summary>
        Bad = 2
    }

}
