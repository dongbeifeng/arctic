using System;

namespace Arctic.EventBus
{
    /// <summary>
    /// 事件总线选项。
    /// </summary>    
    public sealed class SimpleEventBusOptions
    {
        public Event[] Events { get; set; } = Array.Empty<Event>();

        public class Event
        {
            public string EventType { get; set; } = default!;

            public string[]? Handlers { get; set; }
        }
    }

}
