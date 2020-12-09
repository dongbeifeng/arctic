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

namespace Arctic.Web
{
    /// <summary>
    /// 查询操作符。
    /// </summary>
    public enum ListFilterOperator
    {
        /// <summary>
        /// 表示目标字段等于查询参数
        /// </summary>
        E,

        /// <summary>
        /// 表示目标字段 like 查询参数
        /// </summary>        
        Like,

        /// <summary>
        /// 表示目标字段大于查询参数
        /// </summary>
        GT,

        /// <summary>
        /// 表示目标字段大于等于查询参数
        /// </summary>
        GTE,

        /// <summary>
        /// 表示目标字段小于查询参数
        /// </summary>
        LT,

        /// <summary>
        /// 表示目标字段小于等于查询参数
        /// </summary>
        LTE,

        /// <summary>
        /// 表示目标字段 IN 查询参数
        /// </summary>
        IN,

        /// <summary>
        /// 使用 Linq 查询，用于查询方式不整齐的情况。如果应用于名为 X 的属性上，则会寻找 XExpr 的属性以获取表示谓词的 <see cref="Expression{TDelegate}"/>，TDelegate 是 <see cref="Func{T, TResult}"/>
        /// </summary>
        Linq,
    }

}
