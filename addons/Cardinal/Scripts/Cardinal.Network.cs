using System;
using Godot;


namespace CardinalEngine {
    public partial class Cardinal {
        public static bool ConnectedToServer { get; internal set; }
        public static Guid LocalNetworkID { get; internal set; }
    }
}
