namespace DiscovererBlog_API.Entity.Request;

public class UserRequest
{
    /// <summary>
    /// 注册传入
    /// </summary>
    public class Register
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// 登录传入
    /// </summary>
    public class Login
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// 发送验证码传入
    /// </summary>
    public class SendCode
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }

    /// <summary>
    /// 重置密码传入
    /// </summary>
    public class ResetPassword
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// 更改用户名传入
    /// </summary>
    public class ModifyUserName
    {
        /// <summary>
        /// 新用户名
        /// </summary>
        public string NewUserName { get; set; }
    }
}