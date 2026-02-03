using System;
using NUnit.Framework;
using static QuikGraph.Tests.SerializationTestHelpers;

namespace QuikGraph.Tests.Exceptions
{
    /// <summary>
    /// Tests for exceptions.
    /// </summary>
    [TestFixture]
    internal sealed class ExceptionTests
    {
        private static void ExceptionConstructorTest<TException>(
            Func<string, Exception, TException> createException)
            where TException : Exception
        {
            const string message = "Test exception message.";
            var innerException = new Exception("Inner");

            Exception exception = createException(message, innerException);
            Assert.That(message, Is.EqualTo(exception.Message));
            Assert.That(innerException, Is.SameAs(exception.InnerException));

            exception = createException(message, null);
            Assert.That(message, Is.EqualTo(exception.Message));
            Assert.That(exception.InnerException, Is.Null);
        }

        [Test]
        public void ExceptionsConstructor()
        {
            ExceptionConstructorTest((m, e) => new NegativeCapacityException(m, e));
            ExceptionConstructorTest((m, e) => new NegativeCycleGraphException(m, e));
            ExceptionConstructorTest((m, e) => new NegativeWeightException(m, e));
            ExceptionConstructorTest((m, e) => new NonAcyclicGraphException(m, e));
            ExceptionConstructorTest((m, e) => new NonStronglyConnectedGraphException(m, e));
            ExceptionConstructorTest((m, e) => new NoPathFoundException(m, e));
            ExceptionConstructorTest((m, e) => new ParallelEdgeNotAllowedException(m, e));
            ExceptionConstructorTest((m, e) => new VertexNotFoundException(m, e));
        }

        [Obsolete("Obsolete")]
        private static void ExceptionSerializationTest<TException>(
            Func<TException> createException)
            where TException : Exception
        {
            Exception exception = createException();

            // Save the full ToString() value, including the exception message and stack trace.
            string exceptionToString = exception.ToString();

            Exception deserializedException = SerializeAndDeserialize(exception);

            // Double-check that the exception message and stack trace (owned by the base Exception) are preserved
            Assert.That(exception, Is.Not.SameAs(deserializedException));
            Assert.That(exceptionToString, Is.EqualTo(deserializedException.ToString()));
        }

        [Test]
        [Obsolete("Obsolete")]
        public void ExceptionsSerialization()
        {
            ExceptionSerializationTest(() => new NegativeCapacityException());
            ExceptionSerializationTest(() => new NegativeCycleGraphException());
            ExceptionSerializationTest(() => new NegativeWeightException());
            ExceptionSerializationTest(() => new NonAcyclicGraphException());
            ExceptionSerializationTest(() => new NonStronglyConnectedGraphException());
            ExceptionSerializationTest(() => new NoPathFoundException());
            ExceptionSerializationTest(() => new ParallelEdgeNotAllowedException());
            ExceptionSerializationTest(() => new VertexNotFoundException());
        }
    }
}