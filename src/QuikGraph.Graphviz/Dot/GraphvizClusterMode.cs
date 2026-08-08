
using System;


namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Enumeration of possible cluster modes.
    /// </summary>

    [Serializable]

    public enum GraphvizClusterMode
    {
        /// <summary>
        /// Local.
        /// </summary>
        Local,

        /// <summary>
        /// Global.
        /// </summary>
        Global,

        /// <summary>
        /// None.
        /// </summary>
        None
    }
}