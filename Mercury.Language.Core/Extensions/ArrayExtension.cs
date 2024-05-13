using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using Mercury.Language.Core;
using Mercury.Language.Comparers;
using Mercury.Language.Exceptions;
using Mercury.Language.Math;
using Mercury.Language.Math.Matrix;

namespace System
{
    public static class ArrayExtension
    {
        #region Private Methods

        private static T[,] CreateAs<T>(T[,] matrix)
        {
            return new T[matrix.Rows(), matrix.Rows()];
        }

        private static T[,] CreateAs<T>(T[][] matrix)
        {
            return new T[matrix.Length, matrix[0].Length];
        }

        #endregion
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


        #region Premitive Array
        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this T[] array1, T[] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[] array1, T[] array2, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>(), message, paramStr);
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        public static Boolean ValueEquals<T>(this T[] array1, T[] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (!NearlyEquals(array1[i], array2[i], tolerance))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[] array1, T[] array2, T tolerance, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (!NearlyEquals(array1[i], array2[i], tolerance))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this T[,] array1, T[,] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[,] array1, T[,] array2, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>(), message, paramStr);
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        public static Boolean ValueEquals<T>(this T[,] array1, T[,] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            if (array1.GetLength(1) != array2.GetLength(1))
            {
                return false;
            }

            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    if (!NearlyEquals(array1[i, j], array2[i, j], tolerance))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[,] array1, T[,] array2, T tolerance, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            if (array1.GetLength(1) != array2.GetLength(1))
            {
                return false;
            }

            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    if (!NearlyEquals(array1[i, j], array2[i, j], tolerance))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this T[,,] array1, T[,,] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[,,] array1, T[,,] array2, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>(), message, paramStr);
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        public static Boolean ValueEquals<T>(this T[,,] array1, T[,,] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            if (array1.GetLength(1) != array2.GetLength(1))
            {
                return false;
            }

            if (array1.GetLength(2) != array2.GetLength(2))
            {
                return false;
            }

            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    for (int k = 0; k < array1.GetLength(2); k++)
                    {
                        if (!NearlyEquals(array1[i, j, k], array2[i, j, k], tolerance))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[,,] array1, T[,,] array2, T tolerance, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            if (array1.GetLength(1) != array2.GetLength(1))
            {
                return false;
            }

            if (array1.GetLength(2) != array2.GetLength(2))
            {
                return false;
            }

            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    for (int k = 0; k < array1.GetLength(2); k++)
                    {
                        if (!NearlyEquals(array1[i, j, k], array2[i, j, k], tolerance))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this T[][] array1, T[][] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[][] array1, T[][] array2, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>(), message, paramStr);
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        public static Boolean ValueEquals<T>(this T[][] array1, T[][] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Length != array2[i].Length)
                {
                    return false;
                }

                for (int j = 0; j < array1[i].Length; j++)
                {
                    if (!NearlyEquals(array1[i][j], array2[i][j], tolerance))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[][] array1, T[][] array2, T tolerance, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Length != array2[i].Length)
                {
                    return false;
                }

                for (int j = 0; j < array1[i].Length; j++)
                {
                    if (!NearlyEquals(array1[i][j], array2[i][j], tolerance))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this T[][][] array1, T[][][] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[][][] array1, T[][][] array2, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>(), message, paramStr);
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        public static Boolean ValueEquals<T>(this T[][][] array1, T[][][] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Length != array2[i].Length)
                {
                    return false;
                }

                for (int j = 0; j < array1[i].Length; j++)
                {
                    if (array1[i].Length != array2[i].Length)
                    {
                        return false;
                    }

                    for (int k = 0; k < array1[i][j].Length; k++)
                    {
                        if (!NearlyEquals(array1[i][j][k], array2[i][j][k], tolerance))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this T[][][] array1, T[][][] array2, T tolerance, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Length != array2[i].Length)
                {
                    return false;
                }

                for (int j = 0; j < array1[i].Length; j++)
                {
                    if (array1[i].Length != array2[i].Length)
                    {
                        return false;
                    }

                    for (int k = 0; k < array1[i][j].Length; k++)
                    {
                        if (!NearlyEquals(array1[i][j][k], array2[i][j][k], tolerance))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region Nullable Array
        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[] array1, Nullable<T>[] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[] array1, Nullable<T>[] array2, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].HasValue && array2[i].HasValue)
                {
                    if (!NearlyEquals(array1[i].Value, array2[i].Value, Zero<T>()))
                    {
                        return false;
                    }
                }
                else if (!array1[i].HasValue && array2[i].HasValue)
                {
                    return false;
                }
                else if (array1[i].HasValue && !array2[i].HasValue)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[] array1, Nullable<T>[] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].HasValue && array2[i].HasValue)
                {
                    if (!NearlyEquals(array1[i].Value, array2[i].Value, tolerance))
                    {
                        return false;
                    }
                }
                else if (!array1[i].HasValue && array2[i].HasValue)
                {
                    return false;
                }
                else if (array1[i].HasValue && !array2[i].HasValue)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[,] array1, Nullable<T>[,] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[,] array1, Nullable<T>[,] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            if (array1.GetLength(1) != array2.GetLength(1))
            {
                return false;
            }

            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    if (array1[i, j].HasValue && array2[i, j].HasValue)
                    {
                        if (!NearlyEquals(array1[i, j].Value, array2[i, j].Value, tolerance))
                        {
                            return false;
                        }
                    }
                    else if (!array1[i, j].HasValue && array2[i, j].HasValue)
                    {
                        return false;
                    }
                    else if (array1[i, j].HasValue && !array2[i, j].HasValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[,] array1, Nullable<T>[,] array2, T tolerance, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            if (array1.GetLength(1) != array2.GetLength(1))
            {
                return false;
            }

            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    if (array1[i, j].HasValue && array2[i, j].HasValue)
                    {
                        if (!NearlyEquals(array1[i, j].Value, array2[i, j].Value, tolerance))
                        {
                            return false;
                        }
                    }
                    else if (!array1[i, j].HasValue && array2[i, j].HasValue)
                    {
                        return false;
                    }
                    else if (array1[i, j].HasValue && !array2[i, j].HasValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[,,] array1, Nullable<T>[,,] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[,,] array1, Nullable<T>[,,] array2, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>(), message, paramStr);
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[,,] array1, Nullable<T>[,,] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            if (array1.GetLength(1) != array2.GetLength(1))
            {
                return false;
            }

            if (array1.GetLength(2) != array2.GetLength(2))
            {
                return false;
            }

            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    for (int k = 0; k < array1.GetLength(2); k++)
                    {
                        if (array1[i, j, k].HasValue && array2[i, j, k].HasValue)
                        {
                            if (!NearlyEquals(array1[i, j, k].Value, array2[i, j, k].Value, tolerance))
                            {
                                return false;
                            }
                        }
                        else if (!array1[i, j, k].HasValue && array2[i, j, k].HasValue)
                        {
                            return false;
                        }
                        else if (array1[i, j, k].HasValue && !array2[i, j, k].HasValue)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[,,] array1, Nullable<T>[,,] array2, T tolerance, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            if (array1.GetLength(1) != array2.GetLength(1))
            {
                return false;
            }

            if (array1.GetLength(2) != array2.GetLength(2))
            {
                return false;
            }

            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    for (int k = 0; k < array1.GetLength(2); k++)
                    {
                        if (array1[i, j, k].HasValue && array2[i, j, k].HasValue)
                        {
                            if (!NearlyEquals(array1[i, j, k].Value, array2[i, j, k].Value, tolerance))
                            {
                                return false;
                            }
                        }
                        else if (!array1[i, j, k].HasValue && array2[i, j, k].HasValue)
                        {
                            return false;
                        }
                        else if (array1[i, j, k].HasValue && !array2[i, j, k].HasValue)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[][] array1, Nullable<T>[][] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[][] array1, Nullable<T>[][] array2, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>(), message, paramStr);
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[][] array1, Nullable<T>[][] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Length != array2[i].Length)
                {
                    return false;
                }

                for (int j = 0; j < array1[i].Length; j++)
                {
                    if (array1[i][j].HasValue && array2[i][j].HasValue)
                    {
                        if (!NearlyEquals(array1[i][j].Value, array2[i][j].Value, tolerance))
                        {
                            return false;
                        }
                    }
                    else if (!array1[i][j].HasValue && array2[i][j].HasValue)
                    {
                        return false;
                    }
                    else if (array1[i][j].HasValue && !array2[i][j].HasValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[][] array1, Nullable<T>[][] array2, T tolerance, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Length != array2[i].Length)
                {
                    return false;
                }

                for (int j = 0; j < array1[i].Length; j++)
                {
                    if (array1[i][j].HasValue && array2[i][j].HasValue)
                    {
                        if (!NearlyEquals(array1[i][j].Value, array2[i][j].Value, tolerance))
                        {
                            return false;
                        }
                    }
                    else if (!array1[i][j].HasValue && array2[i][j].HasValue)
                    {
                        return false;
                    }
                    else if (array1[i][j].HasValue && !array2[i][j].HasValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[][][] array1, Nullable<T>[][][] array2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>());
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[][][] array1, Nullable<T>[][][] array2, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ValueEquals<T>(array1, array2, Zero<T>(), message, paramStr);
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[][][] array1, Nullable<T>[][][] array2, T tolerance) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Length != array2[i].Length)
                {
                    return false;
                }

                for (int j = 0; j < array1[i].Length; j++)
                {
                    if (array1[i].Length != array2[i].Length)
                    {
                        return false;
                    }

                    for (int k = 0; k < array1[i][j].Length; k++)
                    {
                        if (array1[i][j][j].HasValue && array2[i][j][j].HasValue)
                        {
                            if (!NearlyEquals(array1[i][j][k].Value, array2[i][j][k].Value, tolerance))
                            {
                                return false;
                            }
                        }
                        else if (!array1[i][j][j].HasValue && array2[i][j][j].HasValue)
                        {
                            return false;
                        }
                        else if (array1[i][j][j].HasValue && !array2[i][j][j].HasValue)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the properties of two arrays of the same type and returns if any properties are different.
        /// </summary>
        /// <typeparam name="T">A type to compare.</typeparam>
        /// <param name="array1">The first array to compare.</param>
        /// <param name="array2">The second array to compare.</param>
        /// <param name="tolerance">A tolerance to the 2 values within.</param>
        /// <param name="message">Message to display when not passed</param>
        /// <param name="paramStr">Parameters to enclose in the message</param>
        public static Boolean ValueEquals<T>(this Nullable<T>[][][] array1, Nullable<T>[][][] array2, T tolerance, String message, params String[] paramStr) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Length != array2[i].Length)
                {
                    return false;
                }

                for (int j = 0; j < array1[i].Length; j++)
                {
                    if (array1[i].Length != array2[i].Length)
                    {
                        return false;
                    }

                    for (int k = 0; k < array1[i][j].Length; k++)
                    {
                        if (array1[i][j][j].HasValue && array2[i][j][j].HasValue)
                        {
                            if (!NearlyEquals(array1[i][j][k].Value, array2[i][j][k].Value, tolerance))
                            {
                                return false;
                            }
                        }
                        else if (!array1[i][j][j].HasValue && array2[i][j][j].HasValue)
                        {
                            return false;
                        }
                        else if (array1[i][j][j].HasValue && !array2[i][j][j].HasValue)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Default or Zero value of the Type
        /// </summary>
        /// <typeparam name="T">Type to get the default value</typeparam>
        /// <returns></returns>
        public static T Zero<T>()
        {
            //return (T)(Object)0;
            return default(T);
        }

        /// <summary>
        /// Default or Eps value of the Type
        /// </summary>
        /// <typeparam name="T">Type to get the default value</typeparam>
        /// <returns></returns>
        public static T Eps<T>()
        {
            return (T)(Object)1e-5;
        }

        #region "Private logic"

        /// <summary>
        /// Compares the properties of two objects of the same type and returns if all properties are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compre.</param>
        /// <param name="Esp">A tolerance to the 2 values within.</param>
        /// <returns><c>true</c> if all property values are equal, otherwise <c>false</c>.</returns>
        /// <see cref="https://www.cyotek.com/blog/comparing-the-properties-of-two-objects-via-reflection"/>
        private static Boolean NearlyEquals<T>(T x, T y, T Esp) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (Esp.Equals(Zero<T>()))
                return x.Equals(y);
            else
            {
                // Declare the parameters
                var paramX = Expression.Parameter(typeof(T), "x");
                var paramY = Expression.Parameter(typeof(T), "y");

                // Condition
                var greaterThan = Expression.GreaterThan(paramX, paramY);

                // True clause
                var trueClause = Expression.Subtract(paramX, paramY);

                // False claise
                var falseClause = Expression.Subtract(paramY, paramX);

                var conditional = Expression.Condition(greaterThan, trueClause, falseClause);

                // Compile it
                Func<T, T, T> subtractAbs = Expression.Lambda<Func<T, T, T>>(conditional, paramX, paramY).Compile();

                // Call it
                T abs = subtractAbs(x, y);

                var paramAbs = Expression.Parameter(typeof(T), "abs");
                var paramEsp = Expression.Parameter(typeof(T), "Esp");

                BinaryExpression body = Expression.LessThan(paramAbs, paramEsp);
                Func<T, T, bool> compare = Expression.Lambda<Func<T, T, bool>>(body, paramAbs, paramEsp).Compile();

                return compare(abs, Esp);
            }
        }
        #endregion

        #endregion



        /// <summary>
        ///   Returns a copy of an array in reversed order.
        /// </summary>
        /// 
        public static T[] First<T>(this T[] values, int count)
        {
            var r = new T[count];
            for (int i = 0; i < r.Length; i++)
                r[i] = values[i];
            return r;
        }


        /// <summary>
        ///   Returns a subvector extracted from the current vector.
        /// </summary>
        /// 
        /// <param name="source">The vector to return the subvector from.</param>
        /// <param name="indexes">Array of indices.</param>
        /// <param name="inPlace">True to return the results in place, changing the
        ///   original <paramref name="source"/> vector; false otherwise.</param>
        /// 
        public static T[] Get<T>(this T[] source, int[] indexes, bool inPlace = false)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (indexes == null)
                throw new ArgumentNullException("indexes");

            if (inPlace && source.Length != indexes.Length)
                throw new ArgumentNullException(LocalizedResources.Instance().SOURCE_AND_INDEXES_ARRAYS_MUST_HAVE_THE_SAME_DIMENSION);

            var destination = new T[indexes.Length];
            for (int i = 0; i < indexes.Length; i++)
            {
                int j = indexes[i];
                if (j >= 0)
                    destination[i] = source[j];
                else
                    destination[i] = source[source.Length + j];
            }

            if (inPlace)
            {
                for (int i = 0; i < destination.Length; i++)
                    source[i] = destination[i];
            }

            return destination;
        }

        /// <summary>
        ///   Returns a sub matrix extracted from the current matrix.
        /// </summary>
        /// 
        /// <param name="source">The matrix to return the submatrix from.</param>
        /// <param name="rowIndexes">Array of row indices. Pass null to select all indices.</param>
        /// <param name="columnIndexes">Array of column indices. Pass null to select all indices.</param>
        /// <param name="result">An optional matrix where the results should be stored.</param>
        /// 
        public static T[,] Get<T>(this T[,] source, int[] rowIndexes, int[] columnIndexes, T[,] result = null)
        {
            return Get(source, result, rowIndexes, columnIndexes);
        }

        /// <summary>
        ///   Extracts a selected area from a matrix.
        /// </summary>
        /// 
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        /// 
        private static T[,] Get<T>(this T[,] source, T[,] destination, int[] rowIndexes, int[] columnIndexes)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int rows = source.Rows();
            int cols = source.GetMaxColumnLength();

            int newRows = rows;
            int newCols = cols;

            if (rowIndexes == null && columnIndexes == null)
            {
                return source;
            }

            if (rowIndexes != null)
            {
                newRows = rowIndexes.Length;
                for (int i = 0; i < rowIndexes.Length; i++)
                    if ((rowIndexes[i] < 0) || (rowIndexes[i] >= rows))
                        throw new ArgumentException(LocalizedResources.Instance().ARGUMENT_OUT_OF_RANGE);
            }
            if (columnIndexes != null)
            {
                newCols = columnIndexes.Length;
                for (int i = 0; i < columnIndexes.Length; i++)
                    if ((columnIndexes[i] < 0) || (columnIndexes[i] >= cols))
                        throw new ArgumentException(LocalizedResources.Instance().ARGUMENT_OUT_OF_RANGE);
            }


            if (destination != null)
            {
                if (destination.Rows() < newRows || destination.GetMaxColumnLength() < newCols)
                    throw new ArgumentException("destination", LocalizedResources.Instance().THE_DESTINATION_MATRIX_MUST_BE_BIG_ENOUGH);
            }
            else
            {
                destination = new T[newRows, newCols];
            }

            if (columnIndexes == null)
            {
                for (int i = 0; i < rowIndexes.Length; i++)
                    for (int j = 0; j < cols; j++)
                        destination[i, j] = source[rowIndexes[i], j];
            }
            else if (rowIndexes == null)
            {
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columnIndexes.Length; j++)
                        destination[i, j] = source[i, columnIndexes[j]];
            }
            else
            {
                for (int i = 0; i < rowIndexes.Length; i++)
                    for (int j = 0; j < columnIndexes.Length; j++)
                        destination[i, j] = source[rowIndexes[i], columnIndexes[j]];
            }

            return destination;
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
        /// <summary>
        ///   Directions for the General Comparer.
        /// </summary>
        /// 
        public enum ComparerDirection
        {
            /// <summary>
            ///   Sorting will be performed in ascending order.
            /// </summary>
            /// 
            Ascending = +1,

            /// <summary>
            ///   Sorting will be performed in descending order.
            /// </summary>
            /// 
            Descending = -1
        };


        /// <summary>
        ///   Gets the indices that sort a vector.
        /// </summary>
        /// 
        public static int[] ArgSort<T>(this T[] values)
            where T : IComparable<T>
        {
            int[] idx;
            values.Copy().Sort(out idx);
            return idx;
        }

        /// <summary>
        ///   Sorts the elements of an entire one-dimensional array using the given comparison.
        /// </summary>
        /// 
        public static void Sort<T>(this T[] values, out int[] order, bool stable = false, ComparerDirection direction = ComparerDirection.Ascending)
            where T : IComparable<T>
        {
            if (!stable)
            {
                order = Range(values.Length);
                Array.Sort(values, order);

                if (direction == ComparerDirection.Descending)
                {
                    Array.Reverse(values);
                    Array.Reverse(order);
                }
            }
            else
            {
                var keys = new KeyValuePair<int, T>[values.Length];
                for (var i = 0; i < values.Length; i++)
                    keys[i] = new KeyValuePair<int, T>(i, values[i]);

                if (direction == ComparerDirection.Ascending)
                    Array.Sort(keys, values, new StableComparer<T>((a, b) => a.CompareTo(b)));
                else
                    Array.Sort(keys, values, new StableComparer<T>((a, b) => -a.CompareTo(b)));

                order = new int[values.Length];
                for (int i = 0; i < keys.Length; i++)
                    order[i] = keys[i].Key;
            }
        }

        public static T[] Sort<T>(this T[] source)
        {
            return Sort(source, SortOrder.Asending);
        }

        public static T[] Sort<T>(this T[] source, SortOrder order)
        {
            if (order == SortOrder.Asending)
                return source.OrderBy(x => x).ToArray();
            else
                return source.OrderByDescending(x => x).ToArray();
        }

        public static T[,] ArrayOf<T>(this T[,,] originalArray, int Index)
        {
            if (Index >= originalArray.GetLength(0))
            {
                throw new IndexOutOfRangeException();
            }

            var newArray = new T[originalArray.GetLength(1), originalArray.GetLength(2)];

            for (int i = 0; i < originalArray.GetLength(1); i++)
            {
                for (int j = 0; j < originalArray.GetLength(2); j++)
                {
                    newArray[i, j] = originalArray[Index, i, j];
                }
            }

            return newArray;
        }


        /// <summary>
        /// Check that both arrays have the same Length.
        /// </summary>
        /// <param name="a">Array</param>
        /// <param name="b">Array</param>
        /// <param name="abort">Whether to throw an exception if the check fails.</param>
        /// <returns>true if the arrays have the same Length.</returns>
        /// <exception cref="DimensionMismatchException">if the Lengths differ.</exception>
        public static Boolean CheckEqualLength(double[] a, double[] b, Boolean abort)
        {
            if (a.Length == b.Length)
            {
                return true;
            }
            else
            {
                if (abort)
                {
                    throw new DimensionMismatchException(a.Length, b.Length);
                }
                return false;
            }
        }

        public static bool VerifyValues<T>(this T[] values, int begin, int Length)
        {
            return VerifyValues(values, begin, Length, false);
        }

        public static bool VerifyValues<T>(this T[] values, int begin, int Length, bool allowEmpty)
        {
            if (values == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().INPUT_ARRAY);
            }

            if (begin < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().START_POSITION, begin);
            }

            if (Length < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().LENGTH, Length);
            }

