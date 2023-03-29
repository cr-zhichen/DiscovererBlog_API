namespace DiscovererBlog_API.Entity.Response;

public class CommentResponse
{
    /// <summary>
    /// 发布评论返回
    /// </summary>
    public class PostAComment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public int CommentId { get; set; }
    }

    /// <summary>
    /// 查看评论返回
    /// </summary>
    public class ViewComment
    {
        /// <summary>
        /// 文章评论列表
        /// </summary>
        public List<ViewCommentItem> Comments { get; set; } = new List<ViewCommentItem>();

        /// <summary>
        /// 查看评论返回
        /// </summary>
        public class ViewCommentItem
        {
            
            /// <summary>
            /// 评论id
            /// </summary>
            public int Id { get; set; } // 评论ID
            
            /// <summary>
            /// 评论用户名
            /// </summary>
            public string? UserName { get; set; } // 评论者用户名

            /// <summary>
            /// 评论邮箱
            /// </summary>
            public string? Email { get; set; } // 评论者邮箱

            /// <summary>
            /// 评论内容
            /// </summary>
            public string Content { get; set; } = string.Empty; // 评论内容

            /// <summary>
            /// 评论创建时间
            /// </summary>
            public DateTime CreatedAt { get; set; } // 评论创建时间

            /// <summary>
            /// 评论更新时间
            /// </summary>
            public DateTime UpdatedAt { get; set; } // 评论更新时间

            /// <summary>
            /// 文章评论列表
            /// </summary>
            public List<ViewCommentItem> Comments { get; set; } = new List<ViewCommentItem>();
        }
    }
}