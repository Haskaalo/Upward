using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Upward.Models.Database;
using Upward.Models;
using System.Linq;
using Upward.Helpers;

namespace Upward.ActionFilters
{
    public class GetPackageValidate : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string version = (string)context.ActionArguments["version"];
            string filename = (string)context.ActionArguments["filename"];
            string branch = (string)context.ActionArguments["branch"];

            var notValid = new ApiErrorModel();

            if (filename == null)
            {
                notValid.code = "InvalidFilename";
                notValid.message = "Invalid File name";
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


            var doesExists = db.Pkgfile
                .Any(r => r.Project.ToString() == Id.ToString() && r.Major == major && r.Minor == minor && r.Patch == patch && r.Branch == branch && r.Filename.Contains(filename));

            if (!doesExists)
            {
                notValid.code = "NoSuchPackage";
                notValid.message = "No package with that version or filename exists";
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }
        }
    }
}
