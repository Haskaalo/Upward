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

        // GET: /:tag/:version/:filename
        [HttpGet("/{tag}/{version}/{filename}"), ValidApiKey(MustCheck = false), GetPackageValidate(HasTag = true)]
        public async Task<IActionResult> GetWithTag(string tag, string version, string filename)
        {
            using (var stream = new MemoryStream())
            {
                await storageClient.DownloadObjectAsync("upward-test", $"pkg/{Response.Headers["x-project-id"]}/{version}/{filename}", stream);
                var metaData = await storageClient.GetObjectAsync("upward-test", $"pkg/{Response.Headers["x-project-id"]}/{version}/{filename}");

                return File(stream.ToArray(), metaData.ContentType);
            }
        }

        // GET: /:version/:filename
        [HttpGet("/{version}/{filename}"), ValidApiKey(MustCheck = false), GetPackageValidate(HasTag = false)]
        public async Task<IActionResult> GetNoTag(string version, string filename)
        {
            using (var stream = new MemoryStream())
            {
                await storageClient.DownloadObjectAsync("upward-test", $"pkg/{Response.Headers["x-project-id"]}/{version}/{filename}", stream);
                var metaData = await storageClient.GetObjectAsync("upward-test", $"pkg/{Response.Headers["x-project-id"]}/{version}/{filename}");

                return File(stream.ToArray(), metaData.ContentType);
            }
        }
    }
}
