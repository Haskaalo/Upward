using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Upward.Models;
using Upward.Helpers;

namespace Upward.ActionFilters
{
    public class CreateHasRequiredHeader : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string version = context.HttpContext.Request.Headers["x-upward-version"];
            string tag = context.HttpContext.Request.Headers["x-upward-tag"];

            var notValid = new ApiErrorModel();

            if (string.IsNullOrWhiteSpace(version))
            {
                notValid.code = "NoVersion";
                notValid.message = "There is no X-Upward-Version in the headers";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            if (!ValidVersion.ValidSemver(version))
            {
                notValid.code = "NotValidVersion";
                notValid.message = "The version in X-Upward-Version is not valid";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            if (tag != null && string.IsNullOrWhiteSpace(tag))
            {
                notValid.code = "EmptyTag";
                notValid.message = "The X-Upward-Tag is empty or consist of whitespace(s)";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }
        }
    }
}
