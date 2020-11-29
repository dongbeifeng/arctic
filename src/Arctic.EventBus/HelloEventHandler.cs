using Serilog;
using System.Threading.Tasks;

namespace Arctic.EventBus
{
    /// <summary>
    /// 示例事件处理程序。
    /// </summary>
    public class HelloEventHandler : IEventHandler
    {
        ILogger _logger;
        public HelloEventHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task ProcessAsync(string eventType, object eventData)
        {
            _logger.Information($"HelloEventHandler 哈希 {this.GetHashCode()}: Hello, {eventData}");
            return Task.CompletedTask;
        }
    }
}
