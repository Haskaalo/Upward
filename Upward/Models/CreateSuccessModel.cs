using Newtonsoft.Json;

namespace Upward.Models
{
    public class CreateSuccessModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
