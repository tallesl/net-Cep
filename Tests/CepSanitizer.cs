namespace CepUtility.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public partial class Tests
    {
        [TestMethod]
        public void Sanitize()
        {
            Assert.AreEqual(null, CepSanitizer.Sanitize(""));
            Assert.AreEqual(null, CepSanitizer.Sanitize("foo"));
            Assert.AreEqual(null, CepSanitizer.Sanitize("3013001"));
            Assert.AreEqual("30130010", CepSanitizer.Sanitize("30130010"));
            Assert.AreEqual("30130010", CepSanitizer.Sanitize("30130-010"));
            Assert.AreEqual("30130010", CepSanitizer.Sanitize("30.130-010"));
        }
    }
}
