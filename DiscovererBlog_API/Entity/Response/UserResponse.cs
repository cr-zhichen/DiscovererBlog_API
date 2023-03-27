namespace DiscovererBlog_API.Entity.Response;

public class UserResponse
{
    /// <summary>
    /// 注册返回
    /// </summary>
    public class Register
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// 登录返回
    /// </summary>
    public class Login
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// 重置密码返回
    /// </summary>
    public class ResetPassword
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// 更改用户名返回
    /// </summary>
    public class ModifyUserName
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }
}