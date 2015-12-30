using System;
using System.Collections;
using System.Collections.Generic;

namespace MathEvaluator.Core
{
    public sealed class ReadOnlySet<T> : ISet<T>
    {
        public ReadOnlySet(ISet<T> set)
        {
            this.Source = set;
        }

        private ISet<T> Source { get; }
        public int Count => this.Source.Count;
        public bool IsReadOnly => true;

        public bool Contains(T item)
        {
            return this.Source.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Source.CopyTo(array, arrayIndex);
        }
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return this.Source.IsProperSubsetOf(other);
        }
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return this.Source.IsProperSupersetOf(other);
        }
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return this.Source.IsSubsetOf(other);
        }
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return this.Source.IsSupersetOf(other);
        }
        public bool Overlaps(IEnumerable<T> other)
        {
            return this.Source.Overlaps(other);
        }
        public bool SetEquals(IEnumerable<T> other)
        {
            return this.Source.SetEquals(other);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return this.Source.GetEnumerator();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new InvalidOperationException();
        }
        void ISet<T>.IntersectWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException();
        }
        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException();
        }
        void ISet<T>.UnionWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException();
        }
        void ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException();
        }
        bool ISet<T>.Add(T item)
        {
            throw new InvalidOperationException();
        }
        void ICollection<T>.Clear()
        {
            throw new InvalidOperationException();
        }
        void ICollection<T>.Add(T item)
        {
            throw new InvalidOperationException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Source.GetEnumerator();
        }
    }
}
