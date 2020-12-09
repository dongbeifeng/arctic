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

using System;
using System.Linq;

namespace Arctic.Web
{
    /// <summary>
    /// 用于修饰 <see cref="IListArgs{T}"/> 中的属性，
    /// <see cref="ListArgsExtensions.ToPagedListAsync{T}(IQueryable{T}, IListArgs{T})"/> 
    /// 方法会根据 ListFilterAttribute 配置自动生成查询对象。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ListFilterAttribute : Attribute
    {
        /// <summary>
        /// 使用 <see cref="ListFilterOperator.Linq"/> 以外的操作符查询。
        /// </summary>
        /// <param name="op">查询操作符</param>
        /// <param name="targetProperty">目标属性，若为 null，则使用同名属性</param>
        public ListFilterAttribute(ListFilterOperator op = ListFilterOperator.E, string? targetProperty = null)
        {
            if (op == ListFilterOperator.Linq && targetProperty != null)
            {
                throw new ArgumentException("使用 ListFilterOperator.LINQ 时不能指定 targetProperty");
            }
            this.Operator = op;
            this.TargetProperty = targetProperty;
        }

        /// <summary>
        /// 指示使用哪个操作符进行查询。
        /// </summary>
        public ListFilterOperator Operator { get; init; }

        /// <summary>
        /// 使用 <see cref="ListFilterOperator.Linq"/> 以外的查询操作符时指定目标属性。
        /// </summary>
        public string? TargetProperty { get; init; }

    }

}
