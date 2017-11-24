using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Upward.Models.Database;
using Upward.Models;
using System.Linq;
using Upward.Helpers;
using System.Text.RegularExpressions;

namespace Upward.ActionFilters
{
    public class PutPackageValidate : ActionFilterAttribute
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

            if (!Regex.IsMatch(filename, @"^[\w\-. ]+$"))
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

            var upwardDb = (upwardContext)context.HttpContext.RequestServices.GetService(typeof(upwardContext));
            var Id = context.HttpContext.Response.Headers["X-Project-Id"];
            string contentType = context.HttpContext.Request.Headers["content-type"];

            var ver = version.Split(".");

            int major = int.Parse(ver[0]);
            int minor = int.Parse(ver[1]);
            int patch = int.Parse(ver[2]);

            var pkg = upwardDb.Pkgfile
                .Where(r => r.Project.ToString() == Id.ToString() && r.Major == major && r.Minor == minor && r.Patch == patch && r.Branch == branch)
                .FirstOrDefault();

            if (pkg == null)
            {
                notValid.code = "NoSuchVersion";
                notValid.message = "This version hasn't been created";

                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            if (pkg.Filename.Contains(filename))
            {
                notValid.code = "FileNameAlreadyExist";
                notValid.message = "A same file already exist with the same name and version.";

                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            if (string.IsNullOrEmpty(contentType))
            {
                notValid.code = "WrongMediaType";
                notValid.message = "Content-Type isn't valid.";

                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }
            if (contentType.Split("/").Count() != 2)
            {
                notValid.code = "WrongMediaType";
                notValid.message = "Content-Type isn't valid.";

                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }

            if (context.HttpContext.Request.ContentLength == -1)
            {
                notValid.code = "MissingContentLength";
                notValid.message = "Content-Length isn't valid.";

                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }
            if (context.HttpContext.Request.ContentLength > 2147483648)
            {
                notValid.code = "MissingContentLength";
                notValid.message = "Upload exceeds the maximum allowed package size.";

                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(notValid);
                return;
            }
        }
    }
}
