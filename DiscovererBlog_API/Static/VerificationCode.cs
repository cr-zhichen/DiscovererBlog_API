namespace DiscovererBlog_API.Static;

public static class VerificationCode
{
    public static List<Data> DataList = new();

    /// <summary>
    /// 验证码保存列表数据
    /// </summary>
    public class Data
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 对应的用户
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 验证码到期时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}