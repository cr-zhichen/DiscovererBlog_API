using DiscovererBlog_API.Context;
using DiscovererBlog_API.Entity;
using DiscovererBlog_API.Entity.Response;
using DiscovererBlog_API.Tool;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscovererBlog_API.Controllers;

/// <summary>
/// 统计模块
/// </summary>
[ApiController]
[Route("[controller]")]
public class Statistics : ControllerBase
{
    private readonly DbLinkContext _dbLinkContext;
    private readonly JwtHelper _jwtHelper;

    public Statistics(DbLinkContext dbLinkContext, JwtHelper jwtHelper)
    {
        _dbLinkContext = dbLinkContext;
        _jwtHelper = jwtHelper;
    }

    /// <summary>
    /// 文章数量统计
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("ArticleCount")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> ArticleCount()
    {
        //查找文章数量
        int count = _dbLinkContext.Article.Count();
        StatisticsResponse.ArticleCount re = new()
        {
            Count = count
        };
        return Ok(new Re(0, "查询成功", re));
    }

    /// <summary>
    /// 评论数量统计
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("CommentCount")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> CommentCount()
    {
        //查找评论数量
        int count = _dbLinkContext.Comment.Count();
        StatisticsResponse.CommentCount re = new()
        {
            Count = count
        };
        return Ok(new Re(0, "查询成功", re));
    }
}