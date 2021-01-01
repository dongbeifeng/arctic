// Copyright 2020 王建军
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

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Arctic.Web
{
    /// <summary>
    /// 表示列表页结果
    /// </summary>
    /// <remarks>
    /// 与前端 antd protable 匹配的数据结构。
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class ListResult<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public IEnumerable<T>? Data { get; init; }

        /// <summary>
        /// 记录总数
        /// </summary>
        public int Total { get; init; }
    }

}
