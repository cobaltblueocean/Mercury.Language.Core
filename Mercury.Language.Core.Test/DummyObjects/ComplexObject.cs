// Copyright (c) 2017 - presented by Kei Nakai
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

namespace Mercury.Language.Core.Test.DummyObjects
{
    /// <summary>
    /// ComplexObject Description
    /// </summary>
    public class ComplexObject : IComplexObject
    {
        private List<ChildObject> _children;
        
        public int Id { get; set; }
        public string Name { get; set; }
        public double?[] Value { get; set; }

        public Object Reference { get ; set; }

        public ChildObject[] Children { get { return _children.ToArray(); } }

        public IRefObject ReferenceObject { get; set; }

        public ComplexObject()
        {
            _children = new List<ChildObject>();
        }

        public void AddChild(int key, double? value)
        {
            _children.Add(new ChildObject(key, value));
        }

        public void AddRange(double?[] values)
        {
            throw new NotSupportedException();
        }
    }
}
