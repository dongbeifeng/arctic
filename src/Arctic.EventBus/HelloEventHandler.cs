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

using Serilog;
using System.Threading.Tasks;

namespace Arctic.EventBus
{
    /// <summary>
    /// 示例事件处理程序。
    /// </summary>
    public class HelloEventHandler : IEventHandler
    {
        readonly ILogger _logger;
        public HelloEventHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task ProcessAsync(string eventType, object? eventData)
        {
            _logger.Information($"HelloEventHandler 哈希 {this.GetHashCode()}: Hello, {eventData}");
            return Task.CompletedTask;
        }
    }
}
