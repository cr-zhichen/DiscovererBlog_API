namespace DiscovererBlog_API.Entity.Request;

public class CommentRequest
{
    /// <summary>
    /// 发布评论传入
    /// </summary>
    public class PostAComment
    {
        /// <summary>
        /// 对应的文章ID
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        /// 评论者用户名 登录用户无需传入
        /// </summary>
        public int? UserName { get; set; } // 评论者用户名

        /// <summary>
        /// 评论者邮箱 登录用户无需传入
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 对应的评论ID 可为空
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 删除评论传入
    /// </summary>
    public class DeleteComment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public int CommentId { get; set; }
    }

    /// <summary>
    /// 查看评论传入
    /// </summary>
    public class ViewComment
    {
        /// <summary>
        /// 对应的文章ID
        /// </summary>
        public int ArticleId { get; set; }
    }
}