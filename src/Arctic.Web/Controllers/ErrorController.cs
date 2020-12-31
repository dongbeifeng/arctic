// 代码来源：https://docs.microsoft.com/zh-cn/aspnet/core/web-api/handle-errors?view=aspnetcore-5.0#exception-handler

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Arctic.Web.Controllers
{
    /// <summary>
    /// 错误处理程序
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// 生产环境错误处理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(title: context.Error.Message);
        }

        /// <summary>
        /// 开发环境错误处理
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/error-local-development")]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }
    }
}
