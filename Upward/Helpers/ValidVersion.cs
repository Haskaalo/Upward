using System.Linq;
using System.Threading.Tasks;
using Upward.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Upward.Helpers
{
    public class ValidVersion
    {

        public static bool ValidSemver(string version)
        {
            var ver = version.Split(".");

            if (ver.Count() != 3) return false;

            var isMajor = int.TryParse(ver[0], out int major);
            var isMinor = int.TryParse(ver[1], out int minor);
            var isPatch = int.TryParse(ver[2], out int patch);

            if (!isMajor || !isMinor || !isPatch) return false;

            if (major < 0 || minor < 0 || patch < 0) return false;
            return true;
        }

        // ValidSemver must be used before NoRollback.
        public static async Task<bool> NoRollback(string version, string branch, int projectId, upwardContext db)
        {
            var ver = version.Split(".");

            var major = int.Parse(ver[0]);
            var minor = int.Parse(ver[1]);
            var patch = int.Parse(ver[2]);

            var hasRollback = await db.Pkgfile
                .AnyAsync(r => r.Project == projectId &&
                    r.Branch == branch &&
                    (r.Major > major ||
                    (r.Minor > minor && r.Major == major) ||
                    (r.Patch > patch && r.Major == major && r.Minor == minor) ||
                    (r.Patch == patch && r.Major == major && r.Minor == minor)));

            if (hasRollback) return false;

            return true;
        }
    }
}
