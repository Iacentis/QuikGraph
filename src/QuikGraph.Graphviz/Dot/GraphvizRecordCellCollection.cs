
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Graphviz record cell collection.
    /// </summary>

    [Serializable]

    public sealed class GraphvizRecordCellCollection : Collection<GraphvizRecordCell>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizRecordCellCollection"/> class.
        /// </summary>
        public GraphvizRecordCellCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizRecordCellCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection that is wrapped by the new collection.</param>
        public GraphvizRecordCellCollection( IList<GraphvizRecordCell> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizRecordCellCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection that is wrapped by the new collection.</param>
        public GraphvizRecordCellCollection( GraphvizRecordCellCollection collection)
            : base(collection)
        {
        }
    }
}