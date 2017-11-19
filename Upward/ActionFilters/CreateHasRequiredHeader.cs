using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Upward.Models;
using Upward.Helpers;
using System.Net.Mime;

namespace Upward.ActionFilters
{
    public class CreateHasRequiredHeader : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CreateModel create = (CreateModel)context.ActionArguments["create"];

            var notValid = new ApiErrorModel();

            if (string.IsNullOrWhiteSpace(create.Version))
            {
                notValid.code = "NoVersion";
                notValid.message = "There is no version in the body";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            if (!ValidVersion.ValidSemver(create.Version))
            {
                notValid.code = "NotValidVersion";
                notValid.message = "The version in the body is not valid";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            if (string.IsNullOrWhiteSpace(create.Branch))
            {
                notValid.code = "EmptyBranch";
                notValid.message = "The branch name is empty or consist of whitespace(s)";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            if ((string)context.HttpContext.Request.Headers["content-type"] == null)
            {
                notValid.code = "NoContentType";
                notValid.message = "Missing Content-Type in header";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }
        }
    }
}
