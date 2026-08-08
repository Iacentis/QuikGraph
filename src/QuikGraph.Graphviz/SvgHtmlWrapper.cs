using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text.RegularExpressions;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Helpers to related to SVG and HTML.
    /// </summary>
    public static class SvgHtmlWrapper
    {
        /// <summary>
        /// Creates an HTML file containing the given <paramref name="svgFilePath"/>.
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <param name="svgFilePath">SVG file path.</param>
        /// <returns>Dumped HTML file path.</returns>
        [Pure]
        public static string DumpHtml(GraphvizSize size, string svgFilePath)
        {
            ArgumentNullException.ThrowIfNull(svgFilePath);

            string outputFile = $"{svgFilePath}.html";
            var fileWriter = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
            using var html = new StreamWriter(fileWriter);
            html.WriteLine("<html>");
            html.WriteLine("<body>");
            html.WriteLine(
                $"<object data=\"{svgFilePath}\" type=\"image/svg+xml\" width=\"{size.Width}\" height=\"{size.Height}\">");
            html.WriteLine(
                $"  <embed src=\"{svgFilePath}\" type=\"image/svg+xml\" width=\"{size.Width}\" height=\"{size.Height}\" />");
            html.WriteLine("If you see this, you need to install a SVG viewer");
            html.WriteLine("</object>");
            html.WriteLine("</body>");
            html.WriteLine("</html>");

            return outputFile;
        }

        /// <summary>
        /// Creates an HTML file that wraps the given <paramref name="svgFilePath"/>.
        /// </summary>
        /// <param name="svgFilePath">SVG file path.</param>
        /// <returns>HTML file path.</returns>
        [Pure]
        public static string WrapSvg(string svgFilePath)
        {
            GraphvizSize size;
            var fileReader = new FileStream(svgFilePath, FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(fileReader))
            {
                size = ParseSize(reader.ReadToEnd());
            }

            return DumpHtml(size, svgFilePath);
        }


        private const string WidthGroupName = "Width";


        private const string HeightGroupName = "Height";


        private static readonly Regex SizeRegex = new Regex(
            $@"<\s*svg.*width\s*=\s*""\s*(?<{WidthGroupName}>\d+)\s*(px|)\s*"".*height\s*=\s*""\s*(?<{HeightGroupName}>\d+)\s*(px|)\s*""",
            RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Parses the size of an SVG.
        /// </summary>
        /// <param name="svg">SVG content.</param>
        /// <returns>SVG size.</returns>
        [Pure]
        public static GraphvizSize ParseSize(string svg)
        {
            Match match = SizeRegex.Match(svg);
            if (!match.Success)
                return new GraphvizSize(400, 400);

            int size = int.Parse(match.Groups[WidthGroupName].Value);
            int height = int.Parse(match.Groups[HeightGroupName].Value);
            return new GraphvizSize(size, height);
        }
    }
}