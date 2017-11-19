using System;

namespace Upward.Models.Database
{
    public partial class Userprofile
    {
        public int Id { get; set; }
        public long Size { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
    }
}
