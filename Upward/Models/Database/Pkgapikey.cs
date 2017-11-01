using System;
using System.Collections.Generic;

namespace Upward.Models.Database
{
    public partial class Pkgapikey
    {
        public Guid Key { get; set; }
        public int Project { get; set; }
        public DateTime Created { get; set; }
    }
}
