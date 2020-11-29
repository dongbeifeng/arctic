using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Arctic.NHibernateExtensions.AspNetCore
{
    public static class AutoTransactionHttpContextExtensions
    {
        internal const string KEY_BUILD_RESULT_ON_ERROR = "AutoTransaction.BuildResultOnError";
        public static void SetResultFactoryOnError(this HttpContext httpContext, Func<Exception, ActionResult> factory)
        {
            httpContext.Items[KEY_BUILD_RESULT_ON_ERROR] = factory;
        }

        public static Func<Exception, ActionResult> GetResultFactoryOnError(this HttpContext httpContext)
        {
            return (Func<Exception, ActionResult>)httpContext.Items[KEY_BUILD_RESULT_ON_ERROR];
        }
    }


}
