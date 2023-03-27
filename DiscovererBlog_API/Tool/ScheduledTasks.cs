using DiscovererBlog_API.Static;

namespace DiscovererBlog_API.Tool;

public class ScheduledTasks : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ScheduledTasks(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        //每一分钟删除一次过期验证码
        ScheduledTask s = new ScheduledTask(60,
            new Action(() => { VerificationCode.DataList.RemoveAll(x => x.CreatedAt < DateTime.Now); }));

        // 在这里执行您想要在服务启动时进行的操作
        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // 在这里执行您想要在服务停止时进行的操作
        return Task.CompletedTask;
    }

    private class ScheduledTask
    {
        private int _intervalSecond;
        private Action _action = null;

        public ScheduledTask(int intervalSecond, Action action)
        {
            _intervalSecond = intervalSecond;
            _action = action;
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        public void Start()
        {
            //新建一个线程用来执行定时任务
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _action?.Invoke();

                    //每隔一段时间执行一次
                    Task.Delay(_intervalSecond * 1000).Wait();
                }
            });
        }
    }
}