using System.Security.Claims;
using DiscovererBlog_API.Context;
using DiscovererBlog_API.Entity;
using DiscovererBlog_API.Entity.Request;
using DiscovererBlog_API.Entity.Response;
using DiscovererBlog_API.Tool;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscovererBlog_API.Controllers;

/// <summary>
/// 文章管理
/// </summary>
[ApiController]
[Route("[controller]")]
public class Article : ControllerBase
{
    private readonly DbLinkContext _dbLinkContext;
    private readonly JwtHelper _jwtHelper;

    public Article(DbLinkContext dbLinkContext, JwtHelper jwtHelper)
    {
        _dbLinkContext = dbLinkContext;
        _jwtHelper = jwtHelper;
    }

    /// <summary>
    /// 上传文章
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("UploadArticle")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> UploadArticle(ArticleRequest.UploadArticle data)
    {
        //根据Token获取用户信息
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var claims = claimsIdentity?.Claims;
        var email = claims?.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
        var userId = _dbLinkContext.User.FirstOrDefault(o => o.Email == email)!.Id;

        //判断是否不是修改文章
        if (data.ArticleId is null)
        {
            var newArticle = new global::Article()
            {
                UserId = userId,
                Title = data.Title,
                Content = data.Content ?? "",
                MarkdownContent = data.Md_Content ?? "",
                Tags = data.Tags,
                CreatedAt = data.CreateTime ?? DateTime.Now,
                UpdatedAt = data.UpdateTime ?? DateTime.Now,
            };
            _dbLinkContext.Article.Add(newArticle);
            //保存修改
            int affectedRows = await _dbLinkContext.SaveChangesAsync();
            int articleId = 0;

            if (affectedRows > 0)
            {
                //获取文章Id
                articleId = newArticle.Id;
            }
            else
            {
                return Ok(new Re(-1, "上传失败", null));
            }

            //使用逗号分隔标签 并转换为数组
            string[] tags = data.Tags.Split(",");
            //判断标签是否存在
            foreach (string tag in tags)
            {
                if (!_dbLinkContext.Category.Any(o => o.Name == tag))
                {
                    //添加标签
                    _dbLinkContext.Category.Add(new()
                    {
                        Name = tag,
                        Articles = articleId.ToString() + ",", //添加文章Id
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    });
                }
                else
                {
                    _dbLinkContext.Category.FirstOrDefault(o => o.Name == tag)!.Articles +=
                        articleId + ","; //添加文章Id
                }

                //保存修改
                await _dbLinkContext.SaveChangesAsync();
            }

            return Ok(new Re(0, "上传成功", new ArticleResponse.UploadArticle
            {
                Id = articleId
            }));
        }
        else
        {
            //根据ArticleId查找文章
            global::Article? article =
                _dbLinkContext.Article.FirstOrDefault(o => o.Id == data.ArticleId);

            //判断文章是否存在
            if (article is null)
            {
                return Ok(new Re(-1, "文章不存在", null));
            }

            //判断是否是作者
            if (article.UserId != userId)
            {
                return Ok(new Re(-1, "你不是作者", null));
            }

            //将文章写入历史记录
            _dbLinkContext.ArticleHistory.Add(new()
            {
                ArticleId = article.Id,
                UserId = article.UserId,
                Title = article.Title,
                Content = article.Content,
                MarkdownContent = article.MarkdownContent,
                Tags = article.Tags,
                CreatedAt = article.CreatedAt,
            });

            //修改文章
            article.Title = data.Title;
            article.Content = data.Content ?? "";
            article.MarkdownContent = data.Md_Content ?? "";
            article.Tags = data.Tags;
            article.CreatedAt = data.CreateTime ?? DateTime.Now;
            article.UpdatedAt = data.UpdateTime ?? DateTime.Now;

            //保存修改 并获取文章Id
            _dbLinkContext.Article.Update(article);
            await _dbLinkContext.SaveChangesAsync();

            //使用逗号分隔标签 并转换为数组
            string[] tags = data.Tags.Split(",");
            //判断标签是否存在
            foreach (string tag in tags)
            {
                if (!_dbLinkContext.Category.Any(o => o.Name == tag))
                {
                    //添加标签
                    _dbLinkContext.Category.Add(new()
                    {
                        Name = tag,
                        Articles = article.Id.ToString() + ",", //添加文章Id
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    });
                }
                else
                {
                    _dbLinkContext.Category.FirstOrDefault(o => o.Name == tag)!.Articles +=
                        article.Id + ","; //添加文章Id
                }

                //保存修改
                await _dbLinkContext.SaveChangesAsync();
            }

            return Ok(new Re(0, "修改成功", new ArticleResponse.UploadArticle
            {
                Id = article.Id
            }));
        }
    }

