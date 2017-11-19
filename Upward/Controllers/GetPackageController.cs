using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upward.Models.Database;
using Upward.Models;
using Upward.ActionFilters;
using Google.Cloud.Storage.V1;
using System.IO;

namespace Upward.Controllers
{
    public class GetPackageController : Controller
    {
        private readonly StorageClient storageClient;

        private readonly upwardContext db;

        public GetPackageController(upwardContext Db, StorageClient _storageClient)
        {
            db = Db;
            storageClient = _storageClient;
        }

        // GET: /:branch/:version/:filename
        [HttpGet("/{branch}/{version}/{filename}"), ValidApiKey(MustCheck = false), GetPackageValidate]
        public async Task<IActionResult> GetWithBranch(string branch, string version, string filename)
        {
            using (var stream = new MemoryStream())
            {
                await storageClient.DownloadObjectAsync("upward-test", $"pkg/{Response.Headers["x-project-id"]}/{branch}/{version}/{filename}", stream);
                var metaData = await storageClient.GetObjectAsync("upward-test", $"pkg/{Response.Headers["x-project-id"]}/{branch}/{version}/{filename}");

                return File(stream.ToArray(), metaData.ContentType);
            }
        }
    }
}
