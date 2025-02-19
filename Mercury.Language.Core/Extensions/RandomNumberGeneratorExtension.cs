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
using Mercury.Language.Exceptions;
using System.Security.Cryptography;
using Mercury.Language;

namespace System
{
    /// <summary>
    /// RandomNumberGeneratorExtension Description
    /// </summary>
    public static class RandomNumberGeneratorExtension
    {
        public static double Next(this RandomNumberGenerator g)
        {
            // output random float number in the interval 0 <= x < 1
            int r = RandomNumberGenerator.GetInt32(int.MaxValue); // get 32 random bits
            if (BitConverter.IsLittleEndian)
            {
                byte[] i0 = BitConverter.GetBytes((r << 20));
                byte[] i1 = BitConverter.GetBytes(((r >> 12) | 0x3FF00000));
                byte[] bytes = { i0[0], i0[1], i0[2], i0[3], i1[0], i1[1], i1[2], i1[3] };
                double f = BitConverter.ToDouble(bytes, 0);
                return f - 1.0;
            }
            return r * (1.0 / (0xFFFFFFFF + 1.0));
        }

        public static double[] GetVector(this RandomNumberGenerator r, int dimension)
        {
            // ArgumentChecker.NotNegative(dimension, "dimension");
            double[] result = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                result[i] = r.Next();
            }
            return result;
        }

        public static List<double[]> GetVectors(this RandomNumberGenerator r, int dimension, int n)
        {
            if (dimension < 0)
            {
                throw new ArgumentException(LocalizedResources.Instance().RANDOM_DIMENSION_MUST_BE_GREATER_THAN_ZERO);
            }
            if (n < 0)
            {
                throw new ArgumentException(LocalizedResources.Instance().RANDOM_NUMBER_OF_VALUES_MUST_BE_GREATER_THAN_ZERO);
            }
            List<double[]> result = new List<double[]>(n);
            double[] x;
            for (int i = 0; i < n; i++)
            {
                x = new double[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    x[j] = r.Next();
                }
                result.Add(x);
            }
            return result;
        }
    }
}