            if (begin + Length > values.Length)
            {
                throw new NumberIsTooLargeException(LocalizedResources.Instance().SUBARRAY_ENDS_AFTER_ARRAY_END,
                        begin + Length, values.Length, true);
            }

            if (Length == 0 && !allowEmpty)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Check that both arrays have the same Length.
        /// </summary>
        /// <param name="a">Array</param>
        /// <param name="b">Array</param>
        /// <exception cref="DimensionMismatchException">if the Lengths differ.</exception>
        public static void CheckEqualLength(double[] a, double[] b)
        {
            CheckEqualLength(a, b, true);
        }


        /// <summary>
        /// Check that both arrays have the same Length.
        /// </summary>
        /// <param name="a">Array</param>
        /// <param name="b">Array</param>
        /// <param name="abort">Whether to throw an exception if the check fails.</param>
        /// <returns>true if the arrays have the same Length.</returns>
        /// <exception cref="DimensionMismatchException">if the Lengths differ.</exception>
        public static Boolean CheckEqualLength(int[] a, int[] b, Boolean abort)
        {
            if (a.Length == b.Length)
            {
                return true;
            }
            else
            {
                if (abort)
                {
                    throw new DimensionMismatchException(a.Length, b.Length);
                }
                return false;
            }
        }

        /// <summary>
        /// Check that both arrays have the same Length.
        /// </summary>
        /// <param name="a">Array</param>
        /// <param name="b">Array</param>
        /// <exception cref="DimensionMismatchException">if the Lengths differ.</exception>
        public static void CheckEqualLength(int[] a, int[] b)
        {
            CheckEqualLength(a, b, true);
        }

