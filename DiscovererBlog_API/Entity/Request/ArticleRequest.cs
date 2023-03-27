namespace DiscovererBlog_API.Entity.Request;

public class ArticleRequest
{
    /// <summary>
    /// 上传文章传入
    /// </summary>
    public class UploadArticle
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章内容 不传则使用Md_Content
        /// </summary>
        public string? Content { get; set; } = "";

        /// <summary>
        /// md语法的文章内容 不传则使用Content
        /// </summary>
        public string? Md_Content { get; set; } = "";

        /// <summary>
        /// 分类，用逗号分隔
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 文章创建时间 不传则使用服务器时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 文章更新时间 不传为当前时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 历史记录ID 不传为无历史记录
        /// </summary>
        public int? ArticleId { get; set; }
    }

    /// <summary>
    /// 删除文章传入
    /// </summary>
    public class DeleteArticle
    {
        /// <summary>
        /// 需要删除的文章ID
        /// </summary>
        public int ArticleId { get; set; }
    }

    /// <summary>
    /// 返回文章传入（用来查询文章和历史记录，这个接口需登录后使用，可查询到文章的历史记录）
    /// </summary>
    public class ReturnArticle
    {
        /// <summary>
        /// 需要删除的文章ID
        /// </summary>
        public int ArticleId { get; set; }
    }

    /// <summary>
    /// 删除历史记录传入
    /// </summary>
    public class DeleteHistory
    {
        /// <summary>
        /// 历史记录ID
        /// </summary>
        public int HistoryId { get; set; }
    }

    /// <summary>
    /// 查询文章列表传入
    /// </summary>
    public class QueryArticleList
    {
        /// <summary>
        /// 查询页数
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 查询标签
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string? KeyWord { get; set; }
    }
}