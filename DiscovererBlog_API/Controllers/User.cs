using System.Security.Claims;
using ChatGPT_API.Tool;
using DiscovererBlog_API.Context;
using DiscovererBlog_API.Entity;
using DiscovererBlog_API.Entity.Request;
using DiscovererBlog_API.Entity.Response;
using DiscovererBlog_API.Static;
using DiscovererBlog_API.Tool;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DiscovererBlog_API.Controllers;

/// <summary>
/// 用户信息管理
/// </summary>
[ApiController]
[Route("[controller]")]
public class User : ControllerBase
{
    private readonly DbLinkContext _dbLinkContext;
    private readonly JwtHelper _jwtHelper;

    public User(DbLinkContext dbLinkContext, JwtHelper jwtHelper)
    {
        _dbLinkContext = dbLinkContext;
        _jwtHelper = jwtHelper;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> Login(UserRequest.Login data)
    {
        if (_dbLinkContext.User.Any(
                o => o.Email == data.Email && o.Password == data.Password))
        {
            string userName =
                _dbLinkContext.User.FirstOrDefault(
                    o => o.Email == data.Email)!.Username;

            string token = _jwtHelper.CreateToken(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, data.Email),
            });
            UserResponse.Login re = new()
            {
                UserName = userName,
                Token = token
            };

            return Ok(new Re(0, "登录成功", re));
        }

        return Ok(new Re(-1, "登录失败,账号或密码错误"));
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("Register")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> Register(UserRequest.Register data)
    {
        //判断邮箱是否已经注册
        if (_dbLinkContext.User.Any(o => o.Email == data.Email))
        {
            return Ok(new Re(-1, "邮箱已经注册"));
        }

        //判断验证码是否正确
        var d = VerificationCode.DataList.FindAll(o => o.Email == data.Email);
        if (d.Count == 0)
        {
            return Ok(new Re(-1, "验证码错误"));
        }

        //将用户信息存入数据库
        _dbLinkContext.User.Add(new()
        {
            Username = data.UserName,
            Password = data.Password,
            Email = data.Email,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        });
        await _dbLinkContext.SaveChangesAsync();

        string token = _jwtHelper.CreateToken(new List<Claim>
        {
            new Claim(ClaimTypes.Name, data.UserName),
            new Claim(ClaimTypes.Email, data.Email),
        });

        UserResponse.Register re = new()
        {
            Token = token
        };

        return Ok(new Re(0, "注册成功", re));
    }

    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("SendCode")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> SendCode(UserRequest.SendCode data)
    {
        var d = VerificationCode.DataList.FindAll(o => o.Email == data.Email);
        if (d.Count >= 3)
        {
            return Ok(new Re(-1, "验证码发送过于频繁，请稍后"));
        }

        //生成验证码
        var code = new Random().Next(100000, 999999);
        VerificationCode.DataList.Add(new VerificationCode.Data()
        {
            Code = code.ToString(),
            Email = data.Email,
            CreatedAt = DateTime.Now.AddMinutes(5)
        });

        //发送验证码
        SMTPMail smtpMail = new SMTPMail(new()
        {
            smtpService = ReadConfigJson.ConfigJson["SMTP"]["SmtpService"].ToString(),
            sendEmail = ReadConfigJson.ConfigJson["SMTP"]["SendEmail"].ToString(),
            sendPwd = ReadConfigJson.ConfigJson["SMTP"]["SendPwd"].ToString(),
            port = int.Parse(ReadConfigJson.ConfigJson["SMTP"]["Port"].ToString()),
            reAddress = data.Email,
            subject = "验证码",
            body = $"您的验证码为：{code}，请在5分钟内使用"
        });
        bool isSend = await smtpMail.Send();
        return Ok(new Re(isSend ? 0 : -1, isSend ? "验证码发送成功" : "验证码发送失败"));
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("ResetPassword")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> ResetPassword(UserRequest.ResetPassword data)
    {
        //判断邮箱是否已经注册
        if (_dbLinkContext.User.Any(o => o.Email == data.Email))
        {
            return Ok(new Re(-1, "邮箱已经注册"));
        }

        //判断验证码是否正确
        var d = VerificationCode.DataList.FindAll(o => o.Email == data.Email);
        if (d.Count == 0)
        {
            return Ok(new Re(-1, "验证码错误"));
        }

        //更新用户信息
        var user = _dbLinkContext.User.FirstOrDefault(o => o.Email == data.Email);
        user!.Password = data.Password;
        user.UpdatedAt = DateTime.Now;
        _dbLinkContext.User.Update(user);
        await _dbLinkContext.SaveChangesAsync();

        UserResponse.ResetPassword re = new()
        {
            UserName = user.Username,
            Token = _jwtHelper.CreateToken(new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            })
        };

        return Ok(new Re(0, "重置密码成功", re));
    }

    /// <summary>
    /// 修改用户名
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("ModifyUserName")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> ModifyUserName(UserRequest.ModifyUserName data)
    {
        //根据Token获取用户信息
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var claims = claimsIdentity?.Claims;
        var email = claims?.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;

        var user = _dbLinkContext.User.FirstOrDefault(o => o.Email == email);
        user!.Username = data.NewUserName;
        user.UpdatedAt = DateTime.Now;
        _dbLinkContext.User.Update(user);
        await _dbLinkContext.SaveChangesAsync();

        UserResponse.ModifyUserName re = new()
        {
            UserName = user.Username,
            Token = _jwtHelper.CreateToken(new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            })
        };

        return Ok(new Re(0, "修改用户名成功", re));
    }
}