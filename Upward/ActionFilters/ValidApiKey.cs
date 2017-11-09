using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Upward.Models.Database;
using Upward.Models;
using System.Linq;

namespace Upward.ActionFilters
{
    public class ValidApiKey : ActionFilterAttribute
    {
        public bool MustCheck { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var db = (upwardContext)context.HttpContext.RequestServices.GetService(typeof(upwardContext));

            string key = context.HttpContext.Request.Headers["Authorization"];
            var projectName = context.HttpContext.Request.Host.ToString().Split(".")[0];
            var notAuthorized = new ApiErrorModel();

            var project = db.Project.Where(r => r.Name == projectName).FirstOrDefault();

            if (project == null)
            {
                notAuthorized.code = "NoSuchProject";
                notAuthorized.message = "No such project exist";
                context.HttpContext.Response.StatusCode = 404;
                context.Result = new JsonResult(notAuthorized);
                return;
            }

            context.HttpContext.Response.Headers["X-Project-Id"] = project.Id.ToString();

            if (MustCheck == false)
            {
                var projectExist = db.Project
                    .Any(x => x.Private == false && x.Name == projectName);

                if (projectExist) return;
            }

            if (string.IsNullOrEmpty(key))
            {
                notAuthorized.code = "MissingKey";
                notAuthorized.message = "Api key is missing from headers";
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(notAuthorized);
                return;
            }
            
            if (!key.StartsWith("Bearer "))
            {
                notAuthorized.code = "NotValidAuthorizationType";
                notAuthorized.message = "The Authorization type in the header isn't Bearer";
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(notAuthorized);
                return;
            }

            Guid actualKey;
            var isUUID = Guid.TryParse(key.Substring("Bearer ".Length).Trim(), out actualKey);

            if (!isUUID)
            {
                notAuthorized.code = "NotValidKey";
                notAuthorized.message = "Api key provided isn't valid.";

                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(notAuthorized);
                return;
            }

            var doesKeyExist = db.Pkgapikey.Any(x => x.Project == project.Id && x.Key == actualKey);

            if (!doesKeyExist)
            {
                notAuthorized.code = "KeyNotExist";
                notAuthorized.message = "This key doesn't exists";

                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(notAuthorized);
                return;
            }
        }
    }
}
