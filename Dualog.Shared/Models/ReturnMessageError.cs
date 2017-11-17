using System;
using System.Collections.Generic;
using System.Text;

namespace Dualog.Shared.Models
{
    public class ReturnMessageError
    {
        public ReturnMessageError(string code, string norwegian, string english)
        {
            Code = code;
            NorwegianText = norwegian;
            EnglishText = english;
        }
        public string Code { get; set; }
        public string NorwegianText { get; set; }
        public string EnglishText { get; set; }
    }
}
