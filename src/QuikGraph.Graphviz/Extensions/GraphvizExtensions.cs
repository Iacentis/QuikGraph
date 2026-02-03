using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;


namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Helper extensions to render graphs to graphviz.
    /// </summary>
    public static class GraphvizExtensions
    {
        /// <summary>
        /// Renders a graph to the Graphviz DOT format.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>Graph serialized in DOT format.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        [Pure]
        public static string ToGraphviz<TVertex, TEdge>(this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            return algorithm.Generate();
        }

        /// <summary>
        /// Renders a graph to the Graphviz DOT format.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <param name="initAlgorithm">Delegate that initializes the DOT generation algorithm.</param>
        /// <returns>Graph serialized in DOT format.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="initAlgorithm"/> is <see langword="null"/>.</exception>
        [Pure]
        public static string ToGraphviz<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> graph,
            Action<GraphvizAlgorithm<TVertex, TEdge>> initAlgorithm)
            where TEdge : IEdge<TVertex>
        {
            ArgumentNullException.ThrowIfNull(initAlgorithm);

            var algorithm = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            initAlgorithm(algorithm);
            return algorithm.Generate();
        }

        /// <summary>
        /// Dot to Svg REST API endpoint.
        /// </summary>
        public const string DotToSvgApiEndpoint = "https://rise4fun.com/rest/ask/Agl/";

        /// <summary>
        /// Performs a layout of <paramref name="graph"/> from DOT format to an
        /// SVG (Scalable Vector Graphics) file by calling Agl through
        /// the https://rise4fun.com/ REST services.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>The svg graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        [Pure]
        [Obsolete("Conversion is using an external web service that is no longer available.")]
        public static string ToSvg<TVertex, TEdge>(this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            return ToSvg(ToGraphviz(graph));
        }

        /// <summary>
        /// Performs a layout of <paramref name="graph"/> from DOT format to an
        /// SVG (Scalable Vector Graphics) file by calling Agl through
        /// the https://rise4fun.com/ REST services.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <param name="initAlgorithm">Delegate that initializes the DOT generation algorithm.</param>
        /// <returns>The svg graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="initAlgorithm"/> is <see langword="null"/>.</exception>
        [Pure]
        [Obsolete("Conversion is using an external web service that is no longer available.")]
        public static string ToSvg<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> graph,
            Action<GraphvizAlgorithm<TVertex, TEdge>> initAlgorithm)
            where TEdge : IEdge<TVertex>
        {
            return ToSvg(ToGraphviz(graph, initAlgorithm));
        }

        /// <summary>
        /// Performs a layout from DOT to a SVG (Scalable Vector Graphics) file
        /// by calling Agl through the https://rise4fun.com/ REST services.
        /// </summary>
        /// <param name="dot">The dot graph</param>
        /// <returns>The svg graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dot"/> is <see langword="null"/>.</exception>
        [Pure]
        [Obsolete("Conversion is using an external web service that is no longer available.")]
        public static string ToSvg(string dot)
        {
            if (dot is null)
                throw new ArgumentNullException(nameof(dot));

            var request = WebRequest.Create(DotToSvgApiEndpoint);
            request.Method = "POST";
            // Write dot
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(dot);
            }

            // Read Svg
            WebResponse response = request.GetResponse();
            Stream streamResponse = response.GetResponseStream();
            if (streamResponse is null)
                return string.Empty;
            using (var reader = new StreamReader(streamResponse))
            {
                return reader.ReadToEnd(); // Svg
            }
        }
    }
}