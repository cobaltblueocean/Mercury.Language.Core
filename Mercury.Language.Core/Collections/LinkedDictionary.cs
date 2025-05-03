// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

/// <summary>
/// A class that implements the ADT dictionary by using a chain of nodes.
/// The dictionary is not sorted and has distinct search keys.
/// 
/// @author Frank Md Carrano
/// @version 2.0
/// <see href="https://github.com/marat00/Java/blob/master/LabX/LinkedDictionary.java"/>
/// <summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exceptions;
using Mercury.Language;

namespace System.Collections.Generic
{
    /// <summary>
    /// LinkedHashMap Description
    /// </summary>
    [Serializable]
    public class LinkedDictionary<K, V> : IDictionary<K, V>
    {

        private Node firstNode;   // reference to first node of chain
        private int currentSize; // number of entries 

        private IComparer<K> _comparer;
        private Boolean isIComparer = false;
        private StringComparer _stringComparer = StringComparer.InvariantCulture;

        public Boolean IsIComparer => isIComparer;

        public IComparer<K> Comparer
        {
            get { return _comparer; }
            set
            {
                _comparer = value;
                if (_comparer == null)
                    isIComparer = false;
            }
        }

        public LinkedDictionary() : this(Comparer<K>.Default)
        {

        }


        public LinkedDictionary(IComparer<K> comparer)
        {
            isIComparer = typeof(K).IsComparable();
            Comparer = comparer;

            firstNode = null;
            currentSize = 0;
        } // end default constructor


        public LinkedDictionary(LinkedDictionary<K, V> source)
        {
            isIComparer = typeof(K).IsComparable();
            Comparer = source.Comparer;

            var sourceItems = source.GetEnumerator();
            bool hasItem = true;
            if (sourceItems != null)
            {
                sourceItems.MoveNext();

                while (hasItem)
                {
                    Add(sourceItems.Current);
                    hasItem = sourceItems.MoveNext();
                }
            }
        }

        public LinkedDictionary(IDictionary<K, V> source)
        {
            isIComparer = typeof(K).IsComparable();
            Comparer = Comparer<K>.Default;

            foreach (var item in source)
            {
                Add(item.Key, item.Value);
            }
        }

        public Boolean IsEmpty
        {
            get { return currentSize == 0; }
        } // end isEmpty

        public Boolean IsFull
        {
            get { return false; }
        } // end isFull

        public int Size
        {
            get { return currentSize; }
        } // end getSize

        public ICollection<K> Keys
        {
            get
            {
                K[] keys = new K[Count];
                int i = 0;
                Node currentNode = firstNode;
                while (currentNode != null)
                {
                    keys[i] = currentNode.Key;
                    currentNode = currentNode.NextNode;
                    i++;
                } // end while
                return keys;
            }
        }

        public ICollection<V> Values
        {
            get
            {
                V[] values = new V[Count];
                int i = 0;
                Node currentNode = firstNode;
                while (currentNode != null)
                {
                    values[i] = currentNode.Value;
                    currentNode = currentNode.NextNode;
                    i++;
                } // end while
                return values;
            }
        }

        public ICollection<KeyValuePair<K, V>> Entries
        {
            get
            {
                KeyValuePair<K, V>[] entries = new KeyValuePair<K, V>[Count];
                int i = 0;
                Node currentNode = firstNode;
                while (currentNode != null)
                {
                    entries[i] = new KeyValuePair<K, V>(currentNode.Key, currentNode.Value);
                    currentNode = currentNode.NextNode;
                    i++;
                } // end while
                return entries;
            }
        }
        public int Count
        {
            get
            {
                int i = 0;
                Node currentNode = firstNode;
                while (currentNode != null)
                {
                    i++;
                    currentNode = currentNode.NextNode;
                } // end while

                return i;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }


        public V this[K key]
        {
            get
            {
                //V result;
                // find node before the one that contains or could contain key
                Node currentNode = firstNode;

                while ((currentNode != null) && !key.Equals(currentNode.Key))
                {
                    currentNode = currentNode.NextNode;
                } // end while

                if (currentNode != null)
                {
                    return currentNode.Value;
                }
                else
                {
                    throw new InvalidOperationException(LocalizedResources.Instance().LINKEDDICTIONARY_COULD_NOT_FIND_THE_KEY);
                }
            }
            set
            {
                // find node before the one that contains or could contain key
                Node currentNode = firstNode;

                while ((currentNode != null) && !key.Equals(currentNode.Key))
                {
                    currentNode = currentNode.NextNode;
                } // end while

                if (currentNode != null)
                {
                    currentNode.Value = value;
                }
                else
                {
                    throw new InvalidOperationException(LocalizedResources.Instance().LINKEDDICTIONARY_COULD_NOT_FIND_THE_KEY);
                }
            }
        }

        public void Clear()
        {
            firstNode = null;
            currentSize = 0;
        } // end clear

        public bool ContainsKey(KeyValuePair<K, V> item)
        {

            int index = -1;
            return Find(ref index, item.Key) == 0;
        }

        public bool ContainsKey(K key)
        {
            int index = -1;
            try
            {
                return Find(ref index, key) == 0;
            }
            catch
            {
                if (firstNode is null)
                    return false;
                else
                {
                    Node currentNode = firstNode;
                    while ((currentNode != null) && !key.Equals(currentNode.Key))
                    {
                        currentNode = currentNode.NextNode;
                    } // end while

                    if (currentNode == null)
                        return false;
                    else
                        return true;
                }
            }
        }

        /// <summary>
        /// returns 0 if found
        /// 
        /// when found, index will point to the found node
        /// </summary>        
        private int Find(ref int index, K key)
        {
            int last = -1;
            int cmp = -1;

            Node currentNode = firstNode;
            do
            {
                currentNode = currentNode.NextNode;
                cmp = CompareValue(key, currentNode.Key);
                last++;
            } while (cmp != 0);

            index = last;
            return cmp;
        }

        private int CompareValue(K value1, K value2)
        {
            if (IsIComparer)
                return Comparer.Compare(value1, value2);
            else
                return _stringComparer.Compare(value1.ToString(), value2.ToString());
        }

        public void Add(K key, V value)
        {
            // search chain for a node containing key
            if (!ContainsKey(key))
            {
                // key not in dictionary; add new node at beginning of chain
                Node newNode = new Node(key, value);
                newNode.NextNode = firstNode;
                firstNode = newNode;
                currentSize++;
            }
            else
            {
                // key in dictionary; replace corresponding value
                this[key] = value; // replace value
            } // end if
        }

        public bool Remove(K key)
        {
            if (!IsEmpty)
            {
                // search chain for a node containing key;
                // save reference to preceding node
                Node currentNode = firstNode;
                Node nodeBefore = null;

                while ((currentNode != null) && !key.Equals(currentNode.Key))
                {
                    nodeBefore = currentNode;
                    currentNode = currentNode.NextNode;
                } // end while

                if (currentNode != null)
                {
                    // node found; remove it
                    Node nodeAfter = currentNode.NextNode; // node after the one to be removed

                    if (nodeBefore == null)
                        firstNode = nodeAfter;
                    else
                        nodeBefore.NextNode = nodeAfter;        // disconnect the node to be removed

                    currentSize--;                              // decrease Length for both cases
                } // end if
            } // end if

            return true;
        }

        public bool TryGetValue(K key, out V value)
        {
            try
            {
                if (ContainsKey(key))
                {
                    value = this[key];
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }
            catch
            {
                value = default;
                return false;
            }
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            if (ContainsKey(item.Key))
            {
                if (!this[item.Key].Equals(item.Value))
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            int j = 0;
            int i = 0;

            var entries = Entries;
            foreach (var entry in entries)
            {
                if (j >= arrayIndex)
                {
                    array[i] = new KeyValuePair<K, V>(entry.Key, entry.Value);
                    i++;
                }
                j++;

                // If reach to end of LL, stop
                if (j == entries.Count)
                {
                    break;
                }
            }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            var entries = Entries;
            if (entries.Any(x => x.Key.Equals(item.Key) && x.Value.Equals(item.Value)))
            {
                Remove(item.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            Node currentNode = firstNode;
            while (currentNode != null)
            {
                yield return new KeyValuePair<K, V>(currentNode.Key, currentNode.Value);
                currentNode = currentNode.NextNode;
            } // end while
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public LinkedHashDictionary<K,V> ToLinkedHashDictionary()
        {
            var ret = new LinkedHashDictionary<K, V>();
            AutoParallel.AutoParallelForEach(Entries, (item) =>
            {

                ret.Add(item);
            });

            return ret;
        }

        [Serializable]
        public class Node
        {

            private K _key;
            private V _value;
            private Node _next;

            public Node(K searchKey, V dataValue)
            {
                _key = searchKey;
                _value = dataValue;
                _next = null;
            } // end constructor

            public Node(K searchKey, V dataValue, Node nextNode)
            {
                _key = searchKey;
                _value = dataValue;
                _next = nextNode;
            } // end constructor

            public K Key
            {
                get { return _key; }
            } // end getKey

            public V Value
            {
                get { return _value; }
                set { _value = value; }
            } // end getValue

            public Node NextNode
            {
                get { return _next; }
                set { _next = value; }
            } // end getNextNode
        } // end Node
    }
}
