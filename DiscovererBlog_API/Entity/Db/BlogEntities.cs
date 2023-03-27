using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// 用户表
/// </summary>
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] [MaxLength(50)] public string Username { get; set; } // 用户名

    [Required] [MaxLength(255)] public string Password { get; set; } // 密码（加密存储）

    [Required] [MaxLength(255)] public string Email { get; set; } // 电子邮件地址

    public DateTime CreatedAt { get; set; } // 用户创建时间
    public DateTime UpdatedAt { get; set; } // 用户信息更新时间
}

/// <summary>
/// 文章表
/// </summary>
public class Article
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("User")] public int UserId { get; set; } // 文章作者ID

    [Required] [MaxLength(255)] public string Title { get; set; } // 标题

    [Required] public string Content { get; set; } // 内容

    [Required] public string MarkdownContent { get; set; } // Markdown格式内容

    [MaxLength(255)] public string Tags { get; set; } // 分类（逗号分隔）

    public DateTime CreatedAt { get; set; } // 文章创建时间
    public DateTime UpdatedAt { get; set; } // 文章更新时间
}

/// <summary>
/// 历史记录表
/// </summary>
public class ArticleHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Article")] public int ArticleId { get; set; } // 对应文章ID

    [ForeignKey("User")] public int UserId { get; set; } // 修改者ID

    [Required] [MaxLength(255)] public string Title { get; set; } // 标题
    [Required] public string Content { get; set; } // 历史版本内容

    [Required] public string MarkdownContent { get; set; } // 历史版本Markdown格式内容
    [MaxLength(255)] public string Tags { get; set; } // 分类（逗号分隔）
    public DateTime CreatedAt { get; set; } // 历史版本创建时间
}

/// <summary>
/// 分类表
/// </summary>
public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] [MaxLength(255)] public string Name { get; set; } // 分类名称

    [MaxLength(255)] public string Articles { get; set; } // 分类内文章id（逗号分隔）

    public DateTime CreatedAt { get; set; } // 分类创建时间
    public DateTime UpdatedAt { get; set; } // 分类更新时间
}

/// <summary>
/// 评论表
/// </summary>
public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Article")] public int ArticleId { get; set; } // 对应文章ID

    [ForeignKey("User")] public int? UserId { get; set; } // 评论者ID

    public int? UserName { get; set; } // 评论者用户名

    public string? Email { get; set; } // 评论者邮箱

    public int? ParentId { get; set; } // 用于回复评论

    [Required] public string Content { get; set; } // 评论内容

    public DateTime CreatedAt { get; set; } // 评论创建时间
    public DateTime UpdatedAt { get; set; } // 评论更新时间
}

/// <summary>
/// 通知表
/// </summary>
public class Notification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("User")] public int UserId { get; set; } // 接收通知的用户ID

    [Required] [MaxLength(500)] public string Content { get; set; } // 通知内容

    public bool IsRead { get; set; } // 是否已读

    public DateTime CreatedAt { get; set; } // 通知创建时间
    public DateTime UpdatedAt { get; set; } // 通知更新时间
}