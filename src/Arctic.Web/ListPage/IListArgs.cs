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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Arctic.Web
{
    /// <summary>
    /// 定义列表页参数。
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    public interface IListArgs<T>
    {
        /// <summary>
        /// 获取或设置排序信息。
        /// </summary>
        OrderedDictionary? Sort { get; set; }

        /// <summary>
        /// 获取或设置基于 1 的分页索引。
        /// </summary>
        int? Current { get; set; }

        /// <summary>
        /// 获取或设置每页大小。
        /// </summary>
        int? PageSize { get; set; }


    }

}
