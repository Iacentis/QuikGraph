
using System;


namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Enumeration of possible label justification.
    /// </summary>

    [Serializable]

    public enum GraphvizLabelJustification
    {
        /// <summary>
        /// Left justification.
        /// </summary>
        L,

        /// <summary>
        /// Right justification.
        /// </summary>
        R,

        /// <summary>
        /// Centered.
        /// </summary>
        C
    }
}