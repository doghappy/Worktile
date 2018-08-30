using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worktile.WindowsUI.Models.Project
{
    public class ProjectNavDto
    {
        [JsonProperty("project_nav")]
        public ProjectNav ProjectNav { get; set; }

        public List<Project> Projects { get; set; }
    }
}
