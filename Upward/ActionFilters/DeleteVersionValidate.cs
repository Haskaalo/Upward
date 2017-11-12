using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Upward.Models.Database;
using Upward.Models;
using System.Linq;
using Upward.Helpers;

namespace Upward.ActionFilters
{
    public class DeleteVersionValidate : ActionFilterAttribute
    {
        public bool HasTag { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string version = (string)context.ActionArguments["version"];

            var notValid = new ApiErrorModel();

            if (version == null)
            {
                notValid.code = "NotValidVersion";
                notValid.message = "The version in the path isn't valid";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            if (!ValidVersion.ValidSemver(version))
            {
                notValid.code = "NotValidVersion";
                notValid.message = "The version in the path isn't valid";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            var db = (upwardContext)context.HttpContext.RequestServices.GetService(typeof(upwardContext));
            var Id = context.HttpContext.Response.Headers["X-Project-Id"];

            var ver = version.Split(".");

            int major = int.Parse(ver[0]);
            int minor = int.Parse(ver[1]);
            int patch = int.Parse(ver[2]);

            if (HasTag == true)
            {
                string tag = (string)context.ActionArguments["tag"];

                var doesExistsWithTag = db.Pkgfile
                    .Any(r => r.Project.ToString() == Id.ToString() && r.Major == major && r.Minor == minor && r.Patch == patch && r.Label == tag);

                if (!doesExistsWithTag)
                {
                    notValid.code = "NoSuchVersion";
                    notValid.message = "No such version exists";
                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = new JsonResult(notValid);
                    return;
                }

            }

            var doesExists = db.Pkgfile
                .Any(r => r.Project.ToString() == Id.ToString() && r.Major == major && r.Minor == minor && r.Patch == patch);

            if (!doesExists)
            {
                notValid.code = "NoSuchVersion";
                notValid.message = "No such version exists";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }
        }
    }
}
