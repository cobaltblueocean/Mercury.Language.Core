// Copyright (c) 2017 - presented by Kei Nakai
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

namespace System.Collections.Generic
{
    /// <summary>
    /// HashSetExtension Description
    /// </summary>
    public static class HashSetExtension
    {

        public static Nullable<T>[] ToNullableArray<T>(this ISet<T> val) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.ToArray().Cast<Nullable<T>>().ToArray();
        }

        public static T[] ToPremitiveArray<T>(this ISet<Nullable<T>> val) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.Cast<T>().ToArray();
        }

        public static ISet<T> Sort<T>(this ISet<T> val)
        {
            var sorted = new SortedSet<T>(val);
            val.Clear();
            val.AddAll(sorted);

            return val;
        }

        public static Boolean ValueEquals<T>(this ISet<T> val, ISet<T> target)
        {
            Boolean result = true;

            if (val.Count != target.Count)
                result = false;

            // Create a enumerable of the value in both Set.
            var _buf = val.Join(target, v => v, t => t, (v1, t1) => new { v1 });

            // if the count of both has different, means those 2 Sets' values weren't match
            if (_buf.Count() != val.Count)
                result = false;

            return result;
        }
    }
}
