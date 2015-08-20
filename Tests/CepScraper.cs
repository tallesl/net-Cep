namespace CepUtility.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class Tests
    {
        [TestMethod]
        public void ScrapSomething()
        {
            var scraped = CepScraper.Scrap("30130010");

            Assert.AreEqual("30130010", scraped.Cep);
            Assert.AreEqual("Praça Sete de Setembro", scraped.Logradouro);
            Assert.AreEqual("Centro", scraped.Bairro);
            Assert.AreEqual("Belo Horizonte", scraped.Localidade);
            Assert.AreEqual("MG", scraped.Uf);
        }

        [TestMethod]
        public void ScrapNothing()
        {
            var scraped = CepScraper.Scrap("00000000");
            Assert.AreEqual(null, scraped);
        }

        [TestMethod]
        [ExpectedException(typeof(ScrapException))]
        public void ScrapException()
        {
            CepScraper.Scrap(string.Empty);
        }
    }
}
