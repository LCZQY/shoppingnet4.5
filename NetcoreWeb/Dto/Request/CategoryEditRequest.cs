using System.ComponentModel.DataAnnotations;

namespace ShoppingApi.Dto.Request
{
    /// <summary>
    /// 商品类型编辑请求体
    /// </summary>
    public class CategoryEditRequest
    {
        /// <summary>
        /// key
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Required(ErrorMessage ="类型名称不能够为空")]
        public string CateName { get; set; }

        /// <summary>
        /// 上级类别编号
        /// </summary>
        //[Required(ErrorMessage ="父级Id不能够为空")]
        public string ParentId { get; set; }


    }
}
