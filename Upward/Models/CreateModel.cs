using Newtonsoft.Json;

namespace Upward.Models
{
    public class CreateModel
    {
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("branch")]
        public string Branch { get; set; }
    }
}
