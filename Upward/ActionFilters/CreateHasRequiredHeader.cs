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

            if (create.Tag != null && string.IsNullOrWhiteSpace(create.Tag))
            {
                notValid.code = "EmptyTag";
                notValid.message = "The tag is empty or consist of whitespace(s)";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }
        }
    }
}
