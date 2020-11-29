using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NHibernate;
using Serilog;
using System;
using System.Data;
using System.Diagnostics;
using ISession = NHibernate.ISession;

namespace Arctic.NHibernateExtensions.AspNetCore
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AutoTransactionAttribute : TypeFilterAttribute
    {

        public AutoTransactionAttribute(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            : base(typeof(AutoTransactionImpl))
        {
            this.Arguments = new object[] { isolationLevel, };
            this.IsReusable = false;
        }

        private class AutoTransactionImpl : ActionFilterAttribute, IExceptionFilter
        {
            private Stopwatch _sw;
            IsolationLevel _isolationLevel;
            ISession _session;
            ILogger _logger;

            public AutoTransactionImpl(ISession session, ILogger logger, IsolationLevel isolationLevel)
            {
                _session = session;
                _logger = logger;
                _isolationLevel = isolationLevel;
                _sw = new Stopwatch();
            }

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                _logger.Information("AutoTransaction 开始，{url}", context.HttpContext.Request.GetDisplayUrl());
                _sw.Start();
                _session.BeginTransaction(_isolationLevel);
            }

            public override void OnActionExecuted(ActionExecutedContext filterContext)
            {
                if (filterContext.Exception == null)
                {
                    _session.Flush();
                }
            }

            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
                if (filterContext.Exception == null)
                {
                    var tx = _session.GetCurrentTransaction();
                    if (tx != null)
                    {
                        tx.Commit();
                        _sw.Stop();
                        _logger.Information("AutoTransaction 已提交，{url}，耗时 {elapsedTime} 毫秒", filterContext.HttpContext.Request.GetDisplayUrl(), _sw.ElapsedMilliseconds);
                    }
                }
            }

            public void OnException(ExceptionContext filterContext)
            {
                _logger.Error(filterContext.Exception, "AutoTransaction 捕获错误。" + filterContext.Exception.Message);
                try
                {
                    ITransaction transaction = _session.GetCurrentTransaction();
                    if (transaction != null && transaction.IsActive)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "回滚事务时出错。{message}", ex.Message);
                }

                _sw.Stop();
                _logger.Information("AutoTransaction 已回滚，{url}，耗时 {elapsedTime} 毫秒。", filterContext.HttpContext.Request.GetDisplayUrl(), _sw.ElapsedMilliseconds);

                Func<Exception, ActionResult> _errorHandler = filterContext.HttpContext.GetResultFactoryOnError();
                if (_errorHandler != null)
                {
                    filterContext.Result = _errorHandler(filterContext.Exception);
                    filterContext.ExceptionHandled = true;
                }
            }
        }
    }
}
