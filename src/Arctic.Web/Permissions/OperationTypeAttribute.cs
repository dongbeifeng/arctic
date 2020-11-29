using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Arctic.Web
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OperationTypeAttribute : AuthorizeAttribute, IActionFilter
    {
        const string POLICY_PREFIX = "OPERATION_TYPE_";

        public OperationTypeAttribute(string operationType)
        {
            this.OperationType = operationType;
        }

        public string OperationType
        {
            get
            {
                return Policy.Substring(POLICY_PREFIX.Length);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items[typeof(OperationTypeAttribute)] = OperationType;
        }
    }



}
