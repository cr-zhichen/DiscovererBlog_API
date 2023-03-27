using System.Security.Claims;
using DiscovererBlog_API.Context;
using DiscovererBlog_API.Entity;
using DiscovererBlog_API.Entity.Request;
using DiscovererBlog_API.Entity.Response;
using DiscovererBlog_API.Tool;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscovererBlog_API.Controllers;

/// <summary>
/// 评论管理
/// </summary>
[ApiController]
[Route("[controller]")]
public class Comment : ControllerBase
{
    private readonly DbLinkContext _dbLinkContext;
    private readonly JwtHelper _jwtHelper;

    public Comment(DbLinkContext dbLinkContext, JwtHelper jwtHelper)
    {
        _dbLinkContext = dbLinkContext;
        _jwtHelper = jwtHelper;
    }


    /// <summary>
    /// 发布评论
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("PostAComment")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> PostAComment(CommentRequest.PostAComment data)
    {
        //判断文章是否存在
        if (_dbLinkContext.Article.FirstOrDefault(o => o.Id == data.ArticleId) == null)
        {
            return Ok(new Re(-1, "文章不存在"));
        }

        //判断评论是否存在
        if (data.ParentId != null && _dbLinkContext.Comment.FirstOrDefault(o => o.Id == data.ParentId) == null)
        {
            return Ok(new Re(-1, "评论不存在"));
        }

        global::Comment comment = new();

        //判断是否有Token
        if (Request.Headers.ContainsKey("Authorization"))
        {
            //根据Token获取用户信息
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var claims = claimsIdentity?.Claims;
            var email = claims?.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
            var userId = _dbLinkContext.User.FirstOrDefault(o => o.Email == email)!.Id;

            comment = new global::Comment()
            {
                ArticleId = data.ArticleId,
                UserId = userId,
                UserName = null,
                Email = null,
                ParentId = data.ParentId,
                Content = data.Content,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
        else
        {
            //判断是否有用户名和邮箱
            if (data.UserName == null || data.Email == null)
            {
                return Ok(new Re(-1, "请填写用户名和邮箱"));
            }

            comment = new global::Comment()
            {
                ArticleId = data.ArticleId,
                UserId = null,
                UserName = data.UserName,
                Email = data.Email,
                ParentId = data.ParentId,
                Content = data.Content,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        await _dbLinkContext.Comment.AddAsync(comment);
        await _dbLinkContext.SaveChangesAsync();

        //查找评论id
        int commentId = _dbLinkContext.Comment.FirstOrDefault(o => o.Content == data.Content)!.Id;

        CommentResponse.PostAComment re = new()
        {
            CommentId = commentId
        };

        return Ok(new Re(0, "评论成功", re));
    }

    /// <summary>
    /// 删除评论
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("DeleteComment")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> DeleteComment(CommentRequest.DeleteComment data)
    {
        //根据Token获取用户信息
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var claims = claimsIdentity?.Claims;
        var email = claims?.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
        var userId = _dbLinkContext.User.FirstOrDefault(o => o.Email == email)!.Id;

        //根据评论id获取评论信息
        var comment = _dbLinkContext.Comment.FirstOrDefault(o => o.Id == data.CommentId);
        //判断评论是否存在
        if (comment == null)
        {
            return Ok(new Re(-1, "评论不存在"));
        }

        //判断评论是否属于该用户
        if (comment.UserId != userId)
        {
            return Ok(new Re(-1, "无权删除该评论"));
        }

        //查询是否有子评论
        var childComment = _dbLinkContext.Comment.FirstOrDefault(o => o.ParentId == data.CommentId);
        //删除所有子评论
        if (childComment != null)
        {
            _dbLinkContext.Comment.RemoveRange(childComment);
        }

        //删除评论
        _dbLinkContext.Comment.Remove(comment);
        await _dbLinkContext.SaveChangesAsync();

        return Ok(new Re(0, "删除成功"));
    }

    /// <summary>
    /// 查看文章评论
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("ViewComment")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> ViewComment(CommentRequest.ViewComment data)
    {
        //查询文章是否存在
        if (_dbLinkContext.Article.FirstOrDefault(o => o.Id == data.ArticleId) == null)
        {
            return Ok(new Re(-1, "文章不存在"));
        }

        CommentResponse.ViewComment re = new();

        //查询评论
        var comment = _dbLinkContext.Comment.Where(o => o.ArticleId == data.ArticleId).ToList();
        //判断评论是否存在
        if (comment.Count == 0)
        {
            return Ok(new Re(-1, "该文章暂无评论"));
        }

        //递归查询评论，并添加到返回值
        re.Comments = GetComments(comment, null);
        return Ok(new Re(0, "查询成功", re));
    }


    private List<CommentResponse.ViewComment.ViewCommentItem> GetComments(List<global::Comment> comments, int? parentId)
    {
        var result = new List<CommentResponse.ViewComment.ViewCommentItem>();
        var topLevelComments = comments.Where(c => c.ParentId == parentId).ToList();

        foreach (var comment in topLevelComments)
        {
            var commentItem = new CommentResponse.ViewComment.ViewCommentItem
            {
                UserName = comment.UserName,
                Email = comment.Email,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };

            commentItem.Comments = GetComments(comments, comment.Id);
            result.Add(commentItem);
        }

        return result;
    }
}