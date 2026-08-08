using System.Diagnostics.Contracts;
using System.Reflection;


namespace QuikGraph.Serialization
{
    internal struct PropertySerializationInfo
    {
        /// <summary>
        /// Gets the embedded <see cref="PropertyInfo"/>.
        /// </summary>

        public PropertyInfo Property { get; }

        /// <summary>
        /// Gets the property name.
        /// </summary>

        public string Name { get; }

        private readonly bool _hasValue;


        private readonly object _value;

        public PropertySerializationInfo(PropertyInfo property, string name)
            : this(property, name, null)
        {
        }

        public PropertySerializationInfo(
            PropertyInfo property,
            string name,
            object value)
        {
            Property = property;
            Name = name;
            _value = value;
            _hasValue = _value != null;
        }

        [Pure]
        public bool TryGetDefaultValue(out object value)
        {
            value = _value;
            return _hasValue;
        }
    }
}