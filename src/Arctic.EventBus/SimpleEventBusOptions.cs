namespace Arctic.EventBus
{
    /// <summary>
    /// 事件总线选项。
    /// </summary>    
    public sealed class SimpleEventBusOptions
    {
        public Event[] Events { get; set; } = new Event[0];

        public class Event
        {
            public string EventType { get; set; }

            public string[] Handlers { get; set; }
        }
    }

}
