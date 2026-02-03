using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Base class for observer tests.
    /// </summary>
    internal abstract class ObserverTestsBase
    {
        protected static void Attach_Throws_Test<TAlgorithm>(
             QuikGraph.Algorithms.Observers.IObserver<TAlgorithm> observer)
            where TAlgorithm : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => observer.Attach(null));
        }
    }
}