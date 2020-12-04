using System;

namespace Arctic.EventBus
{
    /// <summary>
    /// 事件总线异常。
    /// </summary>
    [Serializable]
    public class TooManyEventsException : Exception
    {
        /// <summary>
        /// 调用链中的事件数。
        /// </summary>
        public int EventCount { get; private set; }

        public TooManyEventsException(int eventCount)
        {
            this.EventCount = eventCount;
        }

        public TooManyEventsException(string message) : base(message) { }
        public TooManyEventsException(string message, Exception inner) : base(message, inner) { }
        protected TooManyEventsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }

        public override string Message => $"超出事件数限制。事件数：{this.EventCount}。";
    }

}
