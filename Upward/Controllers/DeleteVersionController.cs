using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upward.ActionFilters;
using Google.Cloud.Storage.V1;
using Upward.Models.Database;
using Upward.Helpers;

namespace Upward.Controllers
{
    public class DeleteVersionController : Controller
    {
        private readonly StorageClient storageClient;

        private readonly upwardContext db;

        public DeleteVersionController(upwardContext Db, StorageClient _storageClient)
        {
            db = Db;
            storageClient = _storageClient;
        }

        // DELETE: /:tag/:version
        [HttpDelete("/{tag}/{version}"), ValidApiKey(MustCheck = true), DeleteVersionValidate(HasTag = true)]
        public async Task<IActionResult> DeleteWithTag(string tag, string version)
        {
            await DeleteVersion.Delete(
                projectId: int.Parse(Response.Headers["x-project-id"]),
                version: version,
                client: storageClient,
                db: db);

            return Ok();
        }

        // DELETE: /:version
        [HttpDelete("/{version}"), ValidApiKey(MustCheck = true), DeleteVersionValidate(HasTag = false)]
        public async Task<IActionResult> DeleteNoTag(string version)
        {
            await DeleteVersion.Delete(
                projectId: int.Parse(Response.Headers["x-project-id"]),
                version: version,
                client: storageClient,
                db: db);

            return Ok();
        }
    }
}
