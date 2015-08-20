namespace CepUtility
{
    using System;

    /// <summary>
    /// Got an unexpected HTML.
    /// </summary>
    public class ScrapException : Exception
    {
        internal ScrapException(string message) : base("We got an unexpected HTML from Correios. " + message) { }
    }
}
