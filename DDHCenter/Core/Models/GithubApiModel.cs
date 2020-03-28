using Newtonsoft.Json;
using System.Collections.Generic;

namespace DDHCenter.Core.Models
{

    public class GithubApiModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("tag_name")]
        public string TagName { get; set; }

        [JsonProperty("assets")]
        public List<GitHubAssetModel> Assets { get; set; }
    }
}
