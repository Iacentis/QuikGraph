using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;


namespace QuikGraph.Serialization
{
    internal static class GraphMLResourceResolver
    {
        /// <summary>
        /// Gets the GraphML resource with given <paramref name="resourceName"/>.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <returns>Resource stream.</returns>
        [Pure]
        public static Stream GetResource(string resourceName)
        {
            Stream resourceStream = typeof(GraphMLResourceResolver).Assembly
                .GetManifestResourceStream(
                    typeof(GraphMLResourceResolver),
                    $@"GraphML.{resourceName}");

            Debug.Assert(resourceStream != null);
            return resourceStream;
        }
    }
}