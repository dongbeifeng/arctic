using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Arctic.Web
{
    /// <summary>
    /// 指示 Action 的操作类型，并对用户授权。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OperationTypeAttribute : AuthorizeAttribute, IActionFilter
    {
        const string POLICY_PREFIX = "OPERATION_TYPE_";

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
                return Policy[POLICY_PREFIX.Length..];
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
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
