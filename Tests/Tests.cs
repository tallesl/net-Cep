namespace CepLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Sanitize()
        {
            Assert.AreEqual(null, Cep.Sanitize(""));
            Assert.AreEqual(null, Cep.Sanitize("foo"));
            Assert.AreEqual(null, Cep.Sanitize("3013001"));
            Assert.AreEqual("30130010", Cep.Sanitize("30130010"));
            Assert.AreEqual("30130010", Cep.Sanitize("30130-010"));
            Assert.AreEqual("30130010", Cep.Sanitize("30.130-010"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullProxy()
        {
            Cep.Scrap("30130010", null);
        }

        [TestMethod]
        public void ScrapSomething()
        {
            var scraped = Cep.Scrap("30130010");

            Assert.AreEqual("30130010", scraped.Cep);
            Assert.AreEqual("Praça Sete de Setembro", scraped.Logradouro);
            Assert.AreEqual("Centro", scraped.Bairro);
            Assert.AreEqual("Belo Horizonte", scraped.Localidade);
            Assert.AreEqual("MG", scraped.Uf);
        }

        [TestMethod]
        public void ScrapNothing()
        {
            var scraped = Cep.Scrap("foo");
            Assert.AreEqual(null, scraped);
        }

        [TestMethod]
        [ExpectedException(typeof(ScrapException))]
        public void ScrapException()
        {
            Cep.Scrap(string.Empty);
        }
    }
}
