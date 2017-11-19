using Google.Cloud.Storage.V1;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Upward.Models.Database;

namespace Upward.Helpers
{
    public class DeletePackage
    {
        public static async Task DeleteFile(
            int projectId,
            string version,
            string filename,
            string branch,
            StorageClient client,
            upwardContext db)
        {
            var ver = version.Split(".");

            int major = int.Parse(ver[0]);
            int minor = int.Parse(ver[1]);
            int patch = int.Parse(ver[2]);

            var pkg = await db.Pkgfile
                .Where(r => r.Project == projectId && r.Major == major && r.Minor == minor && r.Patch == patch && r.Branch == branch)
                .FirstOrDefaultAsync();

            var project = await db.Project
                .Where(r => r.Id == projectId)
                .FirstOrDefaultAsync();

            var user = await db.Userprofile
                .Where(r => r.Id == project.Userid)
                .FirstOrDefaultAsync();

            var metaData = await client
                .GetObjectAsync("upward-test", $"pkg/{projectId}/{branch}/{version}/{filename}");

            pkg.Filename = pkg.Filename.Where(val => val != filename).ToArray();
            pkg.Size -= (long)metaData.Size;
            user.Size -= (long)metaData.Size;

            await client.DeleteObjectAsync("upward-test", $"pkg/{projectId}/{branch}/{version}/{filename}");
            await db.SaveChangesAsync();
        }
    }
}
