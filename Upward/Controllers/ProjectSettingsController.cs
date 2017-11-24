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
        private readonly upwardContext upwardDb;
        private readonly accountsContext accountsDb;

        public ProjectSettingsController(upwardContext _upwardDb, accountsContext _accountsDb)
        {
            upwardDb = _upwardDb;
            accountsDb = _accountsDb;
        }

        // GET: /create
        [HttpPost("/create"), ValidApiKey(MustCheck = true), CreateHasRequiredHeader]
        public async Task<IActionResult> Create([FromBody] CreateModel create)
        {
            var projectName = HttpContext.Request.Host.ToString().Split(".")[0];
            var project = accountsDb.Project.Where(r => r.Name == projectName).FirstOrDefault();

            var hasNoRollback = await ValidVersion.NoRollback(create.Version, create.Branch, project.Id, upwardDb);
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
                Branch = create.Branch,
                Filename = new string[] { },
                Created = currentDate
            };

            await upwardDb.Pkgfile.AddAsync(newPackage);
            await upwardDb.SaveChangesAsync();

            var SuccessResponse = new CreateSuccessModel
            {
                Url = $"{HttpContext.Request.Host.ToString()}/{create.Branch}/{create.Version}/",
                Created = currentDate.ToString(),
                Branch = create.Branch,
                Version = create.Version
            };

            return Json(SuccessResponse);
        }
    }
}
