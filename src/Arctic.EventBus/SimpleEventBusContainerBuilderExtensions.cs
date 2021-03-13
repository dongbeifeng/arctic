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

using Autofac;
using Serilog;
using System;
using System.Collections.Generic;

namespace Arctic.EventBus
{
    /// <summary>
    /// 向 autofac 注册事件总线。
    /// </summary>
    public static class SimpleEventBusContainerBuilderExtensions
    {
        static readonly ILogger _logger = Log.ForContext(typeof(SimpleEventBusContainerBuilderExtensions));

        public static void AddEventBus(this ContainerBuilder builder, SimpleEventBusOptions options)
        {
            _logger.Information("正在配置事件总线");

            builder.RegisterType<SimpleEventBus>().AsSelf().InstancePerLifetimeScope();

            foreach (var eventConfig in options.Events)
            {
                List<Type> handlerTypes = new List<Type>();
                foreach (var handlerTypeName in eventConfig.Handlers ?? Array.Empty<string>())
                {
                    _logger.Information("添加事件处理程序 {eventType} --> {handlerType}", eventConfig.EventType, handlerTypeName);

                    var handlerType = Type.GetType(handlerTypeName);

                    if (handlerType == null)
                    {
                        throw new ApplicationException($"配置错误，事件处理程序类型 {handlerTypeName} 不存在。");
                    }
                    handlerTypes.Add(handlerType);
                }
                RegisterEventHandlers(builder, eventConfig.EventType, handlerTypes);
            }

            _logger.Information("已配置事件总线");
        }

        /// <summary>
        /// 注册事件处理程序。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="eventType"></param>
        /// <param name="handlerTypes"></param>
        private static void RegisterEventHandlers(ContainerBuilder builder, string eventType, IList<Type> handlerTypes)
        {
            if (string.IsNullOrWhiteSpace(eventType))
            {
                throw new ArgumentException($"参数 {nameof(eventType)} 不能为 null 或空字符串。");
            }

            if (handlerTypes == null)
            {
                throw new ArgumentNullException(nameof(handlerTypes));
            }

            eventType = eventType.Trim();
            foreach (Type handlerType in handlerTypes)
            {
                if (typeof(IEventHandler).IsAssignableFrom(handlerType) == false)
                {
                    throw new ArgumentException($"类型 {handlerType} 不实现 {typeof(IEventHandler)} 接口。");
                }

                builder.RegisterType(handlerType)
                    .As<IEventHandler>()
                    .InstancePerDependency()
                    .WithMetadata<EventHandlerMeta>(cfg =>
                        cfg.For(x => x.EventType, eventType)
                    );
            }
        }
    }
}
