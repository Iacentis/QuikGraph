
using System;

using System.Diagnostics;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Arguments of an event related to the formatting of an edge.
    /// </summary>

    [Serializable]

    public sealed class FormatEdgeEventArgs<TVertex, TEdge> : EdgeEventArgs<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary />
        internal FormatEdgeEventArgs( TEdge edge,  GraphvizEdge edgeFormat)
            : base(edge)
        {
            Debug.Assert(edgeFormat != null);

            EdgeFormat = edgeFormat;
        }

        /// <summary>
        /// Edge format.
        /// </summary>
        public GraphvizEdge EdgeFormat { get; }
    }
}