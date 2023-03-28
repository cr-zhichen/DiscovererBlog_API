namespace DiscovererBlog_API.Entity.Response;

public class ArticleResponse
{
    /// <summary>
    /// 上传文章返回
    /// </summary>
    public class UploadArticle
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// 返回文章返回
    /// </summary>
    public class ReturnArticle
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 文章作者名
        /// </summary>
        public string UserName { get; set; } // 文章作者名

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; } // 标题

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; } // 内容

        /// <summary>
        /// 文章Markdown内容
        /// </summary>
        public string MarkdownContent { get; set; } // Markdown格式内容

        /// <summary>
        /// 文章分类（逗号分隔）
        /// </summary>
        public string Tags { get; set; } // 分类（逗号分隔）

        /// <summary>
        /// 文章创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } // 文章创建时间

        /// <summary>
        /// 文章更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; } // 文章更新时间

        /// <summary>
        /// 历史记录
        /// </summary>
        public List<HistoryItem> History { get; set; } // 历史记录

        /// <summary>
        /// 历史记录创建时间
        /// </summary>
        public class HistoryItem
        {
            /// <summary>
            /// 历史记录ID
            /// </summary>
            public int HistoryId { get; set; } // 历史版本ID

            /// <summary>
            /// 历史标题
            /// </summary>
            public string Title { get; set; } // 标题

            /// <summary>
            /// 历史内容
            /// </summary>
            public string Content { get; set; } // 历史版本内容

            /// <summary>
            /// 历史Markdown内容
            /// </summary>
            public string MarkdownContent { get; set; } // 历史版本Markdown格式内容

            /// <summary>
            /// 历史分类（逗号分隔）
            /// </summary>
            public string Tags { get; set; } // 分类（逗号分隔）

            /// <summary>
            /// 历史创建时间
            /// </summary>
            public DateTime CreatedAt { get; set; } // 历史版本创建时间
        }
    }

    /// <summary>
    /// 查询文章列表返回
    /// </summary>
    public class QueryArticleList
    {
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<QueryArticleItem> ArticleList { get; set; } = new List<QueryArticleItem>(); // 文章列表

        /// <summary>
        /// 文章列表
        /// </summary>
        public class QueryArticleItem
        {
            /// <summary>
            /// 文章ID
            /// </summary>
            public int Id { get; set; } // 文章ID

            /// <summary>
            /// 文章标题
            /// </summary>
            public string Title { get; set; } // 文章标题

            /// <summary>
            /// 文章Tag
            /// </summary>
            public string Tags { get; set; } // 文章分类

            /// <summary>
            /// 文章修改时间
            /// </summary>
            public DateTime UpdatedAt { get; set; } // 文章修改时间

            /// <summary>
            /// 文章作者名
            /// </summary>
            public string UserName { get; set; } // 文章作者名

            /// <summary>
            /// 文章简介100字以内
            /// </summary>
            public string Introduction { get; set; } // 文章简介
        }
    }

    /// <summary>
    /// 查询文章返回
    /// </summary>
    public class QueryArticle
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 文章作者名
        /// </summary>
        public string UserName { get; set; } // 文章作者名

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; } // 标题

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; } // 内容

        /// <summary>
        /// 文章Markdown内容
        /// </summary>
        public string MarkdownContent { get; set; } // Markdown格式内容

        /// <summary>
        /// 文章分类（逗号分隔）
        /// </summary>
        public string Tags { get; set; } // 分类（逗号分隔）

        /// <summary>
        /// 文章创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } // 文章创建时间

        /// <summary>
        /// 文章更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; } // 文章更新时间
    }
}