﻿namespace CepLibrary
{
    using System;

    /// <summary>
    /// Got an unexpected HTML.
    /// </summary>
    [Serializable]
    public class ScrapException : Exception
    {
        internal ScrapException(string message) : base("We got an unexpected HTML from Correios. " + message) { }
    }
}
