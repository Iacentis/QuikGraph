using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;


namespace QuikGraph.Tests
{
    /// <summary>
    /// Test helpers related to serialization.
    /// </summary>
    internal static class SerializationTestHelpers
    {
        [Pure]
        [Obsolete("Obsolete")]
        public static T SerializeAndDeserialize<T>(T @object)
        {
#if !NET9_0_OR_GREATER
            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            // "Save" object state
            bf.Serialize(ms, @object);

            // Re-use the same stream for de-serialization
            ms.Seek(0, 0);

            // Replace the original exception with de-serialized one
            return (T)bf.Deserialize(ms);
#else
            Assert.Ignore("BinaryFormatter is not supported on .NET 9+.");
            return @object;
#endif
        }
    }
}