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
    /// 用于修饰 <see cref="IListArgs{T}"/> 中的属性，<see cref="ListArgsExtensions.ToPagedListAsync{T}(IQueryable{T}, IListArgs{T})"/> 
    /// 方法会根据 ListFilterAttribute 配置自动生成查询对象。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ListFilterAttribute : Attribute
    {
        /// <summary>
        /// 使用 == 操作符在同名属性上查询。
        /// </summary>
        public ListFilterAttribute()
        {
            this.TargetProperty = null;
            this.Operator = ListFilterOperator.E;
        }

        /// <summary>
        /// 使用指定的操作符在同名属性上查询。
        /// </summary>
        /// <param name="op"></param>
        public ListFilterAttribute(ListFilterOperator op)
        {
            this.TargetProperty = null;
            this.Operator = op;
        }

        /// <summary>
        /// 使用 == 操作符在指定的属性上查询。
        /// </summary>
        /// <param name="targetProperty"></param>
        public ListFilterAttribute(string targetProperty)
        {
            this.TargetProperty = targetProperty;
            this.Operator = ListFilterOperator.E;
        }

        /// <summary>
        /// 使用指定的操作符在指定的属性上查询。
        /// </summary>
        /// <param name="targetProperty"></param>
        /// <param name="op"></param>
        public ListFilterAttribute(string targetProperty, ListFilterOperator op)
        {
            this.TargetProperty = targetProperty;
            this.Operator = op;
        }

        /// <summary>
        /// 指示在目标类型的哪个属性上查询。如果为 null，则在同名属性上查询。
        /// </summary>
        public string? TargetProperty { get; init; }

        /// <summary>
        /// 指示使用哪个操作符进行查询。
        /// </summary>
        public ListFilterOperator Operator { get; init; }
    }

}
