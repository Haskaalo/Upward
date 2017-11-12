using Google.Cloud.Storage.V1;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Upward.Models.Database;

namespace Upward.Helpers
{
    public class DeleteVersion
    {
        public static async Task Delete(
            int projectId,
            string version,
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

            user.Size -= pkg.Size;
            db.Pkgfile.Remove(pkg);

            if (pkg.Filename.Count() == 0)
            {
                await db.SaveChangesAsync();
                return;
            } else
            {
                foreach (string name in pkg.Filename)
                {
                    await client.DeleteObjectAsync("upward-test", $"pkg/{projectId}/{version}/{name}");
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
