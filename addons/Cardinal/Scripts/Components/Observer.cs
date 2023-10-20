using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardinalEngine {
    [Link(ushort.MaxValue - 1)]
    public partial class Observer : NetComponent {
        [Sync(0)]
        public Guid NetworkID;
    }
}
