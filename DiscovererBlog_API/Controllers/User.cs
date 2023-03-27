using DiscovererBlog_API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DiscovererBlog_API.Controllers;

[ApiController]
[Route("[controller]")]
public class User : ControllerBase
{
    [HttpPost(Name = "Login")]
    public IActionResult Login()
    {
        return Ok(new Re(200, "成功获取用户数据"));
    }
}