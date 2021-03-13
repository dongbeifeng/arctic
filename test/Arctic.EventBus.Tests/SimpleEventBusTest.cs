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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Arctic.EventBus.Tests
{
    public class SimpleEventBusTest
    {
        #region 嵌套类型

        private class FooData
        {
            public int Handler1 { get; set; }
            public int Handler2 { get; set; }
        }

        private class Foo1Handler : IEventHandler
        {
            public async Task ProcessAsync(string eventType, object? eventData)
            {
                await Task.Yield();
                FooData data = (FooData)eventData!;
                data.Handler1++;
            }
        }

        private class Foo2Handler : IEventHandler
        {
            public async Task ProcessAsync(string eventType, object? eventData)
            {
                await Task.Yield();
                FooData data = (FooData)eventData!;
                data.Handler2++;
            }
        }

        private class FooExHandler : IEventHandler
        {
            public async Task ProcessAsync(string eventType, object? eventData)
            {
                await Task.Yield();
                throw new Exception("TEST");
            }
        }


        private class BarData
        {
            public SimpleEventBus EventBus { get; set; } = default!;
            public int CallTimes { get; set; }
        }

        private class BarHandler : IEventHandler
        {
            public async Task ProcessAsync(string eventType, object? eventData)
            {
                if (eventData == null)
                {
                    throw new ArgumentNullException(nameof(eventData));
                }

                BarData data = (BarData)eventData;
                data.CallTimes++;
                await Task.Yield();
                await data.EventBus.FireEventAsync(eventType, eventData);
            }
        }


        private class BazData
        {
            public SimpleEventBus EventBus { get; set; } = default!;

            public List<int> Calls { get; } = new List<int>();
        }

        private class BazHandler : IEventHandler
        {
            private static readonly Random _random = new Random();

            public async Task ProcessAsync(string eventType, object? eventData)
            {
                if (eventData == null)
                {
                    throw new ArgumentNullException(nameof(eventData));
                }
                BazData data = (BazData)eventData;

                await Task.Yield();
                if (data.Calls.Count < data.EventBus.MaxEvents)
                {
                    data.Calls.Add(data.Calls.Count);
                    await Task.Delay(_random.Next(10, 50));
                    await data.EventBus.FireEventAsync(eventType, eventData);
                }
            }
        }

        private class QuxData
        {
            public List<IEventHandler> Handlers { get; } = new List<IEventHandler>();
        }

        private class QuxHandler : IEventHandler
        {
            public Task ProcessAsync(string eventType, object? eventData)
            {
                QuxData? data = (QuxData?)eventData;
                data?.Handlers?.Add(this);
                return Task.CompletedTask;
            }
        }

        #endregion


        public SimpleEventBusTest()
        {
        }

        private SimpleEventBus CreateBus()
        {
            return new SimpleEventBus(new[] {
                new Lazy<IEventHandler, EventHandlerMeta>(() => new Foo1Handler(), new EventHandlerMeta { EventType= "Foo" }),
                new Lazy<IEventHandler, EventHandlerMeta>(() => new Foo2Handler(), new EventHandlerMeta { EventType= "Foo" }),
                new Lazy<IEventHandler, EventHandlerMeta>(() => new FooExHandler(), new EventHandlerMeta { EventType= "FooEx" }),
                new Lazy<IEventHandler, EventHandlerMeta>(() => new BarHandler(), new EventHandlerMeta { EventType= "Bar" }),
                new Lazy<IEventHandler, EventHandlerMeta>(() => new BazHandler(), new EventHandlerMeta { EventType= "Baz" }),
            }, NSubstitute.Substitute.For<Serilog.ILogger>());
        }

        [Fact]
        public async Task 引发未注册处理程序的事件不会抛出异常()
        {
            var bus = CreateBus();

            await bus.FireEventAsync("not registered", null);
        }

        //[Fact]
        //public void 注册方法不接受未实现IEventHandler接口的参数()
        //{
        //    var types = new[] { typeof(object) };
        //    Assert.Throws<ArgumentException>(() => EventBusState.RegisterEventHandlers(_builder, "Foo", types));
        //}

        [Fact]
        public async Task 有多个处理程序应按次序被调用()
        {
            var bus = CreateBus();
            FooData data = new FooData();

            await bus.FireEventAsync("Foo", data);

            Assert.Equal(1, data.Handler1);
            Assert.Equal(1, data.Handler2);
        }


        [Fact]
        public async Task 事件名不区分大小写()
        {
            var bus = CreateBus();
            FooData data = new FooData();

            await bus.FireEventAsync("FOO", data);

            Assert.Equal(1, data.Handler1);
            Assert.Equal(1, data.Handler2);
        }


        [Fact]
        public async Task 超出事件数限制时会抛异常()
        {
            var bus = CreateBus();
            BarData data = new BarData
            {
                EventBus = bus
            };

            await Assert.ThrowsAsync<TooManyEventsException>(async () => await bus.FireEventAsync("Bar", data));

            Assert.Equal(bus.MaxEvents, data.CallTimes - 1);
        }



        [Fact]

        public async Task 两个事件总线实例彼此独立()
        {
            BazData data1 = new BazData
            {
                EventBus = CreateBus()
            };
            BazData data2 = new BazData
            {
                EventBus = CreateBus()
            };

            Task task1 = data1.EventBus.FireEventAsync("Baz", data1);
            Task task2 = data2.EventBus.FireEventAsync("Baz", data2);
            await Task.WhenAll(task1, task2);

            Assert.True(data1.Calls.SequenceEqual(data2.Calls));
            Assert.True(data1.Calls.SequenceEqual(Enumerable.Range(0, 8)));
        }


        [Fact]

        public async Task 测试抛异常()
        {
            var bus = CreateBus();

            await Assert.ThrowsAsync<Exception>(async () => await bus.FireEventAsync("FooEx", null));
        }

        [Fact]

        public async Task 一个处理程序注册两次会创建两个实例()
        {

            SimpleEventBus bus = new SimpleEventBus(new[] {
                new Lazy<IEventHandler, EventHandlerMeta>(() => new QuxHandler(), new EventHandlerMeta { EventType= "Qux" }),
                new Lazy<IEventHandler, EventHandlerMeta>(() => new QuxHandler(), new EventHandlerMeta { EventType= "Qux" }),
            }, NSubstitute.Substitute.For<Serilog.ILogger>());

            QuxData data = new QuxData();

            await bus.FireEventAsync("Qux", data);

            Assert.Equal(2, data.Handlers.Count);
            Assert.NotSame(data.Handlers[0], data.Handlers[1]);
        }
    }


}
