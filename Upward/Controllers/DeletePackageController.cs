using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upward.ActionFilters;
using Google.Cloud.Storage.V1;
using Upward.Models.Database;
using Upward.Helpers;

namespace Upward.Controllers
{
    public class DeletePackageController : Controller
    {
        private readonly StorageClient storageClient;

        private readonly upwardContext db;

        public DeletePackageController(upwardContext Db, StorageClient _storageClient)
        {
            db = Db;
            storageClient = _storageClient;
        }

        // DELETE: /:tag/:version/:filename
        [HttpDelete("/{tag}/{version}/{filename}"), ValidApiKey(MustCheck = true), GetPackageValidate(HasTag = true)]
        public async Task<IActionResult> DeleteWithTag(string tag, string version, string filename)
        {
            await DeletePackage.DeleteFile(
                projectId: int.Parse(Response.Headers["x-project-id"]),
                version: version,
                filename: filename,
                client: storageClient,
                db: db
                );

            return Ok();
        }

        // DELETE: /:version/:filename
        [HttpDelete("/{version}/{filename}"), ValidApiKey(MustCheck = true), GetPackageValidate(HasTag = false)]
        public async Task<IActionResult> DeleteWithoutTag(string version, string filename)
        {
            await DeletePackage.DeleteFile(
                projectId: int.Parse(Response.Headers["x-project-id"]),
                version: version,
                filename: filename,
                client: storageClient,
                db: db
                );

            return Ok();
        }
    }
}
