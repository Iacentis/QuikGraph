using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;


namespace QuikGraph.Utils
{
    /// <summary>
    /// Helpers to work with <see cref="IDisposable"/>.
    /// </summary>
    public static class DisposableHelpers
    {
        /// <summary>
        /// Calls an action when going out of scope.
        /// </summary>
        /// <param name="action">The action to call.</param>
        /// <returns>A <see cref="IDisposable"/> object to give to a using clause.</returns>
        [Pure]
        public static IDisposable Finally(Action action)
        {
            return new FinallyScope(action);
        }

        private struct FinallyScope : IDisposable
        {
            private Action _action;

            public FinallyScope(Action action)
            {
                Debug.Assert(action != null);

                _action = action;
            }

            /// <inheritdoc />
            public void Dispose()
            {
                _action();
                _action = null;
            }
        }
    }
}