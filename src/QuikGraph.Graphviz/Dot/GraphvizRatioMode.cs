
using System;


namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Enumeration of possible ratio modes.
    /// </summary>

    [Serializable]

    public enum GraphvizRatioMode
    {
        /// <summary>
        /// Filling.
        /// </summary>
        Fill,

        /// <summary>
        /// Compressing.
        /// </summary>
        Compress,

        /// <summary>
        /// Automatic.
        /// </summary>
        Auto
    }
}