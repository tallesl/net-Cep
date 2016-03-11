namespace CepLibrary
{
    using HtmlAgilityPack;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Scraps addresses from Correios' website.
    /// </summary>
    public static partial class Cep
    {
        /// <summary>
        /// Requests the Correios' website for the given CEP's address, scraps the HTML and returns it.
        /// </summary>
        /// <param name="cep">CEP of the address</param>
        /// <returns>The scraped address or null if no address was found for the given CEP</returns>
        /// <exception cref="ScrapException">If an unexpected HTML if found</exception>
        public static Endereco Scrap(string cep)
        {
            return Parse(cep, Request(cep, null));
        }

        /// <summary>
        /// Requests the Correios' website for the given CEP's address, scraps the HTML and returns it.
        /// </summary>
        /// <param name="cep">CEP of the address</param>
        /// <param name="proxy">Proxy to use for the request</param>
        /// <returns>The scraped address or null if no address was found for the given CEP</returns>
        /// <exception cref="ArgumentNullException">If the given proxy is null</exception>
        /// <exception cref="ScrapException">If an unexpected HTML if found</exception>
        public static Endereco Scrap(string cep, IWebProxy proxy)
        {
            if (proxy == null)
                throw new ArgumentNullException("proxy");

            return Parse(cep, Request(cep, proxy));
        }

        private static string Request(string cep, IWebProxy proxy)
        {
            var bytes = Encoding.ASCII.GetBytes(
                string.Format(CultureInfo.InvariantCulture, "relaxation={0}&TipoCep=ALL&semelhante=N", cep)
            );

            var webRequest = WebRequest.Create("http://www.buscacep.correios.com.br/sistemas/buscacep/resultadoBuscaCepEndereco.cfm");
            webRequest.ContentLength = bytes.Length;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            webRequest.Proxy = proxy ?? webRequest.Proxy;

            using (var stream = webRequest.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            var response = webRequest.GetResponse();
            var encoding = Encoding.GetEncoding("ISO-8859-1");
            using (var stream = new StreamReader(response.GetResponseStream(), encoding))
            {
                return stream.ReadToEnd();
            }
        }

        private static Endereco Parse(string cep, string html)
        {
            if (html.Contains("DADOS NAO ENCONTRADOS"))
                return null;

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var fields = doc.DocumentNode.SelectNodes("//table[@class=\"tmptabela\"]//tr[2]/td");
            if (fields == null)
                throw new ScrapException("Couldn't find any field.");

            if (fields.Count != 4)
                throw new ScrapException(string.Format(CultureInfo.InvariantCulture,
                    "Unexpected number of fields: {0}.", fields.Count));

            var localidadeUf = fields[2].InnerHtml.Trim().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (localidadeUf.Length != 2)
                throw new ScrapException(string.Format(CultureInfo.InvariantCulture,
                    "Unexpected Localidade/UF: {0}.", fields[2].InnerHtml));

            return new Endereco
            {
                Cep = Sanitize(Clean(fields[3].InnerHtml)),
                Logradouro = Clean(fields[0].InnerHtml),
                Bairro = Clean(fields[1].InnerHtml),
                Localidade = Clean(localidadeUf[0]),
                Uf = Clean(localidadeUf[1])
            };
        }

        private static string Clean(string field)
        {
            return HttpUtility.HtmlDecode(field).Trim();
        }
    }
}
