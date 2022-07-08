using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Mercury.Language.Core.Test.DummyObjects;

namespace Mercury.Language.Core.Test
{
    public class ObjectExtensionTest
    {
        Random rnd = new Random();

        [Test]
        public void Test1()
        {
            var objectA = new ComplexObject();
            var objectB = new ComplexObject();
            var objectC = new ComplexObject();

            var data = new Dictionary<int, double?>();
            var values = new double?[] { 1, 0, 0.12345, 1.25 };
            for (int i = 0; i<5; i++)
            {
                data.Add(i, rnd.NextDouble());
            }

            SampleReferenceObject ref1 = new SampleReferenceObject() { Name = "Sample" };
            TestReferenceObject ref2 = new TestReferenceObject() { Name = "Test" };
            TestReferenceObject ref3 = new TestReferenceObject() { Name = "Sample" };

            objectA.Id = 1;
            objectA.Name = "Dummy";
            objectA.Value = values;
            objectA.ReferenceObject = ref1;

            objectB.Id = 1;
            objectB.Name = "Dummy";
            objectB.Value = values;
            objectB.ReferenceObject = ref1;

            objectC.Id = 1;
            objectC.Name = "Dummy";
            objectC.Value = values;
            objectC.ReferenceObject = ref2;

            foreach (var item in data)
            {
                objectA.AddChild(item.Key, item.Value);
                objectB.AddChild(item.Key, item.Value);
                objectC.AddChild(item.Key, item.Value);
            }

            Assert.IsTrue(objectA.AreObjectsEqual(objectB));
            Assert.IsFalse(objectA.AreObjectsEqual(objectC));

            objectB.ReferenceObject = ref3;
            Assert.IsFalse(objectB.AreObjectsEqual(objectC));

            objectB.ReferenceObject.Name = "Test";
            Assert.IsTrue(objectB.AreObjectsEqual(objectC));

            //Assert.Pass();
        }
    }
}