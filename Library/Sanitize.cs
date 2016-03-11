namespace CepLibrary
{
    using System;
    using System.Linq;

    /// <summary>
    /// Sanitizes CEPs.
    /// </summary>
    public static partial class Cep
    {
        /// <summary>
        /// Sanizites a CEP (Código de Endereçamento Postal).
        /// "30130-010" becomes "30130010".
        /// "foo" becomes null.
        /// </summary>
        /// <param name="cep">CEP to sanitize</param>
        /// <returns>The given CEP without mask or null if the given CEP is not valid</returns>
        public static string Sanitize(string cep)
        {
            if (cep == null)
                throw new ArgumentNullException("cep");

            // XXXXXXX
            var digitOnly =
                cep.Length == 8 &&
                cep.All(Char.IsDigit);

            // XXXXX-XXX
            var withDash =
                cep.Length == 9 &&
                cep[5] == '-' &&
                Enumerable.Range(0, 9).Except(new[] { 5 }).All(i => Char.IsDigit(cep[i]));

            // XX.XXX-XXX
            var withDot =
                cep.Length == 10 &&
                cep[2] == '.' &&
                cep[6] == '-' &&
               Enumerable.Range(0, 10).Except(new[] { 2, 6 }).All(i => Char.IsDigit(cep[i]));

            if (digitOnly)
                return cep;
            else if (withDash)
                return cep.Substring(0, 5) + cep.Substring(6, 3);
            else if (withDot)
                return cep.Substring(0, 2) + cep.Substring(3, 3) + cep.Substring(7, 3);
            else
                return null;
        }
    }
}
