using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;


namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Dot escape helpers.
    /// </summary>
    public static class DotEscapers
    {

        private const string EolGroupName = "Eol";


        private const string CommonGroupName = "Common";


        private const string EolPattern = "\\r\\n|\\n|\\r";


        private const string DoubleQuotePattern = "\"";


        private const string BackslashPattern = "\\\\";


        private static readonly Regex RecordEscapeRegex = new Regex(
            $"(?<{EolGroupName}>{EolPattern})|(?<{CommonGroupName}>\\||<|>|{DoubleQuotePattern}| |{BackslashPattern})",
            RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.Compiled);


        private static readonly Regex GeneralEscapeRegex = new Regex(
            $"(?<{EolGroupName}>{EolPattern})|(?<{CommonGroupName}>{DoubleQuotePattern}|{BackslashPattern})",
            RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Escapes the given <paramref name="value"/> as being a record port value.
        /// </summary>
        /// <param name="value">String value to escape.</param>
        /// <returns>Escaped string.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]

        public static string EscapePort( string value)
        {
            return RecordEscapeRegex.Replace(
                value,
                match => "_");
        }

        /// <summary>
        /// Escapes the given <paramref name="value"/> as being a record value.
        /// </summary>
        /// <param name="value">String value to escape.</param>
        /// <returns>Escaped string.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]

        public static string EscapeRecord( string value)
        {
            return RecordEscapeRegex.Replace(
                value,
                match => match.Groups[CommonGroupName].Success
                    ? $@"\{match.Value}"
                    : @"\n");
        }

        /// <summary>
        /// Escapes the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">String value to escape.</param>
        /// <returns>Escaped string.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]

        public static string Escape( string value)
        {
            return GeneralEscapeRegex.Replace(
                value,
                match => match.Groups[CommonGroupName].Success
                    ? $@"\{match.Value}"
                    : @"\n");
        }
    }
}