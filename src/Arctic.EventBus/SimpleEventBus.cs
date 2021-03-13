// Copyright 2020-2021 王建军
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Autofac.Builder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arctic.EventBus
{
    /// <summary>
    /// 基于 Autofac 实现的简单事件总线。
    /// 每次引发事件，都会创建新的事件处理程序实例，而不会重用以前的处理程序实例，这种方式允许事件处理程序有状态。
    /// </summary>
    /// <remarks>
    /// SimpleEventBus 使用 <see cref="IRegistrationBuilder{TLimit, TActivatorData, TRegistrationStyle}.InstancePerLifetimeScope"/> 注册到 Autofac 容器。
    /// </remarks>
    public sealed class SimpleEventBus
    {
        readonly IEnumerable<Lazy<IEventHandler, EventHandlerMeta>> _eventHandlers;

        static readonly Dictionary<SimpleEventBus, Stack<string>> _stacks = new Dictionary<SimpleEventBus, Stack<string>>();

        readonly ILogger _logger;

        private static Stack<string> GetCurrentStack(SimpleEventBus simpleEventBus)
        {
            lock (_stacks)
            {
                if (!_stacks.ContainsKey(simpleEventBus))
                {
                    _stacks[simpleEventBus] = new Stack<string>();
                }

                return _stacks[simpleEventBus];
            }
        }

        private static void RemoveCurrentStack(SimpleEventBus simpleEventBus)
        {
            lock (_stacks)
            {
                _stacks.Remove(simpleEventBus);
            }
        }

        /// <summary>
        /// 初始化新实例。
        /// </summary>
        /// <param name="eventHandlers"></param>
        /// <param name="logger">日志</param>
        /// <param name="maxEvents">指示在一个调用链中最多允许多少个事件</param>
        public SimpleEventBus(IEnumerable<Lazy<IEventHandler, EventHandlerMeta>> eventHandlers, ILogger logger, int maxEvents = 8)
        {
            this.MaxEvents = maxEvents > 0 ? maxEvents : 8;
            _eventHandlers = eventHandlers;
            _logger = logger;
        }

        /// <summary>
        /// 指示在一个调用链中最多允许多少个事件。默认值为 8。
        /// </summary>
        public int MaxEvents { get; private set; }

        /// <summary>
        /// 引发事件。
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventData"></param>
        /// <exception cref="ArgumentNullException">eventType 为 null 或空字符串。</exception>
        /// <exception cref="TooManyEventsException">事件个数已超过 <see cref="MaxEvents"/> 属性的值。</exception>
        public async Task FireEventAsync(string eventType, object? eventData)
        {
            eventType = eventType.Trim();
            if (string.IsNullOrEmpty(eventType))
            {
                throw new ArgumentException("eventType 不能是 null 或空字符串。");
            }

            Stack<string> stack = GetCurrentStack(this);

            _logger.Information("正在引发事件 {eventType}，事件数据 {eventData}", eventType, eventData);
            if (stack.Count > this.MaxEvents)
            {
                throw new TooManyEventsException(stack.Count);
            }

            stack.Push(eventType);
            _logger.Debug("事件个数增加到 {stackSize}", stack.Count);
            try
            {
                await InternalFireEventAsync(eventType, eventData).ConfigureAwait(false);
            }
            finally
            {
                stack.Pop();
                _logger.Debug("事件个数减少到 {stackSize}", stack.Count);
                if (stack.Count == 0)
                {
                    RemoveCurrentStack(this);
                }
            }
        }


        private async Task InternalFireEventAsync(string eventType, object? eventData)
        {
            IEventHandler[] handlers = GetEventHandlers(eventType);
            if (handlers.Length == 0)
            {
                _logger.Warning("事件 {eventType} 没有注册处理程序", eventType);
            }
            foreach (var handler in handlers)
            {
                _logger.Debug("正在调用处理程序 {handlerType}", handler.GetType());
                await handler.ProcessAsync(eventType, eventData).ConfigureAwait(false);
                _logger.Debug("已调用处理程序 {handlerType}", handler.GetType());
            }
        }

        /// <summary>
        /// 为指定的事件获取一组处理程序实例。
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        IEventHandler[] GetEventHandlers(string eventType)
        {
            eventType = eventType.Trim();
            return _eventHandlers
                .Where(x => string.Equals(x.Metadata.EventType, eventType, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Value)
                .ToArray();
        }
    }

}