        //public static T[] CloneExact<T>(this T[] originalArray)
        //{
        //    return (T[])originalArray.Clone();
        //}

        /// <summary>
        /// Specification of ordering direction.
        /// <summary>
        public enum OrderDirection
        {
            /// <summary>Constant for increasing direction. */
            INCREASING,
            /// <summary>Constant for decreasing direction. */
            DECREASING
        }

        /// <summary>
        /// Check that the given array is sorted.
        /// 
        /// <summary>
        /// <param name="val">Values.</param>
        /// <param name="dir">Ordering direction.</param>
        /// <param name="strict">Whether the order should be strict.</param>
        /// <param name="abort">Whether to throw an exception if the check fails.</param>
        /// <returns>{@code true} if the array is sorted.</returns>
        /// <exception cref="NonMonotonicSequenceException">if the array is not sorted </exception>
        /// and {@code abort} is {@code true}.
        public static Boolean CheckOrder(this double[] val, OrderDirection dir, Boolean strict, Boolean abort)
        {
            double previous = val[0];
            int max = val.Length;

            int index;
            //Boolean Ordered = false;

            for (index = 1; index < max; index++)
            {
                switch (dir)
                {
                    case OrderDirection.INCREASING:
                        if (strict)
                        {
                            if (val[index] <= previous)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (val[index] < previous)
                            {
                                break;
                            }
                        }
                        break;
                    case OrderDirection.DECREASING:
                        if (strict)
                        {
                            if (val[index] >= previous)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (val[index] > previous)
                            {
                                break;
                            }
                        }
                        break;
                    default:
                        // Should never happen.
                        throw new MathArithmeticException(LocalizedResources.Instance().ARRAY_INVALID_ORDER_DIRECTION);
                }

                previous = val[index];
            }

            if (index == max)
            {
                // Loop completed.
                return true;
            }

            // Loop early exit means wrong ordering.
            if (abort)
            {
                throw new MathArgumentException(LocalizedResources.Instance().ARRAY_THE_ARRAY_IS_NON_MONOTONIC_SEQUENCE, new Object[] { val[index], previous, index, dir, strict });
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check that the given array is sorted.
        /// 
        /// <summary>
        /// <param name="val">Values.</param>
        /// <param name="dir">Ordering direction.</param>
        /// <param name="strict">Whether the order should be strict.</param>
        /// <exception cref="NonMonotonicSequenceException">if the array is not sorted. </exception>
        /// @since 2.2
        public static void CheckOrder(this double[] val, OrderDirection dir, Boolean strict)
        {
            CheckOrder(val, dir, strict, true);
        }

        /// <summary>
        /// Check that the given array is sorted in strictly increasing order.
        /// 
        /// <summary>
        /// <param name="val">Values.</param>
        /// <exception cref="NonMonotonicSequenceException">if the array is not sorted. </exception>
        /// @since 2.2
        public static void CheckOrder(this double[] val)
        {
            CheckOrder(val, OrderDirection.INCREASING, true);
        }

        public static T[] Copy<T>(this T[] originalArray)
        {
            var newArray = new T[originalArray.Length];
            for (int i = 0; i < originalArray.Length; i++)
                newArray[i] = originalArray[i];

            return newArray;
        }

        /// <summary>
        ///   Creates a memberwise copy of a matrix. Matrix elements
        ///   themselves are copied only in a shallow manner (i.e. not cloned).
        /// </summary>
        /// 
        public static T[,] Copy<T>(this T[,] a)
        {
            return (T[,])a.Clone();
        }

        public static T[] CopyOf<T>(this T[] originalArray, int length)
        {
            var newArray = new T[length];
            if (length > originalArray.Length)
                length = originalArray.Length;

            Array.Copy(originalArray, newArray, length);
            return newArray;
        }

        public static T[] CopyOf<T>(this T[] originalArray, int index, int length)
        {
            var newArray = new T[length];
            if (length + index > originalArray.Length)
                length = originalArray.Length - index;

            Array.Copy(originalArray, index, newArray, 0, length);
            return newArray;
        }

        public static T[] CopyOfRange<T>(this T[] originalArray, int from, int to)
        {
            int newLength = to - from;
            if (newLength < 0)
                throw new IndexOutOfRangeException(from + " > " + to);
            T[] copy = new T[newLength];
            Array.Copy(originalArray, from, copy, 0, Math.Min(originalArray.Length - from, newLength));
            return copy;
        }

        public static void CopyRow<T>(this T[,] sourceArray, int srcRow, int srcPos, ref T[,] dest, int destRow, int destPos, int length)
        {
            if (sourceArray.GetLength(1) < srcPos)
                throw new IndexOutOfRangeException();

            if (sourceArray.GetLength(1) < (srcPos + length))
                throw new IndexOutOfRangeException();

            if (dest.GetLength(1) < destPos)
                throw new IndexOutOfRangeException();

            if (dest.GetLength(1) < (destPos + length))
                throw new IndexOutOfRangeException();

            for (int i = 0; i < length; i++)
            {
                dest[destRow, i + destPos] = sourceArray[srcRow, i + srcPos];
            }
        }

        public static T2[] ConvertTypeArray<T1, T2>(this T1[] source)
        {
            Type t2 = typeof(T2).IsNullable() ? Nullable.GetUnderlyingType(typeof(T2)) : typeof(T2);

            return source.Select(x => (T2)Convert.ChangeType(x, t2)).ToArray();
        }

        public static Complex[] ConvertToComplex(this double[] real)
        {
            Complex[] c = new Complex[real.Length];
            for (int i = 0; i < real.Length; i++)
            {
                c[i] = new Complex(real[i], 0);
            }

            return c;
        }

        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }

        public static void Fill<T>(this T[] originalArray, int idx, int to, T with)
        {
            if ((idx > originalArray.Length) || (to > originalArray.Length))
                throw new IndexOutOfRangeException();

            for (int i = idx; i < to; i++)
            {
                originalArray[i] = with;
            }
        }

        public static void Fill<T>(this T[,] originalArray, int Index, T with)
        {
            for (int i = 0; i < originalArray.GetLength(1); i++)
            {
                originalArray[Index, i] = with;
            }
        }

        public static void Fill<T>(this T[,] originalArray, T with)
        {
            for (int i = 0; i < originalArray.GetLength(0); i++)
            {
                for (int j = 0; j < originalArray.GetLength(1); j++)
                {
                    originalArray[i, j] = with;
                }
            }
        }

        public static void LoadRow<T>(this T[,] originalArray, int Index, T[] data)
        {
            LoadRow(originalArray, Index, 0, data, 0);
        }

        public static void LoadRow<T>(this T[,] originalArray, int Index, int offset, T[] data, int start)
        {
            LoadRow(originalArray, Index, offset, data, start, data.Length - start);
        }

        public static void LoadRow<T>(this T[,] originalArray, int Index, int offset, T[] data, int start, int length)
        {

            if (offset + length > originalArray.GetLength(1))
                throw new IndexOutOfRangeException();

            if (start + length > data.Length)
                throw new IndexOutOfRangeException();

            for (int i = 0; i < length; i++)
                originalArray[Index, i + offset] = data[i + start];
        }

        public static T[] GetRow<T>(this T[,] originalArray, int Index)
        {
            T[] data = new T[originalArray.GetLength(1)];

            for (int i = 0; i < originalArray.GetLength(1); i++)
                data[i] = originalArray[Index, i];

            return data;
        }

        public static void SetRow<T>(this T[,] originalArray, int Index, T[] value)
        {
            if (originalArray.GetLength(1) == value.Length)
            {
                for (int i = 0; i < originalArray.GetLength(1); i++)
                {
                    originalArray[Index, i] = value[i];
                }
            }
            else
            {
                throw new IndexOutOfRangeException(LocalizedResources.Instance().Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow);
            }
        }

        public static T[,] GetRow<T>(this T[,,] originalArray, int Index)
        {
            int rows = originalArray.GetLength(1);
            int cols = originalArray.GetLength(2);
            T[,] data = new T[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = originalArray[Index, i, j];
                }
            }

            return data;
        }

