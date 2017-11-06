using System;

namespace Upward.Models.Database
{
    public partial class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Userid { get; set; }
        public DateTime Created { get; set; }
        public bool Private { get; set; }
    }
}
