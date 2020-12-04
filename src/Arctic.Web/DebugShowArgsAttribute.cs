using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Text.Json;

namespace Arctic.Web
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
