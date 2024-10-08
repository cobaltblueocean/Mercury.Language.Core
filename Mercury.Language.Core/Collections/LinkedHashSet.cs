﻿// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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
    /// LinkedHashSet Description
    /// Ref: StackOverflow
    /// https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
    /// </summary>
    public class LinkedHashSet<T> : ISet<T>
    {
        private readonly IDictionary<T, LinkedListNode<T>> dict;
        private readonly LinkedList<T> list;

        public LinkedHashSet(int initialCapacity)
        {
            this.dict = new Dictionary<T, LinkedListNode<T>>(initialCapacity);
            this.list = new LinkedList<T>();
        }

        public LinkedHashSet()
        {
            this.dict = new Dictionary<T, LinkedListNode<T>>();
            this.list = new LinkedList<T>();
        }

        public LinkedHashSet(IEnumerable<T> e) : this()
        {
            addEnumerable(e);
        }

        public LinkedHashSet(int initialCapacity, IEnumerable<T> e) : this(initialCapacity)
        {
            addEnumerable(e);
        }

        private void addEnumerable(IEnumerable<T> e)
        {
            foreach (T t in e)
            {
                Add(t);
            }
        }

        //
        // ISet implementation
        //

        public bool Add(T item)
        {
            if (this.dict.ContainsKey(item))
            {
                return false;
            }
            LinkedListNode<T> node = this.list.AddLast(item);
            this.dict[item] = node;
            return true;
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            foreach (T t in other)
            {
                Remove(t);
            }
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            T[] ts = new T[Count];
            CopyTo(ts, 0);
            foreach (T t in ts)
            {
                if (!System.Linq.Enumerable.Contains(other, t))
                {
                    Remove(t);
                }
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            int contains = 0;
            int noContains = 0;
            foreach (T t in other)
            {
                if (Contains(t))
                {
                    contains++;
                }
                else
                {
                    noContains++;
                }
            }
            return contains == Count && noContains > 0;
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            int otherCount = System.Linq.Enumerable.Count(other);
            if (Count <= otherCount)
            {
                return false;
            }
            int contains = 0;
            int noContains = 0;
            foreach (T t in this)
            {
                if (System.Linq.Enumerable.Contains(other, t))
                {
                    contains++;
                }
                else
                {
                    noContains++;
                }
            }
            return contains == otherCount && noContains > 0;
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            foreach (T t in this)
            {
                if (!System.Linq.Enumerable.Contains(other, t))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            foreach (T t in other)
            {
                if (!Contains(t))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            foreach (T t in other)
            {
                if (Contains(t))
                {
                    return true;
                }
            }
            return false;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            int otherCount = System.Linq.Enumerable.Count(other);
            if (Count != otherCount)
            {
                return false;
            }
            return IsSupersetOf(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            T[] ts = new T[Count];
            CopyTo(ts, 0);
            HashSet<T> otherList = new HashSet<T>(other);
            foreach (T t in ts)
            {
                if (otherList.Contains(t))
                {
                    Remove(t);
                    otherList.Remove(t);
                }
            }
            foreach (T t in otherList)
            {
                Add(t);
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().LINKEDHASHSET_OTHER_CANNOT_BE_NULL);
            }
            foreach (T t in other)
            {
                Add(t);
            }
        }

        //
        // ICollection<T> implementation
        //

        public int Count
        {
            get
            {
                return this.dict.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.dict.IsReadOnly;
            }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            this.dict.Clear();
            this.list.Clear();
        }

        public bool Contains(T item)
        {
            return this.dict.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            LinkedListNode<T> node;
            if (!this.dict.TryGetValue(item, out node))
            {
                return false;
            }
            this.dict.Remove(item);
            this.list.Remove(node);
            return true;
        }

        public bool RemoveAll(IEnumerable<T> items)
        {
            bool result = true;
            AutoParallel.AutoParallelForEach(items, (item) =>
            {
                if(!Remove(item))
                    result = false;
            });
            return result;
        }

        //
        // IEnumerable<T> implementation
        //

        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        //
        // IEnumerable implementation
        //

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

    }
}
