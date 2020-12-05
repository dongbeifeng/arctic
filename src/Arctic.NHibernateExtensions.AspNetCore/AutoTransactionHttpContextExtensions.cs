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
