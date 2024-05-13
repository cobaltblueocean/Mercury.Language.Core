using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class ListExtension
    {
        public static void EnsureCapacity<T>(this List<T> list, int minCapacity)
        {
            var tmp = list.ToArray().EnsureCapacity(minCapacity);
            list.Clear();
            list.AddRange(tmp);
        }

        public static void SetSize<T>(this List<T> list, int size)
        {
            EnsureCapacity(list, size);
        }

        public static void Shuffle<T>(this List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<T> Copy<T>(this List<T> list)
        {
            if (list == null) return new List<T>();

            T[] buf = new T[list.Count];
            list.CopyTo(buf);

            return buf.ToList();
        }

        public static String[] ToStringArray<T>(this IList<T> list)
        {
            return Array.ConvertAll<T, string>(list.ToArray(), ConvertObjectToString);
        }

        static string ConvertObjectToString<T>(T obj)
        {
            return obj?.ToString() ?? string.Empty;
        }

        public static int BinarySearchFromTo<T>(this List<T> list, T item, int from, int to)
        {
            var dc = new DefaultComparer<T>();
            if (from > to)
                throw new IndexOutOfRangeException();

            int count = to - from;
            return list.BinarySearch(from, count, item, dc);
        }

        public static void AddAllOfFromTo<T>(this List<T> list, List<T> items, int from, int to)
        {
            var count = to - from;
            T[] array = new T[count];
            items.CopyTo(0, array, from, count);

            list.AddRange(items);
        }

        public static Boolean RemoveAll<T>(this IList<T> list, IList<T> items)
        {
            var change = list.Any(x => items.Contains(x));
            if (change)
                list = list.Except(items).ToList();
            return change;
        }

        public static T[] ToPremitiveArrayWithDefaultIfNull<T>(this IList<Nullable<T>> val) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ToPremitiveArrayWithDefaultIfNull(val, default(T));
        }

        public static T[] ToPremitiveArrayWithDefaultIfNull<T>(this IList<Nullable<T>> val, T value) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            var r = new List<T>();
            AutoParallel.AutoParallelFor(0, val.Count, (i) => {
                if (!val[i].HasValue)
                    r.Add(value);
                else
                    r.Add(val[i].Value);
            });
            return r.ToArray();
        }

        public static Boolean IsEmpty<T>(this List<T> originalList)
        {
            return originalList.Count == 0 ? true : false;
        }

        public static Boolean ContainsAll<T>(this IEnumerable<T> originalList, IEnumerable<T> ts)
        {
            return !ts.Except(originalList).Any();
        }

        public static Complex[] ConvertToComplex(this List<double> real)
        {
            Complex[] c = new Complex[real.Count];
            for (int i = 0; i < real.Count; i++)
            {
                c[i] = new Complex(real[i], 0);
            }

            return c;
        }

        public static ISet<T> AddAll<T>(this ISet<T> originalList, params T[] items)
        {
            foreach (T item in items)
            {
                originalList.Add(item);
            }

            return originalList;
        }

        public static ISet<T> AddAll<T>(this ISet<T> originalList, ISet<T> items)
        {
            foreach (T item in items)
            {
                originalList.Add(item);
            }

            return originalList;
        }

        public static IList<T> AddAll<T>(this IList<T> originalList, params T[] items)
        {
            if (items.Length == 0)
                return originalList;

            if (originalList.GetType().IsArray)
            {
                if (originalList.Count == 0)
                    return items;

                var newLength = originalList.Count + items.Length;
                var newArray = new T[newLength];
                originalList.CopyTo(newArray, 0);
                items.CopyTo(newArray, originalList.Count);

                return newArray;
            }
            else
            {
                foreach (T item in items)
                {
                    originalList.Add(item);
                }
                return originalList;
            }
        }

        public static IList<T> AddAll<T>(this IList<T> originalList, IList<T> items)
        {
            if (items.Count == 0)
                return originalList;

            if (originalList.GetType().IsArray)
            {
                if (originalList.Count == 0)
                    return items;

                var newLength = originalList.Count + items.Count;
                var newArray = new T[newLength];
                originalList.CopyTo(newArray, 0);
                items.CopyTo(newArray, originalList.Count);

                return newArray;
            }
            else
            {
                foreach (T item in items)
                {
                    originalList.Add(item);
                }
                return originalList;
            }
        }

        public static T1[] GetKeys<T1, T2>(this List<KeyValuePair<T1, T2>> list)
        {
            T1[] tmp = new T1[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                tmp[i] = list[i].Key;
            }

            return tmp;
        }

        public static T2[] GetValues<T1, T2>(this List<KeyValuePair<T1, T2>> list)
        {
            T2[] tmp = new T2[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                tmp[i] = list[i].Value;
            }

            return tmp;
        }

        public static T[] ToPremitiveArray<T>(this IList<Nullable<T>> val) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.Where(x => x.HasValue).Cast<T>().ToArray();
        }
        public static T[] ToPremitiveArray<T>(this IList<Nullable<T>> val, int length) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.Take(length).Cast<T>().ToArray();
        }

        public static Nullable<T>[] ToNullableArray<T>(this IList<T> val) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.ToArray().Cast<Nullable<T>>().ToArray();
        }

        public static Nullable<T>[] ToNullableArray<T>(this IList<T> val, int length) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.Take(length).ToArray().Cast<Nullable<T>>().ToArray();
        }

        public static Boolean InsertValueAtKey<T1, T2>(this List<KeyValuePair<T1, T2>> list, T1 key, T2 value)
        {
            int search = Array.BinarySearch(list.GetKeys(), 0, list.Count, key);
            if (search >= 0)
            {
                list[search] = new KeyValuePair<T1, T2>(key, value);
                return true;
            }
            else
            {
                list.Add(new KeyValuePair<T1, T2>(key, value));
                return true;
            }
        }

        public static Boolean ValuesEqual<T>(this IList<T> arrayA, IList<T> arrayB)
        {
            if (arrayA.Count == arrayB.Count)
            {
                if (typeof(T).IsPrimitive)
                {
                    for (int i = 0; i < arrayA.Count; i++)
                    {
                        if (!arrayA[i].Equals(arrayB[i]))
                            return false;
                    }
                    return true;
                }
                else
                {
                    for (int i = 0; i < arrayA.Count; i++)
                    {
                        if (!arrayA[i].AreObjectsEqual(arrayB[i]))
                            return false;
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        private class DefaultComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                if (Nullable.GetUnderlyingType(x.GetType()) != null)
                {
                    if (x == null)
                    {
                        if (y == null)
                        {
                            // If x is null and y is null, they're
                            // equald 
                            return 0;
                        }
                        else
                        {
                            // If x is null and y is not null, y
                            // is greaterd 
                            return -1;
                        }
                    }
                    else
                    {
                        // If x is not null...
                        //
                        if (y == null)
                        // ...and y is null, x is greater.
                        {
                            return 1;
                        }
                        else
                        {
                            return Comparer<T>.Default.Compare(x, y);
                        }
                    }
                }
                else
                {
                    return Comparer<T>.Default.Compare(x, y);
                }
            }
        }

        /// <summary>
        /// Replaces a number of elements in the receiver with the same number of elements of another list.
        /// Replaces elements in the receiver, between <code>from</code> (inclusive) and <code>to</code> (inclusive),
        /// with elements of <code>other</code>, starting from <code>otherFrom</code> (inclusive).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="from">the position of the first element to be replaced in the receiver</param>
        /// <param name="to">the position of the last element to be replaced in the receiver</param>
        /// <param name="other">list holding elements to be copied into the receiver.</param>
        /// <param name="otherFrom">position of first element within other list to be copied.</param>
        public static void ReplaceFromToWithFrom<T>(this List<T> list, int from, int to, List<T> other, int otherFrom)
        {
            // overridden for performance only.
            //if (!(other is IList<T>)) {
            //    // slower
            //    base.ReplaceFromToWithFrom(from, to, other, otherFrom);
            //    return;
            //}
            int Length = to - from + 1;
            if (Length > 0)
            {
                list.CheckRangeFromTo(from, to, list.Count);
                other.CheckRangeFromTo(otherFrom, otherFrom + Length - 1, other.Count);
                //Array.Copy(((IList<T>)other).Elements, otherFrom, Elements, from, Length);
                int count = to - from;
                list.RemoveRange(from, count);
                list.InsertRange(from, other.GetRange(otherFrom, count));

            }
        }

        /// <summary>
        /// Checks if the given range is within the contained array's bounds.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="theSize"></param>
        /// <exception cref="IndexOutOfRangeException">if <i>to!=from-1 || from&lt;0 || from&gt;to || to&gt;=size()</i>.</exception>
        public static void CheckRangeFromTo<T>(this IList<T> list, int from, int to, int theSize)
        {
            if (to == from - 1) return;
            if (from < 0 || from > to || to >= theSize)
                throw new IndexOutOfRangeException(String.Format(Mercury.Language.Core.LocalizedResources.Instance().Exception_FromToSize, from, to, theSize));
        }
    }
}
