using Newtonsoft.Json;
using System.Collections.Generic;
using Worktile.WindowsUI.Models.General;

namespace Worktile.WindowsUI.Models.Project
{
    public class Project
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("name_pinyin")]
        public string NamePinYin { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public string Color { get; set; }

        public int Visibility { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        public Archive Archive { get; set; }

        public string Identifier { get; set; }

        [JsonProperty("task_identifier_prefix")]
        public string TaskIdentifierPrefix { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        public List<Member> Members { get; set; }

        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }

        [JsonProperty("static_permission")]
        public string StaticPermission { get; set; }
    }
}
