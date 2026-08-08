using System;


namespace QuikGraph.Algorithms.TSP
{
    /// <summary>
    ///
    /// </summary>
    public sealed class TaskPriority : IComparable<TaskPriority>
    {
        private readonly double _cost;
        private readonly int _pathSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskPriority"/> class.
        /// </summary>
        /// <param name="cost">Task cost.</param>
        /// <param name="pathSize">Path size.</param>
        public TaskPriority(double cost, int pathSize)
        {
            _cost = cost;
            _pathSize = pathSize;
        }

        #region Equality

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            return obj is TaskPriority priority && Equals(priority);
        }

        private bool Equals(TaskPriority other)
        {
            return _cost.Equals(other._cost)
                   && _pathSize == other._pathSize;
        }

        /// <summary>
        /// Checks if <paramref name="priority1"/> is equal to <paramref name="priority2"/>.
        /// </summary>
        /// <param name="priority1">First priority to compare.</param>
        /// <param name="priority2">Second priority to compare.</param>
        /// <returns>True if they are equal, false otherwise.</returns>
        public static bool operator ==(TaskPriority priority1, TaskPriority priority2)
        {
            if (priority1 is null)
                return priority2 is null;
            if (priority2 is null)
                return false;
            return priority1.Equals(priority2);
        }

        /// <summary>
        /// Checks if <paramref name="priority1"/> is not equal to <paramref name="priority2"/>.
        /// </summary>
        /// <param name="priority1">First priority to compare.</param>
        /// <param name="priority2">Second priority to compare.</param>
        /// <returns>True if they are not equal, false otherwise.</returns>
        public static bool operator !=(TaskPriority priority1, TaskPriority priority2)
        {
            return !(priority1 == priority2);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (_cost.GetHashCode() * 397) ^ _pathSize;
        }

        #endregion

        #region IComparable<T>

        /// <inheritdoc />
        public int CompareTo(TaskPriority other)
        {
            if (other is null)
                return 1;

            int costCompare = _cost.CompareTo(other._cost);
            if (costCompare == 0)
                return -_pathSize.CompareTo(other._pathSize);
            return costCompare;
        }

        /// <summary>
        /// Checks if <paramref name="left"/> is less than <paramref name="right"/>.
        /// </summary>
        /// <param name="left">Left priority.</param>
        /// <param name="right">Right priority.</param>
        /// <returns>True if <paramref name="left"/> is less than <paramref name="right"/>, false otherwise.</returns>
        public static bool operator <(TaskPriority left, TaskPriority right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Checks if <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// </summary>
        /// <param name="left">Left priority.</param>
        /// <param name="right">Right priority.</param>
        /// <returns>True if <paramref name="left"/> is less than or equal to <paramref name="right"/>, false otherwise.</returns>
        public static bool operator <=(TaskPriority left, TaskPriority right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Checks if <paramref name="left"/> is greater than <paramref name="right"/>.
        /// </summary>
        /// <param name="left">Left priority.</param>
        /// <param name="right">Right priority.</param>
        /// <returns>True if <paramref name="left"/> is greater than <paramref name="right"/>, false otherwise.</returns>
        public static bool operator >(TaskPriority left, TaskPriority right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Checks if <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// </summary>
        /// <param name="left">Left priority.</param>
        /// <param name="right">Right priority.</param>
        /// <returns>True if <paramref name="left"/> is greater than or equal to <paramref name="right"/>, false otherwise.</returns>
        public static bool operator >=(TaskPriority left, TaskPriority right)
        {
            return left.CompareTo(right) >= 0;
        }

        #endregion
    }
}