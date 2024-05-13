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

// Accord Math Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2017
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Comparers
{
    /// <summary>
    ///   Stable comparer for stable sorting algorithm.
    /// </summary>
    /// 
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// 
    /// <remarks>
    ///   This class helps sort the elements of an array without swapping
    ///   elements which are already in order. This comprises a <c>stable</c>
    ///   sorting algorithm. This class is used by the <see cref="Accord.Math
    ///   .Tools.StableSort{T}(T[], out int[])"/> method to produce a stable sort
    ///   of its given arguments.
    /// </remarks>
    /// 
    /// <example>
    ///   In order to use this class, please use <see cref="Tools.StableSort{T}(T[],
    ///   out int[])"/>.
    /// </example>
    /// 
    /// <seealso cref="ElementComparer{T}"/>
    /// <seealso cref="ArrayComparer{T}"/>
    /// <seealso cref="GeneralComparer"/>
    /// <seealso cref="CustomComparer{T}"/>
    /// 
    public class StableComparer<T> : IComparer<KeyValuePair<int, T>>
    {
        private readonly Comparison<T> comparison;

        /// <summary>
        ///   Constructs a new instance of the <see cref="StableComparer&lt;T&gt;"/> class.
        /// </summary>
        /// 
        /// <param name="comparison">The comparison function.</param>
        /// 
        public StableComparer(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        /// <summary>
        ///   Compares two objects and returns a value indicating
        ///   whether one is less than, equal to, or greater than
        ///   the other.
        /// </summary>
        /// 
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// 
        /// <returns>A signed integer that indicates the relative values of x and y.</returns>
        /// 
        public int Compare(KeyValuePair<int, T> x, KeyValuePair<int, T> y)
        {
            int result = comparison(x.Value, y.Value);
            return result != 0 ? result : x.Key.CompareTo(y.Key);
        }
    }
}