        public static void SetRow<T>(this T[,,] originalArray, int Index, T[,] value)
        {
            int rows = originalArray.GetLength(2);
            int cols = originalArray.GetLength(3);

            if (originalArray.GetLength(2) == value.GetLength(1) && originalArray.GetLength(3) == value.GetLength(2))
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        originalArray[Index, i, j] = value[i, j];
                    }
                }
            }
            else
            {
                throw new IndexOutOfRangeException(LocalizedResources.Instance().Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow);
            }
        }


        public static void SetRow<T>(this T[,,] originalArray, int Index1, int Index2, T[] value)
        {
            if (originalArray.GetLength(3) == value.Length)
            {
                for (int i = 0; i < originalArray.GetLength(3); i++)
                {
                    originalArray[Index1, Index2, i] = value[i];
                }
            }
            else
            {
                throw new IndexOutOfRangeException(LocalizedResources.Instance().Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow);
            }
        }


        /// <summary>
        ///   Gets the number of rows in a multidimensional matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements in the matrix.</typeparam>
        /// <param name="matrix">The matrix whose number of rows must be computed.</param>
        /// 
        /// <returns>The number of rows in the matrix.</returns>
        /// 
        public static int Rows<T>(this T[,] matrix)
        {
            return matrix.GetLength(0);
        }

        /// <summary>
        ///   Gets the number of columns in a multidimensional matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements in the matrix.</typeparam>
        /// <param name="matrix">The matrix whose number of columns must be computed.</param>
        /// 
        /// <returns>The number of columns in the matrix.</returns>
        /// 
        public static int Columns<T>(this T[,] matrix)
        {
            return matrix.GetLength(1);
        }

        /// <summary>
        ///   Gets the number of rows in a jagged matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements in the matrix.</typeparam>
        /// <param name="matrix">The matrix whose number of rows must be computed.</param>
        /// 
        /// <returns>The number of rows in the matrix.</returns>
        /// 
        public static int Rows<T>(this T[][] matrix)
        {
            return matrix.Length;
        }

        /// <summary>
        ///   Gets the number of columns in a jagged matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements in the matrix.</typeparam>
        /// <param name="matrix">The matrix whose number of columns must be computed.</param>
        /// 
        /// <returns>The number of columns in the matrix.</returns>
        /// 
        public static int Columns<T>(this T[][] matrix)
        {
            if (matrix.Length == 0)
                return 0;
            return matrix[0].Length;
        }

        /// <summary>
        ///   Converts a matrix to upper triangular form, if possible.
        /// </summary>
        /// 
        public static T[,] ToUpperTriangular<T>(this T[,] matrix, MatrixType from, T[,] result = null)
        {
            if (result == null)
                result = CreateAs(matrix);
            matrix.CopyTo(result);

            switch (from)
            {
                case MatrixType.UpperTriangular:
                case MatrixType.Diagonal:
                    break;

                case MatrixType.LowerTriangular:
                    Transpose(result, inPlace: true);
                    break;

                default:
                    throw new ArgumentException(LocalizedResources.Instance().Utility_Extension_Array_ToUpperTriangular_OnlyLowerTriangularUpperTriangularAndDiagonalMatricesAreSupportedAtThisTime, "matrixType");
            }

            return result;
        }

        public static IEnumerator<T> ToEnumerator<T>(this T[] baseArray)
        {
            return baseArray.AsEnumerable().GetEnumerator();
        }

        public static byte[] ToByteArray(this int[] intArray)
        {
            byte[] result = new byte[intArray.Length * sizeof(int)];
            Buffer.BlockCopy(intArray, 0, result, 0, result.Length);
            return result;
        }

        public static String[] ToStringArray<T>(this T[] baseArray)
        {
            return baseArray.OfType<object>().Select(o => o.ToString()).ToArray();
        }

        /// <summary>
        ///   Copies the content of an array to another array.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements to be copied.</typeparam>
        /// 
        /// <param name="matrix">The source matrix to be copied.</param>
        /// <param name="destination">The matrix where the elements should be copied to.</param>
        /// <param name="transpose">Whether to transpose the matrix when copying or not. Default is false.</param>
        /// 
        public static void CopyTo<T>(this T[,] matrix, T[,] destination, bool transpose = false)
        {
            if (matrix == destination)
            {
                if (transpose)
                    matrix.Transpose(true);
            }
            else
            {
                if (transpose)
                {
                    int rows = System.Math.Min(matrix.Rows(), destination.Columns());
                    int cols = System.Math.Min(matrix.Columns(), destination.Rows());
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            destination[j, i] = matrix[i, j];
                }
                else
                {
                    if (matrix.Length == destination.Length)
                    {
                        Array.Copy(matrix, 0, destination, 0, matrix.Length);
                    }
                    else
                    {
                        int rows = System.Math.Min(matrix.Rows(), destination.Rows());
                        int cols = System.Math.Min(matrix.Columns(), destination.Columns());
                        for (int i = 0; i < rows; i++)
                            for (int j = 0; j < cols; j++)
                                destination[i, j] = matrix[i, j];
                    }
                }
            }
        }

        /// <summary>
        ///   Copies the content of an array to another array.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements to be copied.</typeparam>
        /// 
        /// <param name="matrix">The source matrix to be copied.</param>
        /// <param name="destination">The matrix where the elements should be copied to.</param>
        /// <param name="transpose">Whether to transpose the matrix when copying or not. Default is false.</param>
        /// 
        public static void CopyTo<T>(this T[,] matrix, T[][] destination, bool transpose = false)
        {
            if (transpose)
            {
                int rows = System.Math.Min(matrix.Rows(), destination.Columns());
                int cols = System.Math.Min(matrix.Columns(), destination.Rows());
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        destination[j][i] = matrix[i, j];
            }
            else
            {
                if (matrix.Length == destination.Length)
                {
                    Array.Copy(matrix, 0, destination, 0, matrix.Length);
                }
                else
                {
                    int rows = System.Math.Min(matrix.Rows(), destination.Rows());
                    int cols = System.Math.Min(matrix.Columns(), destination.Columns());
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            destination[i][j] = matrix[i, j];
                }
            }
        }


        /// <summary>
        ///   Copies the content of an array to another array.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements to be copied.</typeparam>
        /// 
        /// <param name="matrix">The source matrix to be copied.</param>
        /// <param name="destination">The matrix where the elements should be copied to.</param>
        /// 
        public static void CopyTo<T>(this T[,] matrix, T[][] destination)
        {
            for (int i = 0; i < destination.Length; i++)
                for (int j = 0; j < destination[i].Length; j++)
                    destination[i][j] = matrix[i, j];
        }

        /// <summary>
        ///   Gets the transpose of a row vector.
        /// </summary>
        /// 
        /// <param name="rowVector">A row vector.</param>
        /// 
        /// <returns>The transpose of the given vector.</returns>
        /// 
        public static T[,] Transpose<T>(this T[] rowVector)
        {
            var result = new T[rowVector.Length, 1];
            for (int i = 0; i < rowVector.Length; i++)
                result[i, 0] = rowVector[i];
            return result;
        }

        /// <summary>
        ///   Gets the transpose of a row vector.
        /// </summary>
        /// 
        /// <param name="rowVector">A row vector.</param>
        /// <param name="result">The matrix where to store the transpose.</param>
        /// 
        /// <returns>The transpose of the given vector.</returns>
        /// 
        public static T[,] Transpose<T>(this T[] rowVector, T[,] result)
        {
            for (int i = 0; i < rowVector.Length; i++)
                result[i, 0] = rowVector[i];
            return result;
        }
        /// <summary>
        ///   Gets the diagonal vector from a matrix.
        /// </summary>
        /// 
        /// <param name="matrix">A matrix.</param>
        /// 
        /// <returns>The diagonal vector from the given matrix.</returns>
        /// 
        public static T[] Diagonal<T>(this T[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");

            var r = new T[matrix.Rows()];
            for (int i = 0; i < r.Length; i++)
                r[i] = matrix[i, i];

            return r;
        }


        /// <summary>
        ///   Gets the lower triangular part of a matrix.
        /// </summary>
        /// 
        public static T[,] GetLowerTriangle<T>(this T[,] matrix, bool includeDiagonal = true)
        {
            int s = includeDiagonal ? 1 : 0;
            var r = CreateAs(matrix);
            for (int i = 0; i < matrix.Rows(); i++)
                for (int j = 0; j < i + s; j++)
                    r[i, j] = matrix[i, j]; ;
            return r;
        }

        /// <summary>
        ///   Gets the upper triangular part of a matrix.
        /// </summary>
        /// 
        public static T[,] GetUpperTriangle<T>(this T[,] matrix, bool includeDiagonal = false)
        {
            int s = includeDiagonal ? 0 : 1;
            var r = CreateAs(matrix);
            for (int i = 0; i < matrix.Rows(); i++)
                for (int j = i + s; j < matrix.Columns(); j++)
                    r[i, j] = matrix[i, j]; ;
            return r;
        }

        /// <summary>
        ///   Transforms a triangular matrix in a symmetric matrix by copying
        ///   its elements to the other, unfilled part of the matrix.
        /// </summary>
        /// 
        public static T[,] GetSymmetric<T>(this T[,] matrix, MatrixType type, T[,] result = null)
        {
            if (result == null)
                result = CreateAs(matrix);

            switch (type)
            {
                case MatrixType.LowerTriangular:
                    for (int i = 0; i < matrix.Rows(); i++)
                        for (int j = 0; j <= i; j++)
                            result[i, j] = result[j, i] = matrix[i, j];
                    break;
                case MatrixType.UpperTriangular:
                    for (int i = 0; i < matrix.Rows(); i++)
                        for (int j = i; j <= matrix.Columns(); j++)
                            result[i, j] = result[j, i] = matrix[i, j];
                    break;
                default:
                    throw new System.Exception(LocalizedResources.Instance().Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace);
            }

            return result;
        }

        /// <summary>
        ///   Computes the product <c>R = A*B</c> of two matrices <c>A</c>
        ///   and <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product.</param>
        /// 
        public static int[][] Dot(this int[][] a, int[][] b, int[][] result)
        {
            int N = result.Length;
            int K = a.Columns();
            int M = result.Columns();

            var t = new int[K];

            for (int j = 0; j < M; j++)
            {
                for (int k = 0; k < b.Length; k++)
                    t[k] = b[k][j];

                for (int i = 0; i < a.Length; i++)
                {
                    int s = (int)0;
                    for (int k = 0; k < t.Length; k++)
                        s += (int)((int)a[i][k] * (int)t[k]);
                    result[i][j] = (int)s;
                }
            }
            return result;
        }

        /// <summary>
        ///   Multiplies a matrix <c>A</c> and a column vector <c>v</c>,
        ///   giving the product <c>A*v</c>
        /// </summary>
        /// 
        /// <param name="matrix">The matrix <c>A</c>.</param>
        /// <param name="columnVector">The column vector <c>v</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product.</param>
        /// 
        public static byte[] Dot(this byte[][] matrix, byte[] columnVector, byte[] result)
        {

            for (int i = 0; i < matrix.Length; i++)
            {
                byte s = (byte)0;
                for (int j = 0; j < columnVector.Length; j++)
                    s += (byte)((byte)matrix[i][j] * (byte)columnVector[j]);
                result[i] = (byte)s;
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A*B</c> of two matrices <c>A</c> and <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A*B</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[,] Dot(this double[,] a, double[,] b)
        {
            return Dot(a, b, new double[a.Rows(), b.Columns()]);
        }

        /// <summary>
        ///   Computes the product <c>R = A*B</c> of two matrices <c>A</c>
        ///   and <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product.</param>
        /// 
        public static int[,] Dot(this int[,] a, int[,] b, int[,] result)
        {
            int N = result.Length;
            int K = a.Columns();
            int M = result.Columns();

            var t = new int[K];

            for (int j = 0; j < M; j++)
            {
                for (int k = 0; k < b.Length; k++)
                    t[k] = b[k, j];

                for (int i = 0; i < a.Length; i++)
                {
                    int s = (int)0;
                    for (int k = 0; k < t.Length; k++)
                        s += (int)((int)a[i, k] * (int)t[k]);
                    result[i, j] = (int)s;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>R = A*B</c> of two matrices <c>A</c>
        ///   and <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product.</param>
        /// 
        public static Double[,] Dot(this Double[,] a, Double[,] b, Double[,] result)
        {
            int N = result.Length;
            int K = a.Columns();
            int M = result.Columns();

            var t = new Double[K];

            for (int j = 0; j < M; j++)
            {
                for (int k = 0; k < b.Length; k++)
                    t[k] = b[k, j];

                for (int i = 0; i < a.Length; i++)
                {
                    Double s = (Double)0;
                    for (int k = 0; k < t.Length; k++)
                        s += (int)((Double)a[i, k] * (Double)t[k]);
                    result[i, j] = (Double)s;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A*B'</c> of matrix <c>A</c> and transpose of <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The transposed right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A*B'</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[,] DotWithTransposed(this double[,] a, double[,] b)
        {
            return DotWithTransposed(a, b, new double[a.Rows(), b.Rows()]);
        }

        /// <summary>
        ///   Computes the product <c>A*B'</c> of matrix <c>A</c> and transpose of <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The transposed right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A*B'</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[][] DotWithTransposed(this double[][] a, double[][] b)
        {
            return DotWithTransposed(a, b, MatrixUtility.Create<double>(a.Length, b.Length));
        }

        /// <summary>
        ///   Computes the product <c>A*B'</c> of matrix <c>A</c> and
        ///   transpose of <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The transposed right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product <c>R = A*B'</c>
        ///   of the given matrices <c>A</c> and <c>B</c>.</param>
        public static double[,] DotWithTransposed(this double[,] a, double[,] b, double[,] result)
        {
            for (int i = 0; i < a.Length; i++)
            {
                double[] arow = a.GetRow(i);
                for (int j = 0; j < b.Length; j++)
                {
                    double sum = 0;
                    double[] brow = b.GetRow(j);
                    for (int k = 0; k < arow.Length; k++)
                        sum += (double)((double)arow[k] * (double)brow[k]);
                    result[i, j] = (double)sum;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A*B'</c> of matrix <c>A</c> and
        ///   transpose of <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The transposed right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product <c>R = A*B'</c>
        ///   of the given matrices <c>A</c> and <c>B</c>.</param>
        ///    
        public static double[][] DotWithTransposed(this double[][] a, double[][] b, double[][] result)
        {
            for (int i = 0; i < a.Length; i++)
            {
                double[] arow = a[i];
                for (int j = 0; j < b.Length; j++)
                {
                    double sum = 0;
                    double[] brow = b[j];
                    for (int k = 0; k < arow.Length; k++)
                        sum += (double)((double)arow[k] * (double)brow[k]);
                    result[i][j] = (double)sum;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A'*B</c> of transposed of matrix <c>A</c> and <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The transposed left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A'*B</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[,] TransposeAndDot(this double[,] a, double[,] b)
        {
            var result = new Double[a.Columns(), b.Columns()];
            result.Fill<Double>(0d);
            return TransposeAndDot(a, b, result);
        }

        /// <summary>
        ///   Computes the product <c>A'*B</c> of matrix <c>A</c> transposed and matrix <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The transposed left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product <c>R = A'*B</c>
        ///   of the given matrices <c>A</c> and <c>B</c>.</param>
        /// 
        public static double[,] TransposeAndDot(this double[,] a, double[,] b, double[,] result)
        {
            int n = a.Length;
            int m = a.Columns();
            int p = b.Columns();

            var Bcolj = new double[n];
            for (int i = 0; i < p; i++)
            {
                for (int k = 0; k < b.Length; k++)
                    Bcolj[k] = b[k, i];

                for (int j = 0; j < m; j++)
                {
                    double s = (double)0;
                    for (int k = 0; k < Bcolj.Length; k++)
                        s += (double)((double)a[k, j] * (double)Bcolj[k]);
                    result[j, i] = (double)s;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A'*B</c> of transposed of matrix <c>A</c> and <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The transposed left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A'*B</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[][] TransposeAndDot(this double[][] a, double[][] b)
        {
            return TransposeAndDot(a, b, MatrixUtility.Create<double>(a.Columns(), b.Columns()));
        }


        /// <summary>
        ///   Computes the product <c>A'*B</c> of matrix <c>A</c> transposed and matrix <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The transposed left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product <c>R = A'*B</c>
        ///   of the given matrices <c>A</c> and <c>B</c>.</param>
        /// 
        public static double[][] TransposeAndDot(this double[][] a, double[][] b, double[][] result)
        {
            int n = a.Length;
            int m = a.Columns();
            int p = b.Columns();

            var Bcolj = new double[n];
            for (int i = 0; i < p; i++)
            {
                for (int k = 0; k < b.Length; k++)
                    Bcolj[k] = b[k][i];

                for (int j = 0; j < m; j++)
                {
                    double s = (double)0;
                    for (int k = 0; k < Bcolj.Length; k++)
                        s += (double)((double)a[k][j] * (double)Bcolj[k]);
                    result[j][i] = (double)s;
                }
            }
            return result;
        }

        public static T[] GetColumnValue<T>(this T[,] source, int col)
        {
            var ret = new T[source.Rows()];

            for (int i = 0; i < source.Rows(); i++)
            {
                ret[i] = source[i, col];
            }

            return ret;
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        /// 
		/// 
		/// <example>
		///   <code source="Unit Tests\Accord.Tests.Math\Matrix\MatrixTest.cs" region="doc_inverse" />
		/// </example>
        /// 
        public static Double[][] Inverse(this Double[][] matrix)
        {
            return Inverse(matrix, false);
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        /// 
		/// <example>
		///   <code source="Unit Tests\Accord.Tests.Math\Matrix\MatrixTest.cs" region="doc_inverse" />
		/// </example>
        /// 
        public static Double[][] Inverse(this Double[][] matrix, bool inPlace)
        {
            int rows = matrix.Length;
            int cols = matrix[0].Length;

            if (rows != cols)
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_MUST_BE_SQUARE, "matrix");

            if (rows == 3)
            {
                // Special case for 3x3 matrices
                Double a = matrix[0][0], b = matrix[0][1], c = matrix[0][2];
                Double d = matrix[1][0], e = matrix[1][1], f = matrix[1][2];
                Double g = matrix[2][0], h = matrix[2][1], i = matrix[2][2];

                Double den = a * (e * i - f * h) -
                             b * (d * i - f * g) +
                             c * (d * h - e * g);

                if (den == 0)
                    throw new SingularMatrixException();

                Double m = 1 / den;

                var inv = matrix;
                if (!inPlace)
                {
                    inv = new Double[3][];
                    for (int j = 0; j < inv.Length; j++)
                        inv[j] = new Double[3];
                }

                inv[0][0] = m * (e * i - f * h);
                inv[0][1] = m * (c * h - b * i);
                inv[0][2] = m * (b * f - c * e);
                inv[1][0] = m * (f * g - d * i);
                inv[1][1] = m * (a * i - c * g);
                inv[1][2] = m * (c * d - a * f);
                inv[2][0] = m * (d * h - e * g);
                inv[2][1] = m * (b * g - a * h);
                inv[2][2] = m * (a * e - b * d);

                return inv;
            }

            if (rows == 2)
            {
                // Special case for 2x2 matrices
                Double a = matrix[0][0], b = matrix[0][1];
                Double c = matrix[1][0], d = matrix[1][1];

                Double den = a * d - b * c;

                if (den == 0)
                    throw new SingularMatrixException();

                Double m = 1 / den;

                var inv = matrix;
                if (!inPlace)
                {
                    inv = new Double[2][];
                    for (int j = 0; j < inv.Length; j++)
                        inv[j] = new Double[2];
                }

                inv[0][0] = +m * d;
                inv[0][1] = -m * b;
                inv[1][0] = -m * c;
                inv[1][1] = +m * a;

                return inv;
            }

            #region Calculate Jagged Lu Decomposition Inverse
            //return new JaggedLuDecomposition(matrix, false, inPlace).Inverse();

            var lu = matrix.MemberwiseClone();
            var pivotSign = 1;

            var pivotVector = new int[rows];
            for (int i = 0; i < rows; i++)
                pivotVector[i] = i;

            Double[] LUcolj = new Double[rows];


            // Outer loop.
            for (int j = 0; j < cols; j++)
            {
                // Make a copy of the j-th column to localize references.
                for (int i = 0; i < rows; i++)
                    LUcolj[i] = lu[i][j];

                // Apply previous transformations.
                for (int i = 0; i < rows; i++)
                {
                    Double s = 0;

                    // Most of the time is spent in
                    // the following dot product:
                    int kmax = System.Math.Min(i, j);
                    for (int k = 0; k < kmax; k++)
                        s += lu[i][k] * LUcolj[k];

                    lu[i][j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                int p = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (System.Math.Abs(LUcolj[i]) > System.Math.Abs(LUcolj[p]))
                        p = i;
                }

                if (p != j)
                {
                    for (int k = 0; k < cols; k++)
                    {
                        Double t = lu[p][k];
                        lu[p][k] = lu[j][k];
                        lu[j][k] = t;
                    }

                    int v = pivotVector[p];
                    pivotVector[p] = pivotVector[j];
                    pivotVector[j] = v;

                    pivotSign = -pivotSign;
                }

                // Compute multipliers.
                if (j < rows && lu[j][j] != 0)
                {
                    for (int i = j + 1; i < rows; i++)
                        lu[i][j] /= lu[j][j];
                }
            }

            // Copy right hand side with pivoting
            var X = new Double[rows][];
            for (int i = 0; i < rows; i++)
            {
                X[i] = new Double[rows];
                int k = pivotVector[i];
                X[i][k] = 1;
            }

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < rows; k++)
                for (int i = k + 1; i < rows; i++)
                    for (int j = 0; j < rows; j++)
                        X[i][j] -= X[k][j] * lu[i][k];

            // Solve U*X = I;
            for (int k = rows - 1; k >= 0; k--)
            {
                for (int j = 0; j < rows; j++)
                    X[k][j] /= lu[k][k];

                for (int i = 0; i < k; i++)
                    for (int j = 0; j < rows; j++)
                        X[i][j] -= X[k][j] * lu[i][k];
            }

            #endregion

            return X;
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        /// 
        /// 
        /// <example>
        ///   <code source="Unit Tests\Accord.Tests.Math\Matrix\MatrixTest.cs" region="doc_inverse" />
        /// </example>
        /// 
        public static Double[,] Inverse(this Double[,] matrix)
        {
            return Inverse(matrix, false);
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        /// 
		/// <example>
		///   <code source="Unit Tests\Accord.Tests.Math\Matrix\MatrixTest.cs" region="doc_inverse" />
		/// </example>
        /// 
        public static Double[,] Inverse(this Double[,] matrix, bool inPlace)
        {
            int rows = matrix.Rows();
            int cols = matrix.GetMaxColumnLength();

            if (rows != cols)
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_MUST_BE_SQUARE, "matrix");

            if (rows == 3)
            {
                // Special case for 3x3 matrices
                Double a = matrix[0, 0], b = matrix[0, 1], c = matrix[0, 2];
                Double d = matrix[1, 0], e = matrix[1, 1], f = matrix[1, 2];
                Double g = matrix[2, 0], h = matrix[2, 1], i = matrix[2, 2];

                Double den = a * (e * i - f * h) -
                             b * (d * i - f * g) +
                             c * (d * h - e * g);

                if (den == 0)
                    throw new SingularMatrixException();

                Double m = 1 / den;

                var inv = matrix;
                if (!inPlace)
                {
                    inv = new Double[3, 3];
                    for (int j = 0; j < inv.Length; j++)
                        inv.Fill(j, 0d);
                }

                inv[0, 0] = m * (e * i - f * h);
                inv[0, 1] = m * (c * h - b * i);
                inv[0, 2] = m * (b * f - c * e);
                inv[1, 0] = m * (f * g - d * i);
                inv[1, 1] = m * (a * i - c * g);
                inv[1, 2] = m * (c * d - a * f);
                inv[2, 0] = m * (d * h - e * g);
                inv[2, 1] = m * (b * g - a * h);
                inv[2, 2] = m * (a * e - b * d);

                return inv;
            }

            if (rows == 2)
            {
                // Special case for 2x2 matrices
                Double a = matrix[0, 0], b = matrix[0, 1];
                Double c = matrix[1, 0], d = matrix[1, 1];

                Double den = a * d - b * c;

                if (den == 0)
                    throw new SingularMatrixException();

                Double m = 1 / den;

                var inv = matrix;
                if (!inPlace)
                {
                    inv = new Double[2, 2];
                    for (int j = 0; j < inv.Length; j++)
                        inv.Fill(j, 2d);
                }

                inv[0, 0] = +m * d;
                inv[0, 1] = -m * b;
                inv[1, 0] = -m * c;
                inv[1, 1] = +m * a;

                return inv;
            }

            #region Calculate Jagged Lu Decomposition Inverse
            //return new JaggedLuDecomposition(matrix, false, inPlace).Inverse();

            var lu = matrix.MemberwiseClone();
            var pivotSign = 1;

            var pivotVector = new int[rows];
            for (int i = 0; i < rows; i++)
                pivotVector[i] = i;

            Double[] LUcolj = new Double[rows];


            // Outer loop.
            for (int j = 0; j < cols; j++)
            {
                // Make a copy of the j-th column to localize references.
                for (int i = 0; i < rows; i++)
                    LUcolj[i] = lu[i, j];

                // Apply previous transformations.
                for (int i = 0; i < rows; i++)
                {
                    Double s = 0;

                    // Most of the time is spent in
                    // the following dot product:
                    int kmax = System.Math.Min(i, j);
                    for (int k = 0; k < kmax; k++)
                        s += lu[i, k] * LUcolj[k];

                    lu[i, j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                int p = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (System.Math.Abs(LUcolj[i]) > System.Math.Abs(LUcolj[p]))
                        p = i;
                }

                if (p != j)
                {
                    for (int k = 0; k < cols; k++)
                    {
                        Double t = lu[p, k];
                        lu[p, k] = lu[j, k];
                        lu[j, k] = t;
                    }

                    int v = pivotVector[p];
                    pivotVector[p] = pivotVector[j];
                    pivotVector[j] = v;

                    pivotSign = -pivotSign;
                }

                // Compute multipliers.
                if (j < rows && lu[j, j] != 0)
                {
                    for (int i = j + 1; i < rows; i++)
                        lu[i, j] /= lu[j, j];
                }
            }

            // Copy right hand side with pivoting
            var X = new Double[rows, rows];
            for (int i = 0; i < rows; i++)
            {
                X.Fill(i, 0d);
                int k = pivotVector[i];
                X[i, k] = 1;
            }

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < rows; k++)
                for (int i = k + 1; i < rows; i++)
                    for (int j = 0; j < rows; j++)
                        X[i, j] -= X[k, j] * lu[i, k];

            // Solve U*X = I;
            for (int k = rows - 1; k >= 0; k--)
            {
                for (int j = 0; j < rows; j++)
                    X[k, j] /= lu[k, k];

                for (int i = 0; i < k; i++)
                    for (int j = 0; j < rows; j++)
                        X[i, j] -= X[k, j] * lu[i, k];
            }

            #endregion

            return X;
        }


        /// <summary>
        ///   Creates a memberwise copy of a multidimensional matrix. Matrix elements
        ///   themselves are copied only in a shallowed manner (i.e. not cloned).
        /// </summary>
        /// 
        public static T[,] MemberwiseClone<T>(this T[,] a)
        {
            // TODO: Rename to Copy and implement shallow and deep copies
            return (T[,])a.Clone();
        }

        /// <summary>
        ///   Creates a memberwise copy of a vector matrix. Vector elements
        ///   themselves are copied only in a shallow manner (i.e. not cloned).
        /// </summary>
        /// 
        public static T[] MemberwiseClone<T>(this T[] a)
        {
            // TODO: Rename to Copy and implement shallow and deep copies
            return (T[])a.Clone();
        }

        public static int[] Natural(this int[] a, int n)
        {
            return Sequence(a, n, 0, 1);
        }

        public static int[] Sequence(this int[] a, int size, int start, int stride)
        {
            a = new int[size];
            for (int i = 0; i < size; i++)
            {
                a[i] = start + i * stride;
            }
            return a;
        }

        /// <summary>
        ///   Transforms a vector into a matrix of given dimensions.
        /// </summary>
        /// 
        public static T[,] Reshape<T>(this T[] array, int rows, int cols, MatrixOrder order = MatrixOrder.Default)
        {
            return Reshape(array, rows, cols, new T[rows, cols], order);
        }

        /// <summary>
        ///   Transforms a vector into a matrix of given dimensions.
        /// </summary>
        /// 
        public static T[,] Reshape<T>(this T[] array, int rows, int cols, T[,] result, MatrixOrder order = MatrixOrder.Default)
        {
            if (order == MatrixOrder.CRowMajor)
            {
                int k = 0;
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        result[i, j] = array[k++];
            }
            else
            {
                int k = 0;
                for (int j = 0; j < cols; j++)
                    for (int i = 0; i < rows; i++)
                        result[i, j] = array[k++];
            }

            return result;
        }

        /// <summary>
        ///   Combines a vector and a element horizontally.
        /// </summary>
        /// 
        public static T[] Concatenate<T>(this T element, T[] vector)
        {
            T[] r = new T[vector.Length + 1];

            r[0] = element;

            for (int i = 0; i < vector.Length; i++)
                r[i + 1] = vector[i];

            return r;
        }

        /// <summary>
        ///   Determines whether an array is a jagged array 
        ///   (containing inner arrays as its elements).
        /// </summary>
        /// 
        public static bool IsJagged(this Array array)
        {
            if (array.Length == 0)
                return array.Rank == 1;
            return array.GetType().GetElementType().IsArray;
        }

        /// <summary>
        ///   Gets the length of each dimension of an array.
        /// </summary>
        /// 
        /// <param name="array">The array.</param>
        /// <param name="deep">Pass true to retrieve all dimensions of the array,
        ///   even if it contains nested arrays (as in jagged matrices)</param>
        /// <param name="max">Gets the maximum length possible for each dimension (in case
        ///   the jagged matrices has different lengths).</param>
        /// 
        public static int[] GetLength(this Array array, bool deep = true, bool max = false)
        {
            if (array == null)
                return new int[] { -1 };
            if (array.Rank == 0)
                return new int[0];

            if (deep && IsJagged(array))
            {
                if (array.Length == 0)
                    return new int[0];

                int[] rest;
                if (!max)
                {
                    rest = GetLength(array.GetValue(0) as Array, deep);
                }
                else
                {
                    // find the max
                    rest = GetLength(array.GetValue(0) as Array, deep);
                    for (int i = 1; i < array.Length; i++)
                    {
                        int[] r = GetLength(array.GetValue(i) as Array, deep);

                        for (int j = 0; j < r.Length; j++)
                        {
                            if (r[j] > rest[j])
                                rest[j] = r[j];
                        }
                    }
                }

                return array.Length.Concatenate(rest);
            }

            int[] vector = new int[array.Rank];
            for (int i = 0; i < vector.Length; i++)
                vector[i] = array.GetUpperBound(i) + 1;
            return vector;
        }

        /// <summary>
        ///   Creates a vector containing every index that can be used to
        ///   address a given <paramref name="array"/>, in order.
        /// </summary>
        /// 
        /// <param name="array">The array whose indices will be returned.</param>
        /// <param name="deep">Pass true to retrieve all dimensions of the array,
        ///   even if it contains nested arrays (as in jagged matrices).</param>
        /// <param name="max">Bases computations on the maximum length possible for 
        ///   each dimension (in case the jagged matrices has different lengths).</param>
        /// <param name="order">The direction to access the matrix. Pass 1 to read the 
        ///   matrix in row-major order. Pass 0 to read in column-major order. Default is 
        ///   1 (row-major, c-style order).</param>
        /// 
        /// <returns>
        ///   An enumerable object that can be used to iterate over all
        ///   positions of the given <paramref name="array">System.Array</paramref>.
        /// </returns>
        /// 
        /// <example>
        /// <code>
        ///   double[,] a = 
        ///   { 
        ///      { 5.3, 2.3 },
        ///      { 4.2, 9.2 }
        ///   };
        ///   
        ///   foreach (int[] idx in a.GetIndices())
        ///   {
        ///      // Get the current element
        ///      double e = (double)a.GetValue(idx);
        ///   }
        /// </code>
        /// </example>
        /// 
        /// <seealso cref="Accord.Math.Vector.GetIndices{T}(T[])"/>
        /// 
        public static IEnumerable<int[]> GetIndices(this Array array, bool deep = false, bool max = false, MatrixOrder order = MatrixOrder.Default)
        {
            return Combinatorics.Sequences(array.GetLength(deep, max), firstColumnChangesFaster: order == MatrixOrder.FortranColumnMajor);
        }

        public static T GetSafe<T>(this T[] source, int index)
        {
            try
            {
                return source[index];
            }
            catch
            {
                return default(T);
            }
        }

        public static T GetSafe<T>(this T[,] source, int indexA, int indexB)
        {
            try
            {
                return source[indexA, indexB];
            }
            catch
            {
                return default(T);
            }
        }

        public static T GetSafe<T>(this T[,,] source, int indexA, int indexB, int indexC)
        {
            try
            {
                return source[indexA, indexB, indexC];
            }
            catch
            {
                return default(T);
            }
        }


        public static T GetSafe<T>(this T[][] source, int indexA, int indexB)
        {
            try
            {
                return source[indexA][indexB];
            }
            catch
            {
                return default(T);
            }
        }

        public static T GetSafe<T>(this T[][][] source, int indexA, int indexB, int indexC)
        {
            try
            {
                return source[indexA][indexB][indexC];
            }
            catch
            {
                return default(T);
            }
        }

        public static Boolean IsSquare(this double[,] A, int n, int m)
        {
            return (n == m);
        }

        public static Boolean IsSquare(this double[,] A)
        {
            return (A.GetLength(0) == A.GetLength(1));
        }

        public static Boolean IsSquare(this double[][] A)
        {
            return (A.Length == A[0].Length);
        }

        public static Boolean IsSquare(this double[][] A, int n, int m)
        {
            return (n == m);
        }

        /// <summary>
        /// Determines if int array is sorted from 0 -> Max
        /// </summary>
        public static bool IsSorted(this int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1] > arr[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if int array is sorted from 0 -> Max
        /// </summary>
        public static bool IsSorted(this Double[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1] > arr[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if string array is sorted from A -> Z
        /// </summary>
        public static bool IsSorted(this string[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1].CompareTo(arr[i]) > 0) // If previous is bigger, return false
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if int array is sorted from Max -> 0
        /// </summary>
        public static bool IsSortedDescending(this int[] arr)
        {
            for (int i = arr.Length - 2; i >= 0; i--)
            {
                if (arr[i] < arr[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if int array is sorted from Max -> 0
        /// </summary>
        public static bool IsSortedDescending(this double[] arr)
        {
            for (int i = arr.Length - 2; i >= 0; i--)
            {
                if (arr[i] < arr[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if string array is sorted from Z -> A
        /// </summary>
        public static bool IsSortedDescending(this string[] arr)
        {
            for (int i = arr.Length - 2; i >= 0; i--)
            {
                if (arr[i].CompareTo(arr[i + 1]) < 0) // If previous is smaller, return false
                {
                    return false;
                }
            }
            return true;
        }

        public static Boolean IsDistinct<T>(this T[] A)
        {
            return A.Distinct().Count() == A.Count();
        }

        public static Boolean IsDistinct<T>(this T[,] A)
        {
            var query = from T item in A
                        select item;

            return query.Distinct().Count() == query.Count();
        }

        public static Boolean IsDistinct<T>(this T[][] A)
        {
            var query = from T item in A
                        select item;

            return query.Distinct().Count() == query.Count();
        }

        public static Boolean IsDiagonal(this double[,] A)
        {
            return IsDiagonal(A, A.GetLength(0), A.GetLength(1));
        }

        public static Boolean IsDiagonal(this double[,] A, int n, int m)
        {
            Boolean flag = IsSquare(A, n, m);
            if (flag)
            {
                for (int i = 0; flag && i < n - 1; i++)
                {
                    for (int j = i + 1; flag && j < n; j++)
                    {
                        flag = (A[i, j].Equals(0) && A[j, i].Equals(0));
                    }
                }
            }
            return flag;
        }

        public static Boolean IsDiagonal(this double[][] A)
        {
            return IsDiagonal(A, A.Length, A[0].Length);
        }

        public static Boolean IsDiagonal(this double[][] A, int n, int m)
        {
            Boolean flag = IsSquare(A, n, m);
            if (flag)
            {
                for (int i = 0; flag && i < n - 1; i++)
                {
                    for (int j = i + 1; flag && j < n; j++)
                    {
                        flag = (A[i][j].Equals(0) && A[j][i].Equals(0));
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// Compute a linear combination accurately.
        /// This method computes the sum of the products
        /// <code>a<sub>i</sub> b<sub>i</sub></code> to high accuracy.
        /// It does so by using specific multiplication and addition algorithms to
        /// preserve accuracy and reduce cancellation effects.
        /// <br/>
        /// It is based on the 2005 paper
        /// <a href="http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.2.1547">
        /// Accurate Sum and Dot Product</a> by Takeshi Ogita, Siegfried Md Rump,
        /// and Shin'ichi Oishi published in SIAM Jd Scid Comput.
        /// 
        /// <summary>
        /// <param name="a">Factors.</param>
        /// <param name="b">Factors.</param>
        /// <returns><code>&Sigma;<sub>i</sub> a<sub>i</sub> b<sub>i</sub></code>.</returns>
        /// <exception cref="DimensionMismatchException">if arrays dimensions don't match </exception>
        public static double LinearCombination(this double[] a, double[] b)
        {
            CheckEqualLength(a, b);
            int len = a.Length;

            if (len == 1)
            {
                // Revert to scalar multiplication.
                return a[0] * b[0];
            }

            double[] prodHigh = new double[len];
            double prodLowSum = 0;

            for (int i = 0; i < len; i++)
            {
                double ai = a[i];
                double aHigh = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(ai) & ((-1L) << 27));
                double aLow = ai - aHigh;

                double bi = b[i];
                double bHigh = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(bi) & ((-1L) << 27));
                double bLow = bi - bHigh;
                prodHigh[i] = ai * bi;
                double prodLow = aLow * bLow - (((prodHigh[i] -
                                                        aHigh * bHigh) -
                                                       aLow * bHigh) -
                                                      aHigh * bLow);
                prodLowSum += prodLow;
            }


            double prodHighCur = prodHigh[0];
            double prodHighNext = prodHigh[1];
            double sHighPrev = prodHighCur + prodHighNext;
            double sPrime = sHighPrev - prodHighNext;
            double sLowSum = (prodHighNext - (sHighPrev - sPrime)) + (prodHighCur - sPrime);

            int lenMinusOne = len - 1;
            for (int i = 1; i < lenMinusOne; i++)
            {
                prodHighNext = prodHigh[i + 1];
                double sHighCur = sHighPrev + prodHighNext;
                sPrime = sHighCur - prodHighNext;
                sLowSum += (prodHighNext - (sHighCur - sPrime)) + (sHighPrev - sPrime);
                sHighPrev = sHighCur;
            }

            double result = sHighPrev + (prodLowSum + sLowSum);

            if (Double.IsNaN(result))
            {
                // either we have split infinite numbers or some coefficients were NaNs,
                // just rely on the naive implementation and let IEEE754 handle this
                result = 0;
                for (int i = 0; i < len; ++i)
                {
                    result += a[i] * b[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Compute a linear combination accurately.
        /// <p>
        /// This method computes a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> to high accuracyd It does
        /// so by using specific multiplication and addition algorithms to
        /// preserve accuracy and reduce cancellation effectsd It is based
        /// on the 2005 paper <a
        /// href="http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.2.1547">
        /// Accurate Sum and Dot Product</a> by Takeshi Ogita,
        /// Siegfried Md Rump, and Shin'ichi Oishi published in SIAM Jd Scid Comput.
        /// </p>
        /// <summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <returns>a<sub>1</sub>&times;b<sub>1</sub> +</returns>
        /// a<sub>2</sub>&times;b<sub>2</sub>
        /// <see cref="#linearCombination(double,">double, double, double, double, double) </see>
        /// <see cref="#linearCombination(double,">double, double, double, double, double, double, double) </see>
        public static double LinearCombination(this double a1, double b1,
                                               double a2, double b2)
        {

            // the code below is split in many additions/subtractions that may
            // appear redundantd However, they should NOT be simplified, as they
            // use IEEE754 floating point arithmetic rounding properties.
            // The variable naming conventions are that xyzHigh contains the most significant
            // bits of xyz and xyzLow contains its least significant bitsd So theoretically
            // xyz is the sum xyzHigh + xyzLow, but in many cases below, this sum cannot
            // be represented in only one double precision number so we preserve two numbers
            // to hold it as long as we can, combining the high and low order bits together
            // only at the end, after cancellation may have occurred on high order bits

            // split a1 and b1 as one 26 bits number and one 27 bits number

            double a1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a1) & ((-1L) << 27));
            double a1Low = a1 - a1High;
            double b1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b1) & ((-1L) << 27));
            double b1Low = b1 - b1High;

            // accurate multiplication a1 * b1
            double prod1High = a1 * b1;
            double prod1Low = a1Low * b1Low - (((prod1High - a1High * b1High) - a1Low * b1High) - a1High * b1Low);

            // split a2 and b2 as one 26 bits number and one 27 bits number
            double a2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a2) & ((-1L) << 27));
            double a2Low = a2 - a2High;
            double b2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b2) & ((-1L) << 27));
            double b2Low = b2 - b2High;

            // accurate multiplication a2 * b2
            double prod2High = a2 * b2;
            double prod2Low = a2Low * b2Low - (((prod2High - a2High * b2High) - a2Low * b2High) - a2High * b2Low);

            // accurate addition a1 * b1 + a2 * b2
            double s12High = prod1High + prod2High;
            double s12Prime = s12High - prod2High;
            double s12Low = (prod2High - (s12High - s12Prime)) + (prod1High - s12Prime);

            // rounding, s12 may have suffered many cancellations, we try
            // to recover some bits from the extra words we have saved up to now
            double result = s12High + (prod1Low + prod2Low + s12Low);

            if (Double.IsNaN(result))
            {
                // either we have split infinite numbers or some coefficients were NaNs,
                // just rely on the naive implementation and let IEEE754 handle this
                result = a1 * b1 + a2 * b2;
            }

            return result;
        }

        /// <summary>
        /// Compute a linear combination accurately.
        /// <p>
        /// This method computes a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub>
        /// to high accuracyd It does so by using specific multiplication and
        /// addition algorithms to preserve accuracy and reduce cancellation effects.
        /// It is based on the 2005 paper <a
        /// href="http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.2.1547">
        /// Accurate Sum and Dot Product</a> by Takeshi Ogita,
        /// Siegfried Md Rump, and Shin'ichi Oishi published in SIAM Jd Scid Comput.
        /// </p>
        /// <summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <param name="a3">first factor of the third term</param>
        /// <param name="b3">second factor of the third term</param>
        /// <returns>a<sub>1</sub>&times;b<sub>1</sub> +</returns>
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub>
        /// <see cref="#linearCombination(double,">double, double, double) </see>
        /// <see cref="#linearCombination(double,">double, double, double, double, double, double, double) </see>
        public static double LinearCombination(this double a1, double b1,
                                               double a2, double b2,
                                               double a3, double b3)
        {

            // the code below is split in many additions/subtractions that may
            // appear redundantd However, they should NOT be simplified, as they
            // do use IEEE754 floating point arithmetic rounding properties.
            // The variables naming conventions are that xyzHigh contains the most significant
            // bits of xyz and xyzLow contains its least significant bitsd So theoretically
            // xyz is the sum xyzHigh + xyzLow, but in many cases below, this sum cannot
            // be represented in only one double precision number so we preserve two numbers
            // to hold it as long as we can, combining the high and low order bits together
            // only at the end, after cancellation may have occurred on high order bits

            // split a1 and b1 as one 26 bits number and one 27 bits number
            double a1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a1) & ((-1L) << 27));
            double a1Low = a1 - a1High;
            double b1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b1) & ((-1L) << 27));
            double b1Low = b1 - b1High;

            // accurate multiplication a1 * b1
            double prod1High = a1 * b1;
            double prod1Low = a1Low * b1Low - (((prod1High - a1High * b1High) - a1Low * b1High) - a1High * b1Low);

            // split a2 and b2 as one 26 bits number and one 27 bits number
            double a2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a2) & ((-1L) << 27));
            double a2Low = a2 - a2High;
            double b2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b2) & ((-1L) << 27));
            double b2Low = b2 - b2High;

            // accurate multiplication a2 * b2
            double prod2High = a2 * b2;
            double prod2Low = a2Low * b2Low - (((prod2High - a2High * b2High) - a2Low * b2High) - a2High * b2Low);

            // split a3 and b3 as one 26 bits number and one 27 bits number
            double a3High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a3) & ((-1L) << 27));
            double a3Low = a3 - a3High;
            double b3High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b3) & ((-1L) << 27));
            double b3Low = b3 - b3High;

            // accurate multiplication a3 * b3
            double prod3High = a3 * b3;
            double prod3Low = a3Low * b3Low - (((prod3High - a3High * b3High) - a3Low * b3High) - a3High * b3Low);

            // accurate addition a1 * b1 + a2 * b2
            double s12High = prod1High + prod2High;
            double s12Prime = s12High - prod2High;
            double s12Low = (prod2High - (s12High - s12Prime)) + (prod1High - s12Prime);

            // accurate addition a1 * b1 + a2 * b2 + a3 * b3
            double s123High = s12High + prod3High;
            double s123Prime = s123High - prod3High;
            double s123Low = (prod3High - (s123High - s123Prime)) + (s12High - s123Prime);

            // rounding, s123 may have suffered many cancellations, we try
            // to recover some bits from the extra words we have saved up to now
            double result = s123High + (prod1Low + prod2Low + prod3Low + s12Low + s123Low);

            if (Double.IsNaN(result))
            {
                // either we have split infinite numbers or some coefficients were NaNs,
                // just rely on the naive implementation and let IEEE754 handle this
                result = a1 * b1 + a2 * b2 + a3 * b3;
            }

            return result;
        }

        /// <summary>
        /// Compute a linear combination accurately.
        /// <p>
        /// This method computes a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub> +
        /// a<sub>4</sub>&times;b<sub>4</sub>
        /// to high accuracyd It does so by using specific multiplication and
        /// addition algorithms to preserve accuracy and reduce cancellation effects.
        /// It is based on the 2005 paper <a
        /// href="http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.2.1547">
        /// Accurate Sum and Dot Product</a> by Takeshi Ogita,
        /// Siegfried Md Rump, and Shin'ichi Oishi published in SIAM Jd Scid Comput.
        /// </p>
        /// <summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <param name="a3">first factor of the third term</param>
        /// <param name="b3">second factor of the third term</param>
        /// <param name="a4">first factor of the third term</param>
        /// <param name="b4">second factor of the third term</param>
        /// <returns>a<sub>1</sub>&times;b<sub>1</sub> +</returns>
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub> +
        /// a<sub>4</sub>&times;b<sub>4</sub>
        /// <see cref="#linearCombination(double,">double, double, double) </see>
        /// <see cref="#linearCombination(double,">double, double, double, double, double) </see>
        public static double LinearCombination(this double a1, double b1,
                                               double a2, double b2,
                                               double a3, double b3,
                                               double a4, double b4)
        {

            // the code below is split in many additions/subtractions that may
            // appear redundantd However, they should NOT be simplified, as they
            // do use IEEE754 floating point arithmetic rounding properties.
            // The variables naming conventions are that xyzHigh contains the most significant
            // bits of xyz and xyzLow contains its least significant bitsd So theoretically
            // xyz is the sum xyzHigh + xyzLow, but in many cases below, this sum cannot
            // be represented in only one double precision number so we preserve two numbers
            // to hold it as long as we can, combining the high and low order bits together
            // only at the end, after cancellation may have occurred on high order bits

            // split a1 and b1 as one 26 bits number and one 27 bits number
            double a1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a1) & ((-1L) << 27));
            double a1Low = a1 - a1High;
            double b1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b1) & ((-1L) << 27));
            double b1Low = b1 - b1High;

            // accurate multiplication a1 * b1
            double prod1High = a1 * b1;
            double prod1Low = a1Low * b1Low - (((prod1High - a1High * b1High) - a1Low * b1High) - a1High * b1Low);

            // split a2 and b2 as one 26 bits number and one 27 bits number
            double a2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a2) & ((-1L) << 27));
            double a2Low = a2 - a2High;
            double b2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b2) & ((-1L) << 27));
            double b2Low = b2 - b2High;

            // accurate multiplication a2 * b2
            double prod2High = a2 * b2;
            double prod2Low = a2Low * b2Low - (((prod2High - a2High * b2High) - a2Low * b2High) - a2High * b2Low);

            // split a3 and b3 as one 26 bits number and one 27 bits number
            double a3High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a3) & ((-1L) << 27));
            double a3Low = a3 - a3High;
            double b3High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b3) & ((-1L) << 27));
            double b3Low = b3 - b3High;

            // accurate multiplication a3 * b3
            double prod3High = a3 * b3;
            double prod3Low = a3Low * b3Low - (((prod3High - a3High * b3High) - a3Low * b3High) - a3High * b3Low);

            // split a4 and b4 as one 26 bits number and one 27 bits number
            double a4High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a4) & ((-1L) << 27));
            double a4Low = a4 - a4High;
            double b4High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b4) & ((-1L) << 27));
            double b4Low = b4 - b4High;

            // accurate multiplication a4 * b4
            double prod4High = a4 * b4;
            double prod4Low = a4Low * b4Low - (((prod4High - a4High * b4High) - a4Low * b4High) - a4High * b4Low);

            // accurate addition a1 * b1 + a2 * b2
            double s12High = prod1High + prod2High;
            double s12Prime = s12High - prod2High;
            double s12Low = (prod2High - (s12High - s12Prime)) + (prod1High - s12Prime);

            // accurate addition a1 * b1 + a2 * b2 + a3 * b3
            double s123High = s12High + prod3High;
            double s123Prime = s123High - prod3High;
            double s123Low = (prod3High - (s123High - s123Prime)) + (s12High - s123Prime);

            // accurate addition a1 * b1 + a2 * b2 + a3 * b3 + a4 * b4
            double s1234High = s123High + prod4High;
            double s1234Prime = s1234High - prod4High;
            double s1234Low = (prod4High - (s1234High - s1234Prime)) + (s123High - s1234Prime);

            // rounding, s1234 may have suffered many cancellations, we try
            // to recover some bits from the extra words we have saved up to now
            double result = s1234High + (prod1Low + prod2Low + prod3Low + prod4Low + s12Low + s123Low + s1234Low);

            if (Double.IsNaN(result))
            {
                // either we have split infinite numbers or some coefficients were NaNs,
                // just rely on the naive implementation and let IEEE754 handle this
                result = a1 * b1 + a2 * b2 + a3 * b3 + a4 * b4;
            }

            return result;
        }


        #region Transpose

        /// <summary>
        ///   Gets the transpose of a matrix.
        /// </summary>
        /// 
        /// <param name="matrix">A matrix.</param>
        /// 
        /// <returns>The transpose of the given matrix.</returns>
        /// 
        public static T[,] Transpose<T>(this T[,] matrix)
        {
            return Transpose(matrix, false);
        }

        /// <summary>
        ///   Gets the transpose of a matrix.
        /// </summary>
        /// 
        /// <param name="matrix">A matrix.</param>
        /// 
        /// <param name="inPlace">True to store the transpose over the same input
        ///   <paramref name="matrix"/>, false otherwise. Default is false.</param>
        ///   
        /// <returns>The transpose of the given matrix.</returns>
        /// 
        public static T[,] Transpose<T>(this T[,] matrix, bool inPlace)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (inPlace)
            {
                if (rows != cols)
                    throw new ArgumentException(LocalizedResources.Instance().Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace, "matrix");

                for (int i = 0; i < rows; i++)
                {
                    for (int j = i; j < cols; j++)
                    {
                        T element = matrix[j, i];
                        matrix[j, i] = matrix[i, j];
                        matrix[i, j] = element;
                    }
                }

                return matrix;
            }
            else
            {
                T[,] result = new T[cols, rows];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        result[j, i] = matrix[i, j];

                return result;
            }
        }


        /// <summary>
        ///   Gets the generalized transpose of a tensor.
        /// </summary>
        /// 
        /// <param name="array">A tensor.</param>
        /// <param name="order">The new order for the tensor's dimensions.</param>
        /// 
        /// <returns>The transpose of the given tensor.</returns>
        /// 
        public static Array Transpose(this Array array, int[] order)
        {
            if (order.Length != array.Rank)
                throw new ArgumentException(LocalizedResources.Instance().ARRAY_ORDER);

            if (array.Length == 1 || array.Length == 0)
                return array;

            // Get the number of samples at each dimension
            int[] size = new int[array.Rank];
            for (int i = 0; i < size.Length; i++)
                size[i] = array.GetLength(i);

            Array r = Array.CreateInstance(array.GetType().GetElementType(), size.Get(order));

            // Generate all indices for accessing the matrix 
            foreach (int[] pos in Combinatorics.Sequences(size, inPlace: true))
            {
                int[] newPos = pos.Get(order);
                object value = array.GetValue(pos);
                r.SetValue(value, newPos);
            }

            return r;
        }

        /// <summary>
        ///   Gets the generalized transpose of a tensor.
        /// </summary>
        /// 
        /// <param name="array">A tensor.</param>
        /// <param name="order">The new order for the tensor's dimensions.</param>
        /// 
        /// <returns>The transpose of the given tensor.</returns>
        /// 
        public static T Transpose<T>(this T array, int[] order)
            where T : class, IList
        {
            Array arr = array as Array;

            if (arr == null)
                throw new ArgumentException(LocalizedResources.Instance().Utility_Extension_Array_Transpose_TheGivenObjectMustInheritFromSystemArray, "array");

            return Transpose(arr, order) as T;
        }
        #endregion

    }
}
