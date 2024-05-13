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

namespace Mercury.Language.Math.Matrix
{
    /// <summary>
    /// MatrixUtility Description
    /// </summary>
    public class MatrixUtility
    {
        /// <summary>
        ///   Creates a square matrix with ones across its diagonal.
        /// </summary>
        /// 
        public static double[,] Identity(int size)
        {
            return Diagonal(size, 1.0);
        }

        /// <summary>
        ///   Creates a square matrix with ones across its diagonal.
        /// </summary>
        /// 
        public static T[,] Identity<T>(int size) where T : struct
        {
            return Diagonal(size, 1.To<T>());
        }

        public static T[,] CreateAs<T>(T[,] matrix)
        {
            return new T[matrix.Rows(), matrix.Rows()];
        }

        public static T[,] CreateAs<T>(T[][] matrix)
        {
            return new T[matrix.Length, matrix[0].Length];
        }


        /// <summary>
        ///   Returns a square diagonal matrix of the given size.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(int size, T value)
        {
            return Diagonal(size, value, new T[size, size]);
        }

        /// <summary>
        ///   Returns a square diagonal matrix of the given size.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(int size, T value, T[,] result)
        {
            for (int i = 0; i < size; i++)
                result[i, i] = value;
            return result;
        }

        /// <summary>
        ///   Returns a matrix of the given size with value on its diagonal.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(int rows, int cols, T value)
        {
            return Diagonal(rows, cols, value, new T[rows, cols]);
        }

        /// <summary>
        ///   Returns a matrix of the given size with value on its diagonal.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(int rows, int cols, T value, T[,] result)
        {
            int min = System.Math.Min(rows, cols);
            for (int i = 0; i < min; i++)
                result[i, i] = value;
            return result;
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(T[] values)
        {
            return Diagonal(values, new T[values.Length, values.Length]);
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(T[] values, T[,] result)
        {
            for (int i = 0; i < values.Length; i++)
                result[i, i] = values[i];
            return result;
        }

        public static T[] Create<T>(int size, T value)
        {
            var v = new T[size];
            for (int i = 0; i < v.Length; i++)
                v[i] = value;
            return v;
        }

        public static T[][] Create<T>(int rows, int columns, params T[] values)
        {
            if (values.Length == 0)
            {
                T[][] matrix = new T[rows][];
                for (int i = 0; i < matrix.Length; i++)
                    matrix[i] = new T[columns];
                return matrix;
            }
            return values.Reshape(rows, columns).ToJagged();
        }

        /// <summary>
        ///   Creates a matrix with all values set to a given value.
        /// </summary>
        /// 
        /// <param name="rows">The number of rows in the matrix.</param>
        /// <param name="columns">The number of columns in the matrix.</param>
        /// <param name="values">The initial values for the matrix.</param>
        /// <param name="transpose">Whether to transpose the matrix when copying or not. Default is false.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
        public static T[,] Create<T>(int rows, int columns, T[,] values, bool transpose = false)
        {
            var result = new T[rows, columns];

            values.CopyTo( destination: result, transpose: transpose);
            return result;
        }


        /// <summary>
        ///   Creates a tensor with all values set to a given value.
        /// </summary>
        /// 
        /// <param name="shape">The number of dimensions that the matrix should have.</param>
        /// <param name="value">The initial values for the vector.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
        public static Array Create<T>(int[] shape, T value)
        {
            return Create(typeof(T), shape, value);
        }

        /// <summary>
        ///   Creates a tensor with all values set to a given value.
        /// </summary>
        /// 
        /// <param name="elementType">The type of the elements to be contained in the matrix.</param>
        /// <param name="shape">The number of dimensions that the matrix should have.</param>
        /// <param name="value">The initial values for the vector.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
        public static Array Create(Type elementType, int[] shape, object value)
        {
            Array arr = Array.CreateInstance(elementType, shape);
            foreach (int[] idx in arr.GetIndices())
                arr.SetValue(value, idx);
            return arr;
        }
    }
}
