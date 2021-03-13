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
