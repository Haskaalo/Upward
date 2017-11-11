using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using System.IO;
using Upward.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Upward.Helpers
{
    public class UploadPackage
    {
        public static async Task UploadFile(
            int projectId,
            MemoryStream memBody,
            string version,
            string filename,
            string contentType,
            long contentLength,
            StorageClient client,
            upwardContext db)
        {
            var ver = version.Split(".");

            int major = int.Parse(ver[0]);
            int minor = int.Parse(ver[1]);
            int patch = int.Parse(ver[2]);

            var pkg = await db.Pkgfile
                .Where(r => r.Project == projectId && r.Major == major && r.Minor == minor && r.Patch == patch)
                .FirstOrDefaultAsync();

            var project = await db.Project
                .Where(r => r.Id == projectId)
                .FirstOrDefaultAsync();

            var user = await db.Userprofile
                .Where(r => r.GithubId == project.Userid)
                .FirstOrDefaultAsync();

            pkg.Filename = pkg.Filename.Concat(new string[] { filename }).ToArray();
            pkg.Size += contentLength;
            user.Size += contentLength;

            await client.UploadObjectAsync(
                bucket: "upward-test",
                objectName: $"pkg/{projectId}/{version}/{filename}",
                contentType: contentType,
                source: memBody
                );

            await db.SaveChangesAsync();
        }
    }
}
