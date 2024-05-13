﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Mercury.Language.Core.Test.Collections
{
    public class ReadOnlySetTest
    {
        [Test]
        public void AddItemTest()
        {
            ClassicAssert.Throws<NotSupportedException>(() =>
            {
                var readOnlySet = new ReadOnlySet<int>(new HashSet<int> { 1, 2, 3 });
                readOnlySet.Add(4);
            });
        }
    }
}
