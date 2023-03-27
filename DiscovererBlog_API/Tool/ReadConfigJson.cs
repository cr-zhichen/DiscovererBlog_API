using Newtonsoft.Json.Linq;

namespace DiscovererBlog_API.Tool;

public static class ReadConfigJson
{
    public static JObject ConfigJson = JObject.Parse(
        File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "appsettings.json")));
}