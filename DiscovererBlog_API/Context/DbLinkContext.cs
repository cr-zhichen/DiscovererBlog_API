using Microsoft.EntityFrameworkCore;

namespace DiscovererBlog_API.Context;

public class DbLinkContext : DbContext
{
    public DbLinkContext()
    {
    }

    public DbLinkContext(DbContextOptions<DbLinkContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Article> Article { get; set; }
    public DbSet<ArticleHistory> ArticleHistory { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Notification> Notification { get; set; }
}