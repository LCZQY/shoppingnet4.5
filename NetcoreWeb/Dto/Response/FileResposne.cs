namespace ShoppingApi.Dto.Response
{
    /// <summary>
    /// 文件响应体
    /// </summary>
    public class FileResposne
    {
        /// <summary>
        /// 主键 
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 是否封面
        /// </summary>
        public bool IsIcon { get; set; }


    }
}
