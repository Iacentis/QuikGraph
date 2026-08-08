
using System;

using System.Xml.Serialization;

namespace QuikGraph.Serialization
{
    /// <summary>
    /// XML serializable edge class.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>

    [Serializable]

    public class XmlSerializableEdge<TVertex> : IEdge<TVertex>
    {
        /// <inheritdoc />
        [XmlElement]
        // ReSharper disable once NotNullMemberIsNotInitialized, Justification: Because it's a serializable structure.
        public TVertex Source { get; set; }

        /// <inheritdoc />
        [XmlElement]
        // ReSharper disable once NotNullMemberIsNotInitialized, Justification: Because it's a serializable structure.
        public TVertex Target { get; set; }
    }
}