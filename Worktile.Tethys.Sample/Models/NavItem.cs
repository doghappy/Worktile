using System;
using System.Collections.Generic;

namespace Worktile.Tethys.Sample.Models
{
    class NavItem
    {
        public string Name { get; set; }
        public string Glyph { get; set; }
        public Type SourcePageType { get; set; }
        public List<NavItem> Children { get; set; }
    }
}
