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
using NHibernate;
using Serilog;
using System;
using System.Data;
using System.Diagnostics;

namespace Arctic.AspNetCore
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
            readonly Stopwatch _sw;
            readonly IsolationLevel _isolationLevel;
            readonly ISession _session;
            readonly ILogger _logger;

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
                _logger.Warning("发生错误，AutoTransaction 即将回滚，{url}", filterContext.HttpContext.Request.GetDisplayUrl());
                try
                {
                    ITransaction transaction = _session.GetCurrentTransaction();
                    if (transaction != null && transaction.IsActive)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                    }
                    _sw.Stop();
                    _logger.Warning("AutoTransaction 已回滚，{url}，耗时 {elapsedTime} 毫秒", filterContext.HttpContext.Request.GetDisplayUrl(), _sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    _sw.Stop();
                    _logger.Error(ex, "AutoTransaction 回滚时出错，{url}，耗时 {elapsedTime} 毫秒", filterContext.HttpContext.Request.GetDisplayUrl(), _sw.ElapsedMilliseconds);
                }
            }
        }
    }
}
