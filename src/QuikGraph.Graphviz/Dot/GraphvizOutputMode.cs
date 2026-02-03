
using System;


namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Enumeration of possible output modes.
    /// </summary>

    [Serializable]

    public enum GraphvizOutputMode
    {
        /// <summary>
        /// Breadth first.
        /// </summary>
        BreadthFirst,

        /// <summary>
        /// Nodes first.
        /// </summary>
        NodesFirst,

        /// <summary>
        /// Edges first.
        /// </summary>
        EdgesFirst
    }
}