using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static QuikGraph.Utils.MathUtils;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Graphviz size (float).
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{" + nameof(Width) + "}x{" + nameof(Height) + "}")]
    public struct GraphvizSizeF
        : ISerializable

    {
        /// <summary>
        /// Width.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:width">See more</see>
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// Height.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:height">See more</see>
        /// </summary>
        public float Height { get; }

        /// <summary>
        /// Indicates if this size is empty.
        /// </summary>
        public bool IsEmpty => IsZero(Width) || IsZero(Height);

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizSizeF"/> struct.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="width"/> and/or <paramref name="height"/> is negative.</exception>
        public GraphvizSizeF(float width, float height)
        {
            if (width < 0.0 || height < 0.0)
                throw new ArgumentException("Width and height must be positive or 0.");

            Width = width;
            Height = height;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}x{1}", Width, Height);
        }


        #region ISerializable

        private GraphvizSizeF(SerializationInfo info, StreamingContext context)
            : this(
                (float)(info.GetValue("w", typeof(float)) ?? 0),
                (float)(info.GetValue("h", typeof(float)) ?? 0))
        {
        }

        /// <inheritdoc />
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("w", Width);
            info.AddValue("h", Height);
        }

        #endregion
    }

    /// <summary>
    /// Graphviz size.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{" + nameof(Width) + "}x{" + nameof(Height) + "}")]
    public struct GraphvizSize
        : ISerializable

    {
        /// <summary>
        /// Width.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:width">See more</see>
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:height">See more</see>
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Indicates if this size is empty.
        /// </summary>
        public bool IsEmpty => Width == 0 || Height == 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizSize"/> struct.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="width"/> and/or <paramref name="height"/> is negative.</exception>
        public GraphvizSize(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("Width and height must be positive or 0.");

            Width = width;
            Height = height;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}x{1}", Width, Height);
        }


        #region ISerializable

        private GraphvizSize(SerializationInfo info, StreamingContext context)
            : this(
                (int)(info.GetValue("w", typeof(int)) ?? 0),
                (int)(info.GetValue("h", typeof(int)) ?? 0))
        {
        }

        /// <inheritdoc />
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("w", Width);
            info.AddValue("h", Height);
        }

        #endregion
    }
}