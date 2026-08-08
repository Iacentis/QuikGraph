using System;
using System.Diagnostics.Contracts;


namespace QuikGraph.Serialization.Tests
{
    /// <summary>
    /// Various helpers for Serialization tests.
    /// </summary>
    internal static class TestHelpers
    {
        /// <summary>
        /// Converts a <see cref="Type"/> into a string representation for easy serialization.
        /// </summary>
        [Pure]

        public static string TypeToSerializableType( Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }

        [Pure]

        public static string VertexIdentity_Simple(int vertex)
        {
            return vertex.ToString();
        }

        [Pure]

        public static string VertexIdentity_Complex( EquatableTestVertex vertex)
        {
            return vertex.ID;
        }
    }
}