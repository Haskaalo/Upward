using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upward.ActionFilters;
using Upward.Models;
using Upward.Models.Database;
using Microsoft.AspNetCore.Http;
using Upward.Helpers;
using System;

namespace Upward.Controllers
{
    public class ProjectSettingsController : Controller
    {
        private readonly upwardContext db;

        public ProjectSettingsController(upwardContext context)
        {
            db = context;
        }

        // GET: /create
        [HttpPost("/create"), ValidApiKey(MustCheck = true), CreateHasRequiredHeader]
        public async Task<IActionResult> Create([FromBody] CreateModel create)
        {
            var projectName = HttpContext.Request.Host.ToString().Split(".")[0];
            var project = db.Project.Where(r => r.Name == projectName).FirstOrDefault();

            var hasNoRollback = await new ValidVersion(db).NoRollback(create.Version, project.Id);
            var apiError = new ApiErrorModel();
            if (!hasNoRollback)
            {
                apiError.code = "VersionHasRollback";
                apiError.message = "The version specified in the header has a rollback";

                HttpContext.Response.StatusCode = 400;
                return Json(apiError);
            }

            var ver = create.Version.Split(".");
            var currentDate = DateTime.Now;

            var newPackage = new Pkgfile
            {
                Project = project.Id,
                Major = int.Parse(ver[0]),
                Minor = int.Parse(ver[1]),
                Patch = int.Parse(ver[2]),
                Label = create.Tag,
                Sha256 = new string[] { },
                Filename = new string[] { },
                Created = currentDate
            };

            await db.Pkgfile.AddAsync(newPackage);
            await db.SaveChangesAsync();

            var SuccessResponse = new CreateSuccessModel
            {
                Url = $"{HttpContext.Request.Host.ToString()}/{create.Version}/",
                UrlWithTag = create.Tag == null ? null : $"{HttpContext.Request.Host.ToString()}/{create.Tag}/{create.Version}",
                Created = currentDate.ToString(),
                Tag = create.Tag,
                Version = create.Version
            };

            return Json(SuccessResponse);
        }
    }
}
