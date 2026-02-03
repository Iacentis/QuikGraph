
using System;

using System.Collections.Generic;

using System.Runtime.Serialization;



namespace QuikGraph.Collections
{
    /// <summary>
    /// Stores association of vertices to edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>

    [Serializable]

    public sealed class EdgeEdgeDictionary<TVertex, TEdge> : Dictionary<TEdge, TEdge>

        , ICloneable

        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeEdgeDictionary{TVertex,TEdge}"/> class.
        /// </summary>
        public EdgeEdgeDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeEdgeDictionary{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="capacity">Dictionary capacity.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="capacity"/> is negative.</exception>
        public EdgeEdgeDictionary(int capacity)
            : base(capacity)
        {
        }


        [Obsolete("Obsolete")]
        private EdgeEdgeDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Clones this vertices/edges dictionary.
        /// </summary>
        /// <returns>Cloned dictionary.</returns>

        public EdgeEdgeDictionary<TVertex, TEdge> Clone()
        {
            var clone = new EdgeEdgeDictionary<TVertex, TEdge>(Count);
            foreach (KeyValuePair<TEdge, TEdge> pair in this)
            {
                clone.Add(pair.Key, pair.Value);
            }
            return clone;
        }


        /// <inheritdoc />
        object ICloneable.Clone()
        {
            return Clone();
        }

    }
}