using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class ArgumentChecker
    {
        /// <summary>
        /// Checks that the specified boolean is true.
        /// <p>
        /// Given the input parameter, this returns normally only if it is true.
        /// This will typically be the result of a caller-specific check.
        /// For example:
        /// <pre>
        ///  ArgumentChecker.IsTrue(collection.contains("value"));
        /// </pre>
        /// </summary>
        /// <param name="validIfTrue">a boolean resulting from testing an argument, may be null</param>
        /// <exception cref="ArgumentException">if the test value is false</exception>
        public static void IsTrue(Boolean validIfTrue)
        {
            // return void, not the parameter, as no need to check a Boolean method parameter
            if (!validIfTrue)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Checks that the specified boolean is true.
        /// <p>
        /// Given the input parameter, this returns normally only if it is true.
        /// This will typically be the result of a caller-specific check.
        /// For example:
        /// <pre>
        ///  ArgumentChecker.IsTrue(collection.contains("value"));
        /// </pre>
        /// </summary>
        /// <param name="validIfTrue">a boolean resulting from testing an argument, may be null</param>
        /// <param name="message">the error message, not null</param>
        /// <exception cref="ArgumentException">if the test value is false</exception>
        public static void IsTrue(Boolean validIfTrue, String message)
        {
            // return void, not the parameter, as no need to check a Boolean method parameter
            if (!validIfTrue)
            {
                throw new ArgumentException(message);
            }
        }

        public static T NotNull<T>(T parameter)
        {
            return NotNull(parameter, "parameter");
        }

        public static T NotNull<T>(T parameter, String name)
        {
            if (parameter == null)
            {
                throw new ArgumentException();
            }
            return parameter;
        }

        public static T[] NoNulls<T>(T[] parameter)
        {
            return NoNulls(parameter, "parameter");
        }

        public static T[] NoNulls<T>(T[] parameter, String name)
        {
            NotNull(parameter, name);
            for (int i = 0; i < parameter.Length; i++)
            {
                if (parameter[i] == null)
                {
                    throw new ArgumentException();
                }
            }
            return parameter;
        }

        public static IEnumerable<T> NoNulls<T>(IEnumerable<T> parameter, String name)
        {
            int i = 0;

            NotNull(parameter, name);
            foreach (var item in parameter)
            {
                if (item == null)
                {
                    throw new ArgumentException();
                }

                i++;
            }
            return parameter;
        }
        public static List<T> NoNulls<T>(List<T> parameter)
        {
            return NoNulls(parameter, "parameter");
        }

        public static List<T> NoNulls<T>(List<T> parameter, String name)
        {
            NotNull(parameter, name);
            for (int i = 0; i < parameter.Count; i++)
            {
                if (parameter[i] == null)
                {
                    throw new ArgumentException();
                }
            }
            return parameter;
        }

        public static IDictionary<TKey, TValue> NotNull<TKey, TValue>(IDictionary<TKey, TValue> parameter)
        {
            return NotNull(parameter, "parameter");
        }


        public static IDictionary<TKey, TValue> NotNull<TKey, TValue>(IDictionary<TKey, TValue> parameter, String name)
        {
            if (parameter == null)
            {
                throw new ArgumentException();
            }
            return parameter;
        }

        public static IDictionary<TKey, TValue> NoNullEntry<TKey, TValue>(IDictionary<TKey, TValue> parameter, String name)
        {
            NotNull(parameter, name);
            int i = 0;
            foreach (var item in parameter)
            {
                if (item.Key == null)
                {
                    throw new ArgumentException();
                }
                if (item.Value == null)
                {
                    throw new ArgumentException();
                }
                i++;
            }
            return parameter;
        }

        public static IDictionary<TKey, TValue> NoNullKey<TKey, TValue>(IDictionary<TKey, TValue> parameter, String name)
        {
            NotNull(parameter, name);
            int i = 0;
            foreach (var item in parameter)
            {
                if (item.Key == null)
                {
                    throw new ArgumentException();
                }
                i++;
            }
            return parameter;
        }

        public static IDictionary<TKey, TValue> NoNullValue<TKey, TValue>(IDictionary<TKey, TValue> parameter, String name)
        {
            NotNull(parameter, name);
            int i = 0;
            foreach (var item in parameter)
            {
                if (item.Value == null)
                {
                    throw new ArgumentException();
                }
                i++;
            }
            return parameter;
        }

    }
}
