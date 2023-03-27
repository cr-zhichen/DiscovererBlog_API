namespace DiscovererBlog_API.Entity;

public class Re
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }

    public Re(int statusCode, string message, object? data = null)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }
}