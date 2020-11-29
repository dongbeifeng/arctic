using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Arctic.Web
{
    public static class ControllerBaseExtensions
    {
        public static ActionResult Success(this ControllerBase controller, IEnumerable data, int total, string message = "OK")
        {
            return controller.Ok(new
            {
                data,
                success = true,
                total,
                message,
            });
        }

        public static ActionResult Success(this ControllerBase controller, string message)
        {
            return controller.Ok(new
            {
                success = true,
                message,
            });
        }


        public static ActionResult Error(this ControllerBase controller, string message = "OK")
        {
            return controller.Ok(new
            {
                success = false,
                message,
            });
        }
    }
}
