using System;
using System.Collections.Generic;
using System.Text;

namespace Worktile.Repository.Entities
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Locale { get; set; }
        public string Timezone { get; set; }

        public List<User> Users { get; set; }
    }
}
