using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upward.ActionFilters;
using Upward.Models;
using Upward.Models.Database;
using Microsoft.AspNetCore.Http;
using Upward.Helpers;

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
        public async Task<IActionResult> Create(ICollection<IFormFile> files)
        {
            var projectName = HttpContext.Request.Host.ToString().Split(".")[0];
            var project = db.Project.Where(r => r.Name == projectName).FirstOrDefault();

            var hasNoRollback = await new ValidVersion(db).NoRollback(HttpContext.Request.Headers["x-upward-version"], project.Id);
            var apiError = new ApiErrorModel();
            if (!hasNoRollback)
            {
                apiError.code = "VersionHasRollback";
                apiError.message = "The version specified in the header has a rollback";

                HttpContext.Response.StatusCode = 400;
                return Json(apiError);
            }
            return Ok("test");
        }
    }
}
