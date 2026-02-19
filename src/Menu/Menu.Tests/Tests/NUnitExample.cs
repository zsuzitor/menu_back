using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Tests.Tests
{
    [TestFixture]
    public class NUnitExample
    {
        private object _primeService;

        [SetUp]
        public void SetUp()
        {
            _primeService = new object();
        }

        [Test]
        public void IsPrime_InputIs1_ReturnFalse()
        {

            //Assert.IsFalse(false, "1 should not be prime");
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void IsPrime_ValuesLessThan2_ReturnFalse(int value)
        {

            //Assert.AreEqual(value,value, $"{value} should not be");
        }
    }
}
