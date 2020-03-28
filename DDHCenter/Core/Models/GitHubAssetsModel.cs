using Newtonsoft.Json;
using System;

namespace DDHCenter.Core.Models
{
    public class GitHubAssetModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("browser_download_url")]
        public string DownloadUrl { get; set; }
    }
}
