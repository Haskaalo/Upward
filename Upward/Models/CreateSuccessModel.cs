using Newtonsoft.Json;

namespace Upward.Models
{
    public class CreateSuccessModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("url_with_tag")]
        public string UrlWithTag { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