    /// <summary>
    /// 删除文章
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("DeleteArticle")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> DeleteArticle(ArticleRequest.DeleteArticle data)
    {
        //根据Token获取用户信息
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var claims = claimsIdentity?.Claims;
        var email = claims?.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
        var userId = _dbLinkContext.User.FirstOrDefault(o => o.Email == email)!.Id;

        //根据ArticleId查找文章
        global::Article? article =
            _dbLinkContext.Article.FirstOrDefault(o => o.Id == data.ArticleId);

        //判断文章是否存在
        if (article is null)
        {
            return Ok(new Re(-1, "文章不存在", null));
        }

        //判断是否是作者
        if (article.UserId != userId)
        {
            return Ok(new Re(-1, "你不是作者", null));
        }

        //删除文章
        _dbLinkContext.Article.Remove(article);

        //根据文章id删除文章历史记录
        _dbLinkContext.ArticleHistory.RemoveRange(
            _dbLinkContext.ArticleHistory.Where(o => o.ArticleId == data.ArticleId));

        //根据文章id删除文章评论
        _dbLinkContext.Comment.RemoveRange(
            _dbLinkContext.Comment.Where(o => o.ArticleId == data.ArticleId));

        //根据文章从标签中删除文章Id
        foreach (string tag in article.Tags.Split(","))
        {
            _dbLinkContext.Category.FirstOrDefault(o => o.Name == tag)!.Articles =
                _dbLinkContext.Category.FirstOrDefault(o => o.Name == tag)!.Articles.Replace(
                    data.ArticleId + ",", "");

            //判断标签是否还有文章
            if (_dbLinkContext.Category.FirstOrDefault(o => o.Name == tag)!.Articles == "")
            {
                //删除标签
                _dbLinkContext.Category.Remove(_dbLinkContext.Category.FirstOrDefault(o => o.Name == tag)!);
            }
        }

        await _dbLinkContext.SaveChangesAsync();

        return Ok(new Re(0, "删除成功", null));
    }

    /// <summary>
    /// 返回文章
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("ReturnArticle")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> ReturnArticle(ArticleRequest.ReturnArticle data)
    {
        //根据Token获取用户信息
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var claims = claimsIdentity?.Claims;
        var email = claims?.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
        var userId = _dbLinkContext.User.FirstOrDefault(o => o.Email == email)!.Id;

        //根据ArticleId查找文章
        global::Article? article =
            _dbLinkContext.Article.FirstOrDefault(o => o.Id == data.ArticleId);

        //判断文章是否存在
        if (article is null)
        {
            return Ok(new Re(-1, "文章不存在", null));
        }

        //判断是否是作者
        if (article.UserId != userId)
        {
            return Ok(new Re(-1, "你不是作者", null));
        }

        //根据ArticleId查找历史记录列表
        List<ArticleHistory> articleHistoryList =
            _dbLinkContext.ArticleHistory.Where(o => o.ArticleId == data.ArticleId).ToList();

        var reHistoryList = new List<ArticleResponse.ReturnArticle.HistoryItem>();

        foreach (var item in articleHistoryList ?? new())
        {
            reHistoryList.Add(new ArticleResponse.ReturnArticle.HistoryItem
            {
                HistoryId = item.Id,
                Title = item.Title,
                Content = item.Content,
                MarkdownContent = item.MarkdownContent,
                Tags = item.Tags,
                CreatedAt = item.CreatedAt,
            });
        }

        //返回
        var re = new ArticleResponse.ReturnArticle()
        {
            Id = article.Id,
            UserName = userId,
            Title = article.Title,
            Content = article.Content,
            MarkdownContent = article.MarkdownContent,
            Tags = article.Tags,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
            History = reHistoryList
        };

        return Ok(new Re(0, "成功返回", re));
    }


    /// <summary>
    /// 删除历史记录
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("DeleteHistory")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> DeleteHistory(ArticleRequest.DeleteArticle data)
    {
        //根据Token获取用户信息
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var claims = claimsIdentity?.Claims;
        var email = claims?.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
        var userId = _dbLinkContext.User.FirstOrDefault(o => o.Email == email)!.Id;

        //根据HistoryId查找历史记录
        ArticleHistory? articleHistory =
            _dbLinkContext.ArticleHistory.FirstOrDefault(o => o.Id == data.ArticleId);

        //判断历史记录是否存在
        if (articleHistory is null)
        {
            return Ok(new Re(-1, "历史记录不存在", null));
        }

        //判断是否是作者
        if (articleHistory.UserId != userId)
        {
            return Ok(new Re(-1, "你不是作者", null));
        }

        //删除历史记录
        _dbLinkContext.ArticleHistory.Remove(articleHistory);
        await _dbLinkContext.SaveChangesAsync();

        return Ok(new Re(0, "删除成功", null));
    }

    /// <summary>
    /// 查询文章列表
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("QueryArticleList")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> QueryArticleList(ArticleRequest.QueryArticleList data)
    {
        string[]? tags = data.Tag?.Split(",");
        ArticleRequest.QueryArticleList articleList = new();

        //根据条件查询文章列表
        var articleListQuery = _dbLinkContext.Article.AsQueryable();

        //如果有标签，根据标签查询
        if (tags != null)
        {
            foreach (string tag in tags)
            {
                articleListQuery = articleListQuery.Where(o => o.Tags.Contains(tag));
            }
        }

        //根据关键字查询
        if (data.KeyWord != null)
        {
            //标题或内容包含关键字
            articleListQuery = articleListQuery.Where(o => o.Title.Contains(data.KeyWord) ||
                                                           o.Content.Contains(data.KeyWord));
        }

        //根据创建时间排序
        articleListQuery = articleListQuery.OrderByDescending(o => o.CreatedAt);

        //分页
        articleListQuery = articleListQuery.Skip((data.Page - 1) * data.PageSize).Take(data.PageSize);

        //查询
        var articleListQueryResult = await articleListQuery.ToListAsync();

        ArticleResponse.QueryArticleList re = new();
        re.ArticleList = new();

        foreach (var item in articleListQueryResult)
        {
            string userName = _dbLinkContext.User.FirstOrDefault(o => o.Id == item.UserId)!.Username;
            //文章简介，截取前100个字符
            string introduction = item.Content.Length > 100 ? item.Content.Substring(0, 100) : item.Content;

            re.ArticleList.Add(new()
            {
                Id = item.Id,
                Title = item.Title,
                Tags = item.Tags,
                UpdatedAt = item.UpdatedAt,
                UserName = userName,
                Introduction = introduction
            });
        }

        return Ok(new Re(0, "成功返回", re));
    }
}