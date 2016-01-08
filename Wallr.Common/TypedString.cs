using System;

namespace Wallr.Common
{
    public abstract class TypedString<T> : IComparable<T>, IEquatable<T> where T : TypedString<T>
    {
        public string Value { get; private set; }

        protected virtual StringComparison ComparisonType
        {
            get { return StringComparison.Ordinal; }
        }

        protected TypedString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            this.Value = value;
        }

        public int CompareTo(T other)
        {
            return string.Compare(this.Value, other.Value, ComparisonType);
        }

        public bool Equals(T other)
        {
            return string.Equals(this.Value, other.Value, ComparisonType);
        }

        public override bool Equals(object obj)
        {
            return obj is T && Equals(obj as T);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}