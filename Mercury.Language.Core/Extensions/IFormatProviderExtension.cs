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

namespace System.Globalization
{
    /// <summary>
    /// IFormatProviderExtension Description
    /// </summary>
    public static class IFormatProviderExtension
    {
        /// <summary>
        /// Tries to get a <see cref="CultureInfo"/> from the format provider,
        /// returning the current culture if it fails.
        /// </summary>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> that supplies culture-specific
        /// formatting information.
        /// </param>
        /// <returns>A <see cref="CultureInfo"/> instance.</returns>
        public static CultureInfo GetCultureInfo(this IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                return CultureInfo.CurrentCulture;
            }

            return (formatProvider as CultureInfo)
                ?? (formatProvider.GetFormat(typeof(CultureInfo)) as CultureInfo)
                    ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Tries to get a <see cref="NumberFormatInfo"/> from the format
        /// provider, returning the current culture if it fails.
        /// </summary>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> that supplies culture-specific
        /// formatting information.
        /// </param>
        /// <returns>A <see cref="NumberFormatInfo"/> instance.</returns>
        public static NumberFormatInfo GetNumberFormatInfo(this IFormatProvider formatProvider)
        {
            return NumberFormatInfo.GetInstance(formatProvider);
        }

        /// <summary>
        /// Tries to get a <see cref="TextInfo"/> from the format provider, returning the current culture if it fails.
        /// </summary>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> that supplies culture-specific
        /// formatting information.
        /// </param>
        /// <returns>A <see cref="TextInfo"/> instance.</returns>
        public static TextInfo GetTextInfo(this IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                return CultureInfo.CurrentCulture.TextInfo;
            }

            return (formatProvider.GetFormat(typeof(TextInfo)) as TextInfo)
                ?? GetCultureInfo(formatProvider).TextInfo;
        }
    }
}
