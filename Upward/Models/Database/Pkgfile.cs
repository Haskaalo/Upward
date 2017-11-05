using System;
using System.Collections.Generic;

namespace Upward.Models.Database
{
    public partial class Pkgfile
    {
        public int Id { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public string Label { get; set; }
        public string[] Sha256 { get; set; }
        public string[] Filename { get; set; }
        public DateTime Created { get; set; }
        public int Project { get; set; }
    }
}
