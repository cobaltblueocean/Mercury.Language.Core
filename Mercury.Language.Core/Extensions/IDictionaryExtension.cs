using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class IDictionaryExtension
    {

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, IDictionary<TKey, TValue> addingDictionary)
        {
            if ((originalDictionary != null) && (addingDictionary != null))
            {
                if (addingDictionary.Count > 0)
                {
                    foreach (var item in addingDictionary)
                    {
                        originalDictionary.AddOrUpdate(item.Key, item.Value);
                    }
                }
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public static HashSet<TKey> KeysToHashSet<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return new HashSet<TKey>(originalDictionary.Keys.ToList());
        }

        public static HashSet<TValue> ValuesToHashSet<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return new HashSet<TValue>(originalDictionary.Values.ToList());
        }

        #region Extension for IDictionary<TKey, TValue>
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary) where TKey : notnull
        {
            return new ReadOnlyDictionary<TKey, TValue>(originalDictionary);
        }

        public static TValue PutIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key, TValue value)
        {
            if (!originalDictionary.ContainsKey(key))
            {
                originalDictionary.Add(key, value);
            }

            return originalDictionary[key];
        }

        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return Sort(originalDictionary, SortOrder.Asending);
        }

        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, SortOrder order)
        {
            var data = (order == SortOrder.Asending) ? originalDictionary.OrderBy(pair => pair.Key) : originalDictionary.OrderByDescending(pair => pair.Key);
            var buf = new List<KeyValuePair<TKey, TValue>>();

            foreach (var item in data)
            {
                buf.Add(new KeyValuePair<TKey, TValue>(item.Key, item.Value));
            }

            originalDictionary.RemoveAll();
            originalDictionary.Clear();

            originalDictionary.AddOrUpdateAll(buf);

            return originalDictionary;
        }

        #endregion

        #region Extension for IDictionary<T1, T2>
        public static Boolean IsEmpty<T1, T2>(this IDictionary<T1, T2> originalDictionary)
        {
            return originalDictionary.Count == 0 ? true : false;
        }

        public static void AddOrUpdate<T1, T2>(this IDictionary<T1, T2> originalDictionary, T1 key, T2 value)
        {
            if (key != null)
            {
                if (originalDictionary.ContainsKey(key))
                    originalDictionary[key] = value;
                else
                    originalDictionary.Add(key, value);
            }
        }

        public static void AddOrUpdateAll<T1, T2>(this IDictionary<T1, T2> originalDictionary, IEnumerable<KeyValuePair<T1, T2>> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    AddOrUpdate(originalDictionary, item.Key, item.Value);
                }
            }
        }

        public static Boolean TryGetValueAtKey<T1, T2>(this IDictionary<T1, T2> originalDictionary, T1 key, out T2 value)
        {
            try
            {
                value = originalDictionary.GetValueAtKey(key);
                return true;
            }
            catch
            {
                value = default(T2);
                return false;
            }
        }

        public static T2 GetValueAtKey<T1, T2>(this IDictionary<T1, T2> originalDictionary, T1 key)
        {
            if (key != null)
            {
                if (originalDictionary.Any(x => x.Key.AreObjectsEqual(key)))
                {
                    return originalDictionary.FirstOrDefault(x => x.Key.AreObjectsEqual(key)).Value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public static Boolean TryGetKeyAtIndex<T1, T2>(this IDictionary<T1, T2> originalDictionary, int index, out T1 key)
        {
            try
            {
                key = originalDictionary.GetKeyAtIndex(index);
                return true;
            }
            catch
            {
                key = default(T1);
                return false;
            }
        }

        public static T1 GetKeyAtIndex<T1, T2>(this IDictionary<T1, T2> originalDictionary, int index)
        {
            var keys = originalDictionary.Keys.ToList();

            if (( 0 <= index) && (index < keys.Count))
            {
                return keys[index];
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public static IDictionary<T1, T2> Head<T1, T2>(this IDictionary<T1, T2> originalDictionary, T1 key, Boolean inclusive = false)
        {
            T1[] array = Enumerable.ToArray(originalDictionary.Keys);
            List<T1> tmp = new List<T1>();
            Comparer comparer = new Comparer(CultureInfo.InvariantCulture);

            int count = array.Count();
            for (int i = 0; i < count; i++)
            {
                int result = comparer.Compare(array[i], key);
                if ((result < 0) || (inclusive && result == 0))
                {
                    tmp.Add(array[i]);
                    continue;
                }
            }


            var resultDictionary = originalDictionary.Clone();
            resultDictionary.Clear();

            foreach (var k in tmp)
            {
                T2 v;
                _ = originalDictionary.TryGetValue(k, out v);
                resultDictionary.Add(k, v);
            }

            return (IDictionary<T1, T2>)resultDictionary;
        }

        public static IDictionary<T1, T2> Tail<T1, T2>(this IDictionary<T1, T2> originalDictionary, T1 key, Boolean inclusive = false)
        {
            T1[] array = Enumerable.ToArray(originalDictionary.Keys);
            List<T1> tmp = new List<T1>();
            Comparer comparer = new Comparer(CultureInfo.InvariantCulture);

            int count = array.Count();
            for (int i = 0; i < count; i++)
            {
                int result = comparer.Compare(array[i], key);
                if ((result > 0) || (inclusive && result == 0))
                {
                    tmp.Add(array[i]);
                    continue;
                }
            }


            var resultDictionary = originalDictionary.Clone();
            resultDictionary.Clear();

            foreach (var k in tmp)
            {
                T2 v;
                _ = originalDictionary.TryGetValue(k, out v);
                resultDictionary.Add(k, v);
            }

            return (IDictionary<T1, T2>)resultDictionary;
        }

        public static void Remove<T1, T2>(this IDictionary<T1, T2> originalDictionary, T1[] keysRemove)
        {
            foreach (var key in keysRemove)
            {
                originalDictionary.Remove(key);
            }
        }

        public static IDictionary<T1, T2> SubDictionary<T1, T2>(this IDictionary<T1, T2> originalDictionary, T1 startKey, Boolean inclusiveStartKey, T1 endKey, Boolean inclusiveEndKey)
        {
            var head = originalDictionary.Head(startKey, inclusiveStartKey);
            var tail = originalDictionary.Tail(endKey, inclusiveEndKey);
            var tmp = originalDictionary.Clone();

            tmp.Remove(head.Keys.ToArray<T1>());
            tmp.Remove(tail.Keys.ToArray<T1>());

            return tmp;
        }

        public static IDictionary<T1, T2> SubDictionary<T1, T2>(this IDictionary<T1, T2> originalDictionary, T1 startKeyInclusive, T1 endKeyInclusive)
        {
            var head = originalDictionary.Head(startKeyInclusive, false);
            var tail = originalDictionary.Tail(endKeyInclusive, false);
            var tmp = originalDictionary.Clone();

            tmp.Remove(head.Keys.ToArray<T1>());
            tmp.Remove(tail.Keys.ToArray<T1>());

            return tmp;
        }

        public static IDictionary<T1, T2> Clone<T1, T2>(this IDictionary<T1, T2> originalDictionary)
        {
            return originalDictionary.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        public static IDictionary<T1, T2> DeepClone<T1, T2>(this IDictionary<T1, T2> originalDictionary) where T2 : ICloneable
        {
            return originalDictionary.ToDictionary(entry => entry.Key, entry => (T2)entry.Value.Clone());
        }

        public static List<T1> ToKeysList<T1, T2>(this IDictionary<T1, T2> originalDictionary)
        {
            return new List<T1>(originalDictionary.Keys);
        }

        public static List<T2> ToValuesList<T1, T2>(this IDictionary<T1, T2> originalDictionary)
        {
            return new List<T2>(originalDictionary.Values);
        }


        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            var keys = originalDictionary.Keys.ToList();
            foreach (var key in keys)
            {
                originalDictionary.Remove(key);
            }
        }

        public static TValue GetSafe<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key)
        {
            try
            {
                return originalDictionary[key];
            }
            catch
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// Ensures that the receiver can hold at least the specified number of elements without needing to allocate new internal memory.
        /// If necessary, allocates new internal memory and increases the capacity of the receiver.
        /// <p>
        /// This method never need be called; it is for performance tuning only.
        /// Calling this method before<tt>put()</tt>ing a large number of associations boosts performance,
        /// because the receiver will grow only once instead of potentially many times.
        /// <p>
        /// <b>This default implementation does nothing.</b> Override this method if necessary.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic">Target <see cref="IDictionary{TKey, TValue}"/> object</param>
        /// <param name="minCapacity">the desired minimum capacity.</param>
        /// <returns><see cref="IDictionary{TKey, TValue}"/> object that allocated size</returns>
        public static IDictionary<TKey, TValue> EnsureCapacity<TKey, TValue>(this IDictionary<TKey, TValue> dic, int minCapacity)
        {
            if (dic.Count >= minCapacity)
                return dic;
            else
            {
                Dictionary<TKey, TValue> newDic = new Dictionary<TKey, TValue>(minCapacity);
                foreach (KeyValuePair<TKey, TValue> pair in dic)
                {
                    newDic.Add(pair.Key, pair.Value);
                }

                return (IDictionary<TKey, TValue>)newDic;
            }
        }

        /// <summary>
        /// Trim the excess items from the Dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> TrimExcess<TKey, TValue>(this IDictionary<TKey, TValue> dic)
        {
            var kv = new KeyValuePair<TKey, TValue>[dic.Count];
            dic.CopyTo(kv, 0);
            List<KeyValuePair<TKey, TValue>> l = kv.ToList();
            l.TrimExcess();
            var newDic = new Dictionary<TKey, TValue>(l.Count);
            AutoParallel.AutoParallelForEach(l, (p) =>
            {
                newDic.Add(p.Key, p.Value);
            });
            return newDic;
        }
        #endregion
    }
}
