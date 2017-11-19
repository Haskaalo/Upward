using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Storage.V1;
using Upward.ActionFilters;
using System.IO;
using Upward.Helpers;
using Upward.Models.Database;
using Upward.Models;
using System;

namespace Upward.Controllers
{
    public class PutPackageController : Controller
    {
        private readonly StorageClient _storageClient;
        private readonly upwardContext db;
        public PutPackageController(StorageClient storageClient, upwardContext _db)
        {
            _storageClient = storageClient;
            db = _db;
        }

        // PUT: /:Branch/:version/:filename
        [HttpPut("/{branch}/{version}/{filename}"), ValidApiKey(MustCheck = true), PutPackageValidate]
        public async Task<IActionResult> Index(string branch, string version, string filename)
        {
            MemoryStream memstream = new MemoryStream();
            Request.Body.CopyTo(memstream);

            await UploadPackage.UploadFile(
                projectId: int.Parse(Response.Headers["x-project-id"]),
                memBody: memstream,
                version: version,
                filename: filename,
                contentType: Request.Headers["content-type"],
                contentLength: (long)Request.ContentLength,
                branch: branch,
                client: _storageClient,
                db: db
                );

            var SuccessResponse = new CreateSuccessModel
            {
                Url = $"{HttpContext.Request.Host.ToString()}/{branch}/{version}/{filename}",
                Created = DateTime.Now.ToString(),
                Branch = branch,
                Version = version
            };

            return Json(SuccessResponse);
        }
    }
}
