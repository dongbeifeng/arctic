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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Arctic.AspNetCore
{
    /// <summary>
    /// 指示 Action 的操作类型，并对用户授权。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OperationTypeAttribute : AuthorizeAttribute, IActionFilter
    {

        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="operationType"></param>
        public OperationTypeAttribute(string operationType)
        {
            this.OperationType = operationType;
        }

        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        public string OperationType
        {
            get
            {
                if (Policy == null)
                {
                    throw new Exception();
                }
                return Policy[POLICY_PREFIX.Value.Length..];
            }
            set
            {
                Policy = $"{POLICY_PREFIX.Value}{value}";
            }
        }

        /// <summary>
        /// 实现 <see cref="IActionFilter.OnActionExecuted(ActionExecutedContext)"/>。
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// 实现 <see cref="IActionFilter.OnActionExecuting(ActionExecutingContext)"/>。
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items[typeof(OperationTypeAttribute)] = OperationType;
        }
    }
}
