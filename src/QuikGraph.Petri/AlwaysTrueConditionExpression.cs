
using System;

using System.Collections.Generic;

namespace QuikGraph.Petri
{
    /// <summary>
    /// Represents an always enabled condition.
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>

    [Serializable]

    public sealed class AlwaysTrueConditionExpression<TToken> : IConditionExpression<TToken>
    {
        /// <inheritdoc />
        public bool IsEnabled(IList<TToken> tokens)
        {
            return true;
        }
    }
}