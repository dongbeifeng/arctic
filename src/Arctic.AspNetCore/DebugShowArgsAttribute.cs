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

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Text.Json;

namespace Arctic.AspNetCore
{
    /// <summary>
    /// 向日志写入 Action 的参数。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DebugShowArgsAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// 初始化新实例。
        /// </summary>
        public DebugShowArgsAttribute()
            : base(typeof(DebugShowActionArgsImpl))
        {
            this.IsReusable = false;
        }

        private class DebugShowActionArgsImpl : ActionFilterAttribute
        {
            readonly ILogger _logger;

            public DebugShowActionArgsImpl(ILogger logger)
            {
                _logger = logger;
            }

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                _logger.Debug("{url} 共有 {argCount} 个参数", context.HttpContext.Request.GetDisplayUrl(), context.ActionArguments.Count);

                int i = 0;
                foreach (var entry in context.ActionArguments)
                {
                    i++;
                    string val = JsonSerializer.Serialize(entry.Value);
                    _logger.Debug("{i}. 名称 {argName} 值 {argValue}", i, entry.Key, val);
                }
            }
        }
    }
}
