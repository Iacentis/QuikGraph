
using System;


namespace QuikGraph.Collections
{
    /// <inheritdoc cref="IQueue{T}" />

    [Serializable]

    public sealed class Queue<T> : System.Collections.Generic.Queue<T>, IQueue<T>
    {
    }
}