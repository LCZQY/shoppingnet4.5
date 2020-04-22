namespace ShoppingApi.Dto.Request
{

    /// <summary>
    /// 文件上传请求体
    /// </summary>
    public class FileRequest
    {
        /// <summary>
        /// 主键 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否封面
        /// </summary>
        public bool IsIcon { get; set; }
    }
}
