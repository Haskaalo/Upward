using System;

namespace Upward.Models.Database
{
    public partial class Pkgapikey
    {
        public Guid Key { get; set; }
        public int Project { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }
}
