namespace DiscovererBlog_API.Entity.Response;

public class StatisticsResponse
{
    /// <summary>
    /// 文章数量 返回
    /// </summary>
    public class ArticleCount
    {
        /// <summary>
        /// 文章数量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 评论数量 返回
    /// </summary>
    public class CommentCount
    {
        /// <summary>
        /// 评论数量
        /// </summary>
        public int Count { get; set; }
    }
}