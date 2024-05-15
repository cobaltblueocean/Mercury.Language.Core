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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language;

namespace System.Collections
{
    /// <summary>
    /// BitSetExtension Description
    /// </summary>
    public static class BitSetExtension
    {

        public static int Cardinality(this BitSet BitSet)
        {
            Int32[] ints = new Int32[(BitSet.Count >> 5) + 1];

            BitSet.CopyTo(ints, 0);

            Int32 count = 0;

            // fix for not truncated bits in last integer that may have been set to true with SetAll()
            ints[ints.Length - 1] &= ~(-1 << (BitSet.Count % 32));

            for (Int32 i = 0; i < ints.Length; i++)
            {
                Int32 c = ints[i];

                // magic (http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel)
                unchecked
                {
                    c = c - ((c >> 1) & 0x55555555);
                    c = (c & 0x33333333) + ((c >> 2) & 0x33333333);
                    c = ((c + (c >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
                }
                count += c;
            }

            return count;
        }

        public static int DefaultedLength(this BitSet BitSet)
        {
            for (int i = 0; i < BitSet.Count; i++)
            {
                if (BitSet[i])
                    return i + 1;
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the first bit that is set to {@code true}
        /// that occurs on or after the specified starting indexd If no such
        /// bit exists then {@code -1} is returned.
        /// 
        /// <p>To iterate over the {@code true} bits in a {@code BitSet},
        /// use the following loop:
        /// 
        ///  <pre> {@code
        /// AutoParallel.AutoParallelFor(int i = bs.nextSetBit(0); i >= 0; i = bs.nextSetBit(i+1)) {
        ///     // operate on index i here
        /// }}</pre>
        /// 
        /// <param Name="">fromIndex the index to start checking from (inclusive)</param>
        /// <returns>the index of the next set bit, or {@code -1} if there is no such bit</returns>
        /// <exception cref="IndexOutOfRangeException">if the specified index is negative </exception>
        /// @since  1.4
        /// </summary>
        public static int NextSetBit(this BitSet BitSet, int fromIndex)
        {
            if (fromIndex < 0)
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().BITARRAY_FROMINDEX_IS_NEGATIVE, fromIndex));

            int ret = -1;

            for (int i = fromIndex; i < BitSet.Count; i++)
            {
                if (BitSet.Get(i))
                {
                    ret = i;
                    break;
                }
            }

            return ret;
        }


        /// <summary>
        /// Returns the index of the first bit that is set to {@code false}
        /// that occurs on or after the specified starting index.
        /// <param Name="">fromIndex the index to start checking from (inclusive)</param>
        /// <returns>the index of the next clear bit</returns>
        /// <exception cref="IndexOutOfRangeException">if the specified index is negative </exception>
        /// @since  1.4
        /// </summary>

        public static int NextClearBit(this BitSet BitSet, int fromIndex)
        {

            if (fromIndex < 0)
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().BITARRAY_FROMINDEX_IS_NEGATIVE, fromIndex));

            int ret = -1;

            for (int i = fromIndex; i < BitSet.Count; i++)
            {
                if (!BitSet.Get(i))
                {
                    ret = i;
                    break;
                }
            }

            return ret;
        }


        /// <summary>
        /// Returns the index of the nearest bit that is set to {@code true}
        /// that occurs on or before the specified starting index.
        /// If no such bit exists, or if {@code -1} is given as the
        /// starting index, then {@code -1} is returned.
        /// <p>To iterate over the {@code true} bits in a {@code BitSet},
        /// use the following loop:
        /// 
        ///  <pre> {@code
        /// AutoParallel.AutoParallelFor(int i = bs.Length(); (i = bs.previousSetBit(i-1)) >= 0; ) {
        ///     // operate on index i here
        /// }}</pre>
        /// 
        /// <param Name="">fromIndex the index to start checking from (inclusive)</param>
        /// <returns>the index of the previous set bit, or {@code -1} if there is no such bit</returns>
        /// <exception cref="IndexOutOfRangeException">if the specified index is less  than {@code -1}</exception>
        /// @since  1.7
        /// </summary>

        public static int PreviousSetBit(this BitSet BitSet, int fromIndex)
        {

            if (fromIndex < 0)
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().BITARRAY_FROMINDEX_IS_NEGATIVE, fromIndex));

            int ret = -1;

            for (int i = fromIndex; i >= 0; i--)
            {
                if (BitSet.Get(i))
                {
                    ret = i;
                    break;
                }
            }

            return ret;
        }


        /// <summary>
        /// Returns the index of the nearest bit that is set to {@code false}
        /// that occurs on or before the specified starting index.
        /// If no such bit exists, or if {@code -1} is given as the
        /// starting index, then {@code -1} is returned.
        /// <param Name="">fromIndex the index to start checking from (inclusive)</param>
        /// <returns>the index of the previous clear bit, or {@code -1} if there is no such bit</returns>
        /// <exception cref="IndexOutOfRangeException">if the specified index is less than {@code -1}</exception>
        /// @since  1.7
        /// </summary>
        public static int PreviousClearBit(this BitSet BitSet, int fromIndex)
        {
            if (fromIndex < 0)
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().BITARRAY_FROMINDEX_IS_NEGATIVE, fromIndex));

            int ret = -1;

            for (int i = fromIndex; i >= 0; i--)
            {
                if (!BitSet.Get(i))
                {
                    ret = i;
                    break;
                }
            }

            return ret;
        }

    }
}
