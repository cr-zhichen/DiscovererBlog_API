using System.Text;
using DiscovererBlog_API.Context;
using DiscovererBlog_API.Tool;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 在服务容器中添加 DbLinkContext 类别作为数据库内容服务
builder.Services.AddDbContext<DbLinkContext>(opt =>
{
    // 从 appsettings.json 文件中取得 MySQL 的连接字符串
    string? connectionString = ReadConfigJson.ConfigJson["ConnectionStrings"]["MySQL"].ToString();
    // 自动侦测 MySQL 的服务器版本
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    // 使用 MySQL 数据库提供者和连接字符串来配置数据库内容选项
    opt.UseMySql(connectionString, serverVersion);
});

//更改监听端口
builder.WebHost.UseUrls(ReadConfigJson.ConfigJson["UseUrls"].ToString());

// 允许所有域名访问
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//JWT鉴权
builder.Services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true, //是否验证Issuer
            ValidIssuer = ReadConfigJson.ConfigJson["Jwt"]["Issuer"].ToString(), //发行人Issuer
            ValidateAudience = true, //是否验证Audience
            ValidAudience = ReadConfigJson.ConfigJson["Jwt"]["Audience"].ToString(), //订阅人Audience
            ValidateIssuerSigningKey = true, //是否验证SecurityKey
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(ReadConfigJson.ConfigJson["Jwt"]["SecretKey"].ToString())), //SecurityKey
            ValidateLifetime = true, //是否验证失效时间
            ClockSkew = TimeSpan.FromMinutes(5), //Token验证偏差时间 
            RequireExpirationTime = true, //是否需要过期时间
        };
    });


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    // 读取XML文档
    var xmlFile = $"DiscovererBlog_API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    // 添加JWT验证配置
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "请输入Token Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddSingleton(new JwtHelper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Web API");
        c.RoutePrefix = string.Empty;
        c.OAuthClientId("swagger");
        c.OAuthAppName("Swagger UI");
        c.OAuthUsePkce();
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.UseRouting();

// 启用 CORS
app.UseCors("AllowAll");

//JWT中间件
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseHttpsRedirection();

app.MapControllers();

app.Run();