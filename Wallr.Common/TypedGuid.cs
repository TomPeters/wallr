using System;

namespace Wallr.Common
{
    public abstract class TypedGuid<T> : IEquatable<T> where T : TypedGuid<T>
    {
        public Guid Value { get; private set; }

        protected TypedGuid(Guid value)
        {
            Value = value;
        }

        public bool Equals(T other)
        {
            return Value.Equals(other.Value);
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
            return Value.ToString();
        }
    }
}