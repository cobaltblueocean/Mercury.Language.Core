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


        #region Extension for IDictionary<TKey, TValue>
        /// <summary>
        /// Add a dictionary's KeyValuePair entries from another dictionary
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="addingDictionary">Dictionary to add</param>
        /// <exception cref="NullReferenceException"></exception>
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

        /// <summary>
        /// Safe way to evaluate if the Key exists in the dictionary
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="key">Key to find</param>
        /// <returns>Returns true if the key exists, otherwise returns false</returns>
        public static Boolean ContainsKeyEx<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key)
        {
            return originalDictionary.Keys.Any(x => x.AreObjectsEqual(key));
        }

        /// <summary>
        /// Get a HashSet of Key
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns>A HashSet of Key</returns>
        public static HashSet<TKey> KeysToHashSet<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return new HashSet<TKey>(originalDictionary.Keys.ToList());
        }

        /// <summary>
        /// Get a HashSet of the Value
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns>A HashSet of the Value</returns>
        public static HashSet<TValue> ValuesToHashSet<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return new HashSet<TValue>(originalDictionary.Values.ToList());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns></returns>
        public static List<TKey> KeysToList<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return new List<TKey>(originalDictionary.Keys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns></returns>
        public static List<TValue> ValuesToList<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return new List<TValue>(originalDictionary.Values);
        }

        /// <summary>
        /// Return a ReadOnlyDictionary from the original Dictionary
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns>A ReadOnlyDictionary contains same entries from original Dictionary</returns>
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary) where TKey : notnull
        {
            return new ReadOnlyDictionary<TKey, TValue>(originalDictionary);
        }

        /// <summary>
        /// Add the Key and Value if the key doesn't exist
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Target Dictionary</param>
        /// <param name="key">Key to add</param>
        /// <param name="value">Value to supply</param>
        /// <returns>The value added</returns>
        public static TValue PutIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key, TValue value)
        {
            if (!originalDictionary.ContainsKey(key))
            {
                originalDictionary.Add(key, value);
            }

            return originalDictionary[key];
        }

        /// <summary>
        /// Sort by Key
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns>Sorted Dictionary</returns>
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return Sort(originalDictionary, SortOrder.Asending);
        }

        /// <summary>
        /// Sorted by Key, and Order
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="order">Sort order</param>
        /// <returns>Sorted Dictionary</returns>
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


        /// <summary>
        /// Check if the Dictionary is empty
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns>True if the dictionary is empty, otherwise false</returns>
        public static Boolean IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return originalDictionary.Count == 0 ? true : false;
        }

        /// <summary>
        /// Add the Key and Value to the Dictionary.  If the Key exists, update the value with new supplied value
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Target Dictionary</param>
        /// <param name="key">Key to add or update</param>
        /// <param name="value">Value to supply</param>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key, TValue value)
        {
            if (key != null)
            {
                if (originalDictionary.ContainsKey(key))
                    originalDictionary[key] = value;
                else
                    originalDictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Add the set of Key and Value pairs to the Dictionary.  If the Key exists, update the value with new supplied value
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Target Dictionary</param>
        /// <param name="key">Key to add or update</param>
        /// <param name="value">Value to supply</param>
        public static void AddOrUpdateAll<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    AddOrUpdate(originalDictionary, item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// Safe way to get the value from the dictionary by given key
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="key">Key to find</param>
        /// <returns>Returns the value if the key exists, otherwise returns null or default value</returns>
        public static Boolean TryGetValueAtKey<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key, out TValue value)
        {
            try
            {
                value = originalDictionary.GetValueAtKey(key);
                return true;
            }
            catch
            {
                value = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// Get the value from the dictionary by given key
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="key">Key to find</param>
        /// <returns>Returns the value if the key exists, otherwise throw an exception</returns>
        public static TValue GetValueAtKey<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key)
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

        /// <summary>
        /// Safe way to get the Key by index
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="index">Index to get the key</param>
        /// <param name="key">Key that found at the index</param>
        /// <returns>True if the index exists</returns>
        public static Boolean TryGetKeyAtIndex<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, int index, out TKey key)
        {
            try
            {
                key = originalDictionary.GetKeyAtIndex(index);
                return true;
            }
            catch
            {
                key = default(TKey);
                return false;
            }
        }

        /// <summary>
        /// Get the key at the index
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="index">Index to get the key</param>
        /// <returns>Key at the index</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static TKey GetKeyAtIndex<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, int index)
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="key"></param>
        /// <param name="inclusive"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Head<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key, Boolean inclusive = false)
        {
            TKey[] array = Enumerable.ToArray(originalDictionary.Keys);
            List<TKey> tmp = new List<TKey>();
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
                TValue v;
                _ = originalDictionary.TryGetValue(k, out v);
                resultDictionary.Add(k, v);
            }

            return (IDictionary<TKey, TValue>)resultDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="key"></param>
        /// <param name="inclusive"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Tail<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key, Boolean inclusive = false)
        {
            TKey[] array = Enumerable.ToArray(originalDictionary.Keys);
            List<TKey> tmp = new List<TKey>();
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
                TValue v;
                _ = originalDictionary.TryGetValue(k, out v);
                resultDictionary.Add(k, v);
            }

            return (IDictionary<TKey, TValue>)resultDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="keysRemove"></param>
        public static void Remove<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey[] keysRemove)
        {
            foreach (var key in keysRemove)
            {
                originalDictionary.Remove(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="startKey"></param>
        /// <param name="inclusiveStartKey"></param>
        /// <param name="endKey"></param>
        /// <param name="inclusiveEndKey"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> SubDictionary<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey startKey, Boolean inclusiveStartKey, TKey endKey, Boolean inclusiveEndKey)
        {
            var head = originalDictionary.Head(startKey, inclusiveStartKey);
            var tail = originalDictionary.Tail(endKey, inclusiveEndKey);
            var tmp = originalDictionary.Clone();

            tmp.Remove(head.Keys.ToArray<TKey>());
            tmp.Remove(tail.Keys.ToArray<TKey>());

            return tmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="startKeyInclusive"></param>
        /// <param name="endKeyInclusive"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> SubDictionary<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey startKeyInclusive, TKey endKeyInclusive)
        {
            var head = originalDictionary.Head(startKeyInclusive, false);
            var tail = originalDictionary.Tail(endKeyInclusive, false);
            var tmp = originalDictionary.Clone();

            tmp.Remove(head.Keys.ToArray<TKey>());
            tmp.Remove(tail.Keys.ToArray<TKey>());

            return tmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return originalDictionary.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> DeepClone<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary) where TValue : ICloneable
        {
            return originalDictionary.ToDictionary(entry => entry.Key, entry => (TValue)entry.Value.Clone());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            var keys = originalDictionary.Keys.ToList();
            foreach (var key in keys)
            {
                originalDictionary.Remove(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <param name="key"></param>
        /// <returns></returns>
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
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Dictionary to evaluate</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> TrimExcess<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            var kv = new KeyValuePair<TKey, TValue>[originalDictionary.Count];
            originalDictionary.CopyTo(kv, 0);
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
