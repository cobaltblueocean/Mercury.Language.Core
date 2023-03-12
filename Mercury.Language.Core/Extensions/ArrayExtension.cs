﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ArrayExtension
    {

        /// <summary>
        ///   Creates a range vector (like NumPy's arange function).
        /// </summary>
        /// 
        /// <param name="n">The exclusive upper bound of the range.</param>
        ///
        /// <remarks>
        /// <para>
        ///   The Range methods should be equivalent to NumPy's np.arange method, with one
        ///   single difference: when the intervals are inverted (i.e. a > b) and the step
        ///   size is negative, the framework still iterates over the range backwards, as 
        ///   if the step was negative.</para>
        /// <para>
        ///   This function never includes the upper bound of the range. For methods
        ///   that include it, please see the <see cref="Interval(int, int)"/> methods.</para>  
        /// </remarks>
        ///
        /// <seealso cref="Interval(int, int)"/>
        ///
        private static int[] Range(int n)
        {
            int[] r = new int[(int)n];
            for (int i = 0; i < r.Length; i++)
                r[i] = (int)i;
            return r;
        }

        public static T[] Add<T>(this T[] array, int index, T value)
        {
            T[] target = new T[array.Length + 1];
            int j = 0;

            for (int i = 0; i < array.Length; i++)
            {
                if (i == index)
                {
                    target[j] = value;
                    j++;
                }
                target[j] = array[i];
                j++;
            }

            return target;
        }


        public static T[] Add<T>(this T[] array, T value)
        {
            T[] target = new T[array.Length + 1];
            array.CopyTo(target, 0);
            target[target.Length - 1] = value;

            return target;
        }


        public static T[] EnsureCapacity<T>(this T[] array, int minCapacity)
        {
            int oldCapacity = array.Length;
            T[] newArray;
            if (minCapacity > oldCapacity)
            {
                int newCapacity = (oldCapacity * 3) / 2 + 1;
                if (newCapacity < minCapacity)
                {
                    newCapacity = minCapacity;
                }

                newArray = new T[newCapacity];
                Array.Copy(array, 0, newArray, 0, oldCapacity);
            }
            else
            {
                newArray = array;
            }
            return newArray;
        }

        public static T[] SafeCopy<T>(this T[] array)
        {
            if (array != null)
            {
                var dist = new T[array.Length];
                Array.Copy(array, dist, array.Length);
                return dist;
            }
            else
                return null;
        }

        public static T[][] SafeCopy<T>(this T[][] array)
        {
            if (array != null)
            {
                var dist = new T[array.Length, array[0].Length];
                Array.Copy(array.ToMultidimensional(), dist, array.Length);
                return dist.ToJagged();
            }
            else
                return null;
        }

        public static T[][][] SafeCopy<T>(this T[][][] array)
        {
            if (array != null)
            {
                var dist = new T[array.Length, array[0].Length, array[0][0].Length];
                Array.Copy(array.ToMultidimensional(), dist, array.Length);
                return dist.ToJagged();
            }
            else
                return null;
        }

        /// <summary>
        /// Ensures that the specified array cannot hold more than <tt>maxCapacity</tt> elements.
        /// An application can use this operation to minimize array storage.
        /// <p>
        /// Returns the identical array if <tt>array.length &lt;= maxCapacity</tt>.
        /// Otherwise, returns a new array with a length of <tt>maxCapacity</tt>
        /// containing the first <tt>maxCapacity</tt> elements of <tt>array</tt>.
        /// 
        /// <summary>
        /// <param name=""> maxCapacity   the desired maximum capacity.</param>
        public static T[] TrimToCapacity<T>(this T[] array, int maxCapacity)
        {
            if (array.Length > maxCapacity)
            {
                T[] oldArray = array;
                array = new T[maxCapacity];
                oldArray.CopyTo(array, 0);
            }
            return array;
        }

        public static T[,] ToTransposedArray<T>(this T[,] array)
        {
            var rows = array.GetLength(0);
            var columns = array.GetLength(1);

            var result = new T[columns, rows];

            for (var i = 0; i < columns; i++)
            {
                for (var j = 0; j < rows; j++)
                {
                    result[i, j] = array[j, i];
                }
            }

            return result;
        }

        public static T[,,] ToTransposedArray<T>(this T[,,] array)
        {
            var rows = array.GetLength(0);
            var columns = array.GetLength(1);
            var width = array.GetLength(3);

            var result = new T[width, rows, columns];

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < rows; j++)
                {
                    for (var k = 0; k < columns; k++)
                    {
                        result[i, j, k] = array[k, j, i];
                    }
                }
            }

            return result;
        }

        public static T[][] ToJagged<T>(this T[,] array, bool transpose = false)
        {
            int row = array.GetLength(0);
            int col = array.GetLength(1);
            T[][] jagary;

            if (transpose)
            {
                jagary = new T[col][];
                for (int i = 0; i < col; i++)
                {
                    jagary[i] = new T[row];
                    for (int j = 0; j < row; j++)
                    {
                        jagary[i][j] = array[i, j];
                    }
                }
            }
            else
            {
                jagary = new T[row][];
                for (int i = 0; i < row; i++)
                {
                    jagary[i] = new T[col];
                    for (int j = 0; j < col; j++)
                    {
                        jagary[i][j] = array[i, j];
                    }
                }
            }

            return jagary;
        }

        public static T[][][] ToJagged<T>(this T[,,] array)
        {
            int slice = array.GetLength(0);

            T[][][] jagary = new T[slice][][];
            for (int i = 0; i < slice; i++)
            {
                int row = array.GetLength(1);
                jagary[i] = new T[row][];
                for (int j = 0; j < row; j++)
                {
                    int col = array.GetLength(2);
                    jagary[i][j] = new T[col];
                    for (int k = 0; k < col; k++)
                    {
                        jagary[i][j][k] = array[i, j, k];
                    }
                }
            }

            return jagary;
        }

        public static T[,] ToMultidimensional<T>(this T[][] array, bool transpose = false)
        {
            int row = array.Length;
            int col = array.GetMaxColumnLength();

            if (transpose)
            {
                T[,] mult = new T[col, row];

                for (int i = 0; i < col; i++)
                {
                    for (int j = 0; j < row; j++)
                    {
                        if (j < array[i].Length)
                            mult[i, j] = array[i][j];
                    }
                }

                return mult;
            }
            else
            {
                T[,] mult = new T[row, col];

                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)

                    {
                        if (j < array[i].Length)
                            mult[i, j] = array[i][j];
                    }
                }

                return mult;
            }
        }

        public static T[,,] ToMultidimensional<T>(this T[][][] array)
        {
            T[,,] mult = new T[array.GetLength(0), array.GetLength(1), array.GetLength(2)];
            int slice = array.GetLength(0);
            int row = array.GetLength(1);
            int col = array.GetLength(2);

            for (int i = 0; i < slice; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < col; k++)
                    {
                        mult[i, j, k] = array[i][j][k];
                    }
                }
            }

            return mult;
        }

        public static T[][] Initialize<T>(this T[][] array, int row, int col)
        {
            array = new T[row][];
            for (int i = 0; i < row; i++)
            {
                array[i] = new T[col];
            }

            return array;
        }

        public static T[][][] Initialize<T>(this T[][][] array, int slice, int row, int col)
        {
            array = new T[slice][][];
            for (int i = 0; i < slice; i++)
            {
                array[i] = new T[row][];
                for (int j = 0; j < row; j++)
                {
                    array[i][j] = new T[col];
                }
            }

            return array;
        }

        public static int GetMaxColumnLength<T>(this T[,] array)
        {
            return array.GetLength(1);
        }

        public static int GetMaxColumnLength<T>(this T[][] array)
        {
            int length = 0;
            foreach (T[] row in array)
            {
                if (length < row.Length)
                    length = row.Length;
            }
            return length;
        }
    }
}
