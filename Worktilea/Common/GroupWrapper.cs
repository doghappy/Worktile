using System.Collections.Generic;

namespace Worktile.Common
{
    class GroupWrapper : List<object>
    {
        public GroupWrapper(IEnumerable<object> items) : base(items) { }

        public string Key { get; set; }
    }
}
