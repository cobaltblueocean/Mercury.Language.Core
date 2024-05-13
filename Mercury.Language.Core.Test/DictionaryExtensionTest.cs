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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Core.Test
{
    /// <summary>
    /// DictionaryExtensionTest Description
    /// </summary>
    public class DictionaryExtensionTest
    {
        [Test]
        public void TestKeyAtIndex()
        {
            Dictionary<double?, double?> _temp = new Dictionary<double?, double?>();

            _temp.Add(3, 6);
            _temp.Add(4, 8);
            _temp.Add(5, 10);

            var key = _temp.GetKeyAtIndex(1);

            Assert.That(4 == key);

            SortedDictionary<double?, double?> _temp2 = new SortedDictionary<double?, double?>();

            _temp2.Add(7, 8);
            _temp2.Add(9, 12);
            _temp2.Add(8, 16);

            var key2 = _temp2.GetKeyAtIndex(2);

            Assert.That(9 == key2);
        }

        [Test]
        public void DictionaryOperationTest()
        {
            IDictionary<int, string> dict1 = new Dictionary<int, string>();
            IDictionary<int, string> dict2 = new Dictionary<int, string>();

            dict1.Add(0, "A");
            dict1.Add(1, "B");
            dict1.Add(2, "C");

            dict2.AddRange(dict1);

            Assert.That(dict1.Count.Equals(dict2.Count));

            foreach(var item in dict1)
            {
                Assert.That(item.Value.Equals(dict2[item.Key]));
            }

            Assert.That(dict1.AreObjectsEqual(dict2));
        }
    }
}
