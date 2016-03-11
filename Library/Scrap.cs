namespace CepLibrary
{
    using HtmlAgilityPack;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;

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
            return Parse(cep, Request(cep));
        }

        private static string Request(string cep)
        {
            var bytes = Encoding.ASCII.GetBytes(
                string.Format(CultureInfo.InvariantCulture, "relaxation={0}&TipoCep=ALL&semelhante=N&cfm=1&Metodo=listaLogradouro&TipoConsulta=relaxation&StartRow=1&EndRow=10", cep)
            );

            var webRequest = WebRequest.Create("http://www.buscacep.correios.com.br/servicos/dnec/consultaEnderecoAction.do");
            webRequest.ContentLength = bytes.Length;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

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
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var message = doc.DocumentNode.SelectSingleNode("//div[@class='ctrlcontent']//font[@color='black']") ??
                doc.DocumentNode.SelectSingleNode("//div[@class='ctrlcontent']//div[@class='informativo2']");
            if (message != null)
            {
                if (message.InnerHtml == string.Format(CultureInfo.InvariantCulture, "O endereço informado {0} não foi encontrado.", cep))
                    return null;
                else
                    throw new ScrapException(string.Format(CultureInfo.InvariantCulture, "Found \"{0}\" message.", message.InnerHtml));
            }

            var fields = doc.DocumentNode.SelectNodes("//div[@class='ctrlcontent']//div/table[1]/tr/td");
            if (fields == null)
                throw new ScrapException("Couldn't find any field.");
            else if (fields.Count != 5)
                throw new ScrapException(string.Format(CultureInfo.InvariantCulture, "Unexpected number of fields: {0}.", fields.Count));
            else
                return new Endereco
                {
                    Cep = Sanitize(fields[4].InnerHtml),
                    Logradouro = fields[0].InnerHtml,
                    Bairro = fields[1].InnerHtml,
                    Localidade = fields[2].InnerHtml,
                    Uf = fields[3].InnerHtml
                };
        }
    }
}
