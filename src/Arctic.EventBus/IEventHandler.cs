using System.Threading.Tasks;

namespace Arctic.EventBus
{
    /// <summary>
    /// 定义事件处理程序。
    /// 事件处理程序使用 <see cref="Autofac.Builder.IRegistrationBuilder.InstancePerDependency"/> 注册到容器。
    /// 可通过构造函数向事件处理程序注入依赖项。
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// 处理事件。
        /// </summary>
        /// <param name="eventType">事件名称。</param>
        /// <param name="eventData">事件数据。</param>
        Task ProcessAsync(string eventType, object? eventData);
    }
}
