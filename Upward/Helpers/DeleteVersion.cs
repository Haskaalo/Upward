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
            string branch,
            StorageClient client,
            upwardContext upwardDb,
            accountsContext accountsDb)
        {
            var ver = version.Split(".");

            int major = int.Parse(ver[0]);
            int minor = int.Parse(ver[1]);
            int patch = int.Parse(ver[2]);

            var pkg = await upwardDb.Pkgfile
                .Where(r => r.Project == projectId && r.Major == major && r.Minor == minor && r.Patch == patch && r.Branch == branch)
                .FirstOrDefaultAsync();

            var project = await accountsDb.Project
                .Where(r => r.Id == projectId)
                .FirstOrDefaultAsync();

            var user = await accountsDb.Userprofile
                .Where(r => r.Id == project.Userid)
                .FirstOrDefaultAsync();

            user.Size -= pkg.Size;
            upwardDb.Pkgfile.Remove(pkg);

            if (pkg.Filename.Count() == 0)
            {
                await accountsDb.SaveChangesAsync();
                await upwardDb.SaveChangesAsync();
                return;
            } else
            {
                foreach (string name in pkg.Filename) await client.DeleteObjectAsync("upward-test", $"pkg/{projectId}/{branch}/{version}/{name}");

                await accountsDb.SaveChangesAsync();
                await upwardDb.SaveChangesAsync();
            }
        }
    }
}
