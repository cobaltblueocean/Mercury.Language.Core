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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Core.Utility
{
    /// <summary>
    /// ObjectUtility Description
    /// </summary>
    public class ObjectUtility
    {
        private static char AT_SIGN = '@';

        public static Boolean AllNotNull(params Object[] values)
        {
            if (values == null)
            {
                return false;
            }
            else
            {
                Object[] var1 = values;
                int var2 = values.Length;

                for (int var3 = 0; var3 < var2; ++var3)
                {
                    Object val = var1[var3];
                    if (val == null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static Boolean AllNull(params Object[] values)
        {
            return !AnyNotNull(values);
        }

        public static Boolean AnyNotNull(params Object[] values)
        {
            return FirstNonNull(values) != null;
        }

        public static Boolean AnyNull(params Object[] values)
        {
            return !AllNotNull(values);
        }

        public static Boolean CONST(Boolean v)
        {
            return v;
        }

        public static byte CONST(byte v)
        {
            return v;
        }

        public static char CONST(char v)
        {
            return v;
        }

        public static double CONST(double v)
        {
            return v;
        }

        public static float CONST(float v)
        {
            return v;
        }

        public static int CONST(int v)
        {
            return v;
        }

        public static long CONST(long v)
        {
            return v;
        }

        public static short CONST(short v)
        {
            return v;
        }

        public static T CONST<T>(T v)
        {
            return v;
        }

        public static byte CONST_BYTE(int v)
        {
            if (v >= -128 && v <= 127)
            {
                return (byte)v;
            }
            else
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().SUPPLIED_VALUE_MUST_BE_A_VALID_BYTE_LITERAL_BETWEEN_A_AND_B, "-128", "127", v));
            }
        }

        public static short CONST_SHORT(int v)
        {
            if (v >= -32768 && v <= 32767)
            {
                return (short)v;
            }
            else
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().SUPPLIED_VALUE_MUST_BE_A_VALID_BYTE_LITERAL_BETWEEN_A_AND_B, "-32768", "32767", v));
            }
        }

        public static T DefaultIfNull<T>(T obj, T defaultValue)
        {
            return obj != null ? obj : defaultValue;
        }
        public static T FirstNonNull<T>(params T[] values)
        {
            if (values != null)
            {
                T[] var1 = values;
                int var2 = values.Length;

                for (int var3 = 0; var3 < var2; ++var3)
                {
                    T val = var1[var3];
                    if (val != null)
                    {
                        return val;
                    }
                }
            }

            return default;
        }

        public static T GetFirstNonNull<T>(params Func<T>[] suppliers)
        {
            if (suppliers != null)
            {
                Func<T>[] var1 = suppliers;
                int var2 = suppliers.Length;

                for (int var3 = 0; var3 < var2; ++var3)
                {
                    Func<T> supplier = var1[var3];
                    if (supplier != null)
                    {
                        T value = supplier.Invoke();
                        if (value != null)
                        {
                            return value;
                        }
                    }
                }
            }

            return default;
        }

        public static T GetIfNull<T>(T obj, Func<T> defaultSupplier)
        {
            return obj != null ? obj : (defaultSupplier == null ? default : defaultSupplier.Invoke());
        }
    }
}
