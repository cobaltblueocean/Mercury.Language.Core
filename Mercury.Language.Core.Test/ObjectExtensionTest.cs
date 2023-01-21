using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Mercury.Language.Core.Test.DummyObjects;
using Mercury.Language.Core.Test.ProtoTypeObjects;

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

            var t1 = new Tuple<double?, double?>(3.2575342465753425, 112367173.16659279);
            var t2 = new Tuple<double?, double?>(3.2575342465753425, -141799015.05124027);

            objectA.Description = "Description of ObjectA";
            objectB.Description = "Description of ObjectB";
            objectC.Description = "Description of ObjectC";

            SampleReferenceObject ref1 = new SampleReferenceObject() { Name = "Sample" };
            TestReferenceObject ref2 = new TestReferenceObject() { Name = "Test" };
            TestReferenceObject ref3 = new TestReferenceObject() { Name = "Sample" };

            objectA.Id = 1;
            objectA.Name = "Dummy";
            objectA.Value = values;
            objectA.ReferenceObject = ref1;
            objectA.Function = new SimpleFunction();

            objectB.Id = 1;
            objectB.Name = "Dummy";
            objectB.Value = values;
            objectB.ReferenceObject = ref1;
            objectB.Function = new SimpleFunction();

            objectC.Id = 1;
            objectC.Name = "Dummy";
            objectC.Value = values;
            objectC.ReferenceObject = ref2;
            objectC.Function = new MultiplyFunction();

            foreach (var item in data)
            {
                objectA.AddChild(item.Key, item.Value);
                objectB.AddChild(item.Key, item.Value);
                objectC.AddChild(item.Key, item.Value);
            }

            objectA.AddItem(t1);
            objectA.AddItem(t2);

            objectB.AddItem(t1);
            objectB.AddItem(t2);

            objectC.AddItem(t1);
            objectC.AddItem(t2);

            Assert.IsTrue(objectA.AreObjectsEqual(objectB));
            Assert.IsFalse(objectA.AreObjectsEqual(objectC));

            objectB.ReferenceObject = ref3;
            Assert.IsFalse(objectB.AreObjectsEqual(objectC));

            objectB.ReferenceObject.Name = "Test";
            objectB.Function = new MultiplyFunction();
            Assert.IsTrue(objectB.AreObjectsEqual(objectC));

            //Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            var protoType = new ProtpTypeClass<ProtoTypeImplemebtedClass>();

            Assert.IsTrue(protoType.IsGenericParameterImplementType(typeof(ProtoTypeInterface1)));
            Assert.IsTrue(protoType.GetType().IsGenericParameterImplementType(typeof(ProtoTypeInterface1)));

            Assert.IsTrue(protoType.IsGenericParameterImplementBaseClass(typeof(ProtpTypeBaseClass)));
            Assert.IsTrue(protoType.GetType().IsGenericParameterImplementBaseClass(typeof(ProtpTypeBaseClass)));
        }
    }
}