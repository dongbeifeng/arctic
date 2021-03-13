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
