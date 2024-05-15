using Mercury.Language;
using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Contains utility methods for checking inputs to methods.
    /// <p>
    /// This utility is used throughout the system to validate inputs to methods.
    /// Most of the methods return their validated input, allowing patterns like this:
    /// <pre>
    ///  // constructor
    ///  public Person(String name, int age) {
    ///    _name = ArgumentChecker.NotBlank(name, "name");
    ///    _age = ArgumentChecker.NotNegative(age, "age");
    ///  }
    /// </pre>
    /// </summary>
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
        public static void IsTrue(Boolean validIfTrue, params Object[] message)
        {
            // return void, not the parameter, as no need to check a Boolean method parameter
            if (!validIfTrue)
            {
                var stringMsg = "";
                if (message.Length == 1)
                {
                    stringMsg = (String)message[0];
                }
                else if (message.Length > 1)
                {
                    String msgBody = (String)message[0];
                    var args = message.CopyOf(1, message.Length - 1);
                    stringMsg = String.Format(msgBody, args);
                }

                throw new ArgumentException(stringMsg);
            }
        }

        /// <summary>
        /// Checks that the specified boolean is false.
        /// <p>
        /// Given the input parameter, this returns normally only if it is false.
        /// This will typically be the result of a caller-specific check.
        /// For example:
        /// <pre>
        ///  ArgumentChecker.isFalse(collection.contains("value"), "Collection must not contain 'value'");
        /// </pre>
        /// <p>
        /// This returns {@code void}, and not the value being checked, as there is
        /// never a good reason to validate a boolean parameter value.
        /// </summary>
        /// <param name="validIfFalse">a boolean resulting from testing an argument, may be null</param>
        /// <exception cref="ArgumentException">if the test value is false</exception>
        public static void IsFalse(Boolean validIfFalse)
        {
            // return void, not the parameter, as no need to check a Boolean method parameter
            if (validIfFalse)
            {
                throw new ArgumentException();
            }
        }

        public static void IsFalse(Boolean validIfFalse, String message)
        {
            // return void, not the parameter, as no need to check a Boolean method parameter
            if (validIfFalse)
            {
                throw new ArgumentException(message);
            }
        }

        public static void IsFalse(Boolean validIfFalse, params Object[] message)
        {
            // return void, not the parameter, as no need to check a Boolean method parameter
            if (validIfFalse)
            {
                var stringMsg = "";
                if (message.Length == 1)
                {
                    stringMsg = (String)message[0];
                }
                else if (message.Length > 1)
                {
                    String msgBody = (String)message[0];
                    var args = message.CopyOf(1, message.Length - 1);
                    stringMsg = String.Format(msgBody, args);
                }

                throw new ArgumentException(stringMsg);
            }
        }
        public static String NotEmpty(String parameter)
        {
            return NotEmpty(parameter, "parameter");
        }

        public static String NotEmpty(String parameter, String name)
        {
            NotNull(parameter, name);
            if (String.IsNullOrEmpty(parameter))
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_NOT_BE_EMPTY, name));
            }
            return parameter;
        }

        public static T[] NotEmpty<T>(T[] parameter)
        {
            return NotEmpty(parameter, "parameter");
        }

        public static T[] NotEmpty<T>(T[] parameter, String name)
        {
            NotNull(parameter, name);
            if (parameter.Length == 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_ARRAY_MUST_NOT_BE_EMPTY , name));
            }
            return parameter;
        }

        public static IEnumerable<T> NotEmpty<T>(IEnumerable<T> parameter)
        {
            return NotEmpty(parameter, "parameter");
        }

        public static IEnumerable<T> NotEmpty<T>(IEnumerable<T> parameter, String name)
        {
            NotNull(parameter, name);
            if (!parameter.GetEnumerator().MoveNext())
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_ITERABLE_MUST_NOT_BE_EMPTY, name));
            }
            return parameter;
        }

        public static ICollection<C> NotEmpty<C>(ICollection<C> parameter)
        {
            return NotEmpty(parameter, "parameter");
        }

        public static ICollection<C> NotEmpty<C>(ICollection<C> parameter, String name)
        {
            NotNull(parameter, name);
            if (parameter.Count == 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_COLLECTION_MUST_NOT_BE_EMPTY, name));
            }
            return parameter;
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
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_ARRAY_MUST_NOT_CONTAIN_NULL_AT_INDEX, name, i));
                }
            }
            return parameter;
        }

        public static T[,] NoNulls<T>(T[,] parameter)
        {
            return NoNulls(parameter, "parameter");
        }

        public static T[,] NoNulls<T>(T[,] parameter, String name)
        {
            NotNull(parameter, name);
            for (int i = 0; i < parameter.GetLength(0); i++)
            {
                for (int j = 0; j < parameter.GetLength(1); j++)
                {
                    if (parameter[i, j] == null)
                    {
                        throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_2D_ARRAY_MUST_NOT_CONTAIN_NULL_AT_INDEX, name, i));
                    }
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
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_ENUMERABLE_MUST_NOT_CONTAIN_NULL_AT_INDEX, name, i));
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
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_LIST_MUST_NOT_CONTAIN_NULL_AT_INDEX, name, i));
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
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_NOT_BE_NULL, name));
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
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_DICTIONARY_MUST_NOT_CONTAIN_NULL_KEY_AT_INDEX, name, i));
                }
                if (item.Value == null)
                {
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_DICTIONARY_MUST_NOT_CONTAIN_NULL_VALUE_AT_INDEX, name, i));
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
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_DICTIONARY_MUST_NOT_CONTAIN_NULL_KEY_AT_INDEX, name, i));
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
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_DICTIONARY_MUST_NOT_CONTAIN_NULL_VALUE_AT_INDEX, name, i));
                }
                i++;
            }
            return parameter;
        }
        public static dynamic[] NoNulls(dynamic[] parameter)
        {
            return NoNulls(parameter, "parameter");
        }

        public static dynamic[] NoNulls(dynamic[] parameter, String name)
        {
            NotNull(parameter, name);
            for (int i = 0; i < parameter.Length; i++)
            {
                if (parameter[i] == null)
                {
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_ARRAY_MUST_NOT_CONTAIN_NULL_AT_INDEX , name, i));
                }
            }
            return parameter;
        }

        public static List<dynamic> NoNulls(List<dynamic> parameter)
        {
            return NoNulls(parameter, "parameter");
        }

        public static List<dynamic> NoNulls(List<dynamic> parameter, String name)
        {
            NotNull(parameter, name);
            for (int i = 0; i < parameter.Count; i++)
            {
                if (parameter[i] == null)
                {
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_LIST_MUST_NOT_CONTAIN_NULL_AT_INDEX, name, i));
                }
            }
            return parameter;
        }

        public static long NotNegative(long parameter)
        {
            return NotNegative(parameter, "parameter");
        }

        public static long NotNegative(long parameter, String name)
        {
            if (parameter < 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_NOT_BE_NEGATIVE, name));
            }
            return parameter;
        }

        public static double NotNegative(double parameter)
        {
            return NotNegative(parameter, "parameter");
        }

        public static double NotNegative(double parameter, String name)
        {
            if (parameter < 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_NOT_BE_NEGATIVE, name));
            }
            return parameter;
        }

        public static int NotNegativeOrZero(int parameter)
        {
            return NotNegativeOrZero(parameter, "parameter");
        }

        public static int NotNegativeOrZero(int parameter, String name)
        {
            if (parameter <= 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_NOT_BE_NEGATIVE_OR_ZERO, name));
            }
            return parameter;
        }

        public static long NotNegativeOrZero(long parameter)
        {
            return NotNegativeOrZero(parameter, "parameter");
        }

        public static long NotNegativeOrZero(long parameter, String name)
        {
            if (parameter <= 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_NOT_BE_NEGATIVE_OR_ZERO
, name));
            }
            return parameter;
        }

        public static double NotNegativeOrZero(double parameter)
        {
            return NotNegativeOrZero(parameter, "parameter");
        }

        public static double NotNegativeOrZero(double parameter, String name)
        {
            if (parameter <= 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_NOT_BE_NEGATIVE_OR_ZERO
, name));
            }
            return parameter;
        }

        public static double NotNegativeOrZero(long parameter, double eps)
        {
            return NotNegativeOrZero(parameter, eps, "parameter");
        }

        public static double NotNegativeOrZero(double parameter, double eps, String name)
        {
            if (CompareUtility.CloseEquals(parameter, 0, eps))
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_NOT_BE_NEGATIVE_OR_ZERO
, name));
            }
            if (parameter < 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_BE_GREATER_THAN_ZERO, name));
            }
            return parameter;
        }

        public static double NotZero(long parameter, double eps)
        {
            return NotZero(parameter, eps, "parameter");
        }

        public static double NotZero(double parameter, double eps, String name)
        {
            if (CompareUtility.CloseEquals(parameter, 0d, eps))
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_NOT_BE_ZERO
, name));
            }
            return parameter;
        }

        public static Boolean IsInRangeExclusive(double low, double high, double value)
        {
            return (value > low && value < high);
        }

        public static Boolean IsInRangeInclusive(double low, double high, double value)
        {
            return (value >= low && value <= high);
        }

        public static Boolean IsInRangeExcludingLow(double low, double high, double value)
        {
            return (value > low && value <= high);
        }

        public static Boolean IsInRangeExcludingHigh(double low, double high, double value)
        {
            return (value >= low && value < high);
        }

        public static void InOrderOrEqual<T>(T obj1, T obj2, String param1, String param2) where T : IComparable<T>
        {
            NotNull(obj1, param1);
            NotNull(obj2, param2);
            if (obj1.CompareTo(obj2) > 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INPUT_PARAMETER_MUST_BE_BEFORE, param1, param2));
            }
        }
    }
}
