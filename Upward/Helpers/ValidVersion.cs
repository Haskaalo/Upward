using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upward.Models.Database;

namespace Upward.Helpers
{
    public class ValidVersion
    {
        private readonly upwardContext db;

        public ValidVersion(upwardContext context)
        {
            db = context;
        }
    }
}
