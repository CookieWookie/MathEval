using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace MathEvaluator.Core
{
    static class Helpers
    {
        public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
        public static ReadOnlySet<T> AsReadOnly<T>(this ISet<T> set)
        {
            return new ReadOnlySet<T>(set);
        }

        public static bool CanPop<T>(this Stack<T> stack)
        {
            return stack.Count > 0;
        }
    }
}
