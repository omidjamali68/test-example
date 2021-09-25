using System;
using System.Collections.Generic;

namespace Cooking.Infrastructure
{
    public class Range<T> where T : IComparable<T>
    {
        public Range(T from, T to)
        {
            if (Comparer<T>.Default.Compare(to, from) == -1)
                throw new ArgumentOutOfRangeException(nameof(to));
            From = from;
            To = to;
        }

        public T From { get; set; }
        public T To { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Range<T> other && other.GetType() == GetType()
                                         && Equals(other.From, From) && Equals(other.To, To);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To);
        }
    }
}