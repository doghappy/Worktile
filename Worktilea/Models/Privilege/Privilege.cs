using Newtonsoft.Json;

namespace Worktile.Models.Privilege
{
    public class Privilege
    {
        [JsonProperty("admin")]
        public PrivilegeObject Admin { get; set; }

        [JsonProperty("message")]
        public PrivilegeObject Message { get; set; }

        [JsonProperty("mission")]
        public PrivilegeObject Mission { get; set; }

        [JsonProperty("calendar")]
        public PrivilegeObject Calendar { get; set; }

        [JsonProperty("drive")]
        public PrivilegeObject Drive { get; set; }

        [JsonProperty("report")]
        public PrivilegeObject Report { get; set; }

        [JsonProperty("crm")]
        public PrivilegeObject Crm { get; set; }

        [JsonProperty("approval")]
        public PrivilegeObject Approval { get; set; }

        [JsonProperty("leave")]
        public PrivilegeObject Leave { get; set; }

        [JsonProperty("appraisal")]
        public PrivilegeObject Appraisal { get; set; }

        [JsonProperty("bulletin")]
        public PrivilegeObject Bulletin { get; set; }

        [JsonProperty("okr")]
        public PrivilegeObject Okr { get; set; }

        [JsonProperty("portal")]
        public PrivilegeObject Portal { get; set; }

        [JsonProperty("mission_vnext")]
        public PrivilegeObject MissionVnext { get; set; }
    }
}
