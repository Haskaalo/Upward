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

        private readonly accountsContext accountsDb;
        private readonly upwardContext upwardDb;

        public DeletePackageController(upwardContext _upwardDb, accountsContext _accountsDb, StorageClient _storageClient)
        {
            upwardDb = _upwardDb;
            storageClient = _storageClient;
        }

        // DELETE: /:branch/:version/:filename
        [HttpDelete("/{branch}/{version}/{filename}"), ValidApiKey(MustCheck = true), GetPackageValidate]
        public async Task<IActionResult> DeleteWithBranch(string branch, string version, string filename)
        {
            await DeletePackage.DeleteFile(
                projectId: int.Parse(Response.Headers["x-project-id"]),
                version: version,
                filename: filename,
                branch: branch,
                client: storageClient,
                upwardDb: upwardDb,
                accountsDb: accountsDb
                );

            return Ok();
        }
    }
}
