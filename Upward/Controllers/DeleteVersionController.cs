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

        // DELETE: /:branch/:version
        [HttpDelete("/{branch}/{version}"), ValidApiKey(MustCheck = true), DeleteVersionValidate]
        public async Task<IActionResult> DeleteWithBranch(string branch, string version)
        {
            await DeleteVersion.Delete(
                projectId: int.Parse(Response.Headers["x-project-id"]),
                version: version,
                branch: branch,
                client: storageClient,
                db: db);

            return Ok();
        }
    }
}
