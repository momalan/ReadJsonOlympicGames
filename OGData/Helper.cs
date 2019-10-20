using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGData
{
    public class Helper
    {
        public string Nationality(string nationality)
        {
            if (nationality == "CA")
            {
                return "CANADIAN";
            }
            else if (nationality == "JP")
            {
                return "JAPANESE";
            }
            else if (nationality == "FR")
            {
                return "FRENCH";
            }
            else if (nationality == "SR")
            {
                return "SERBIAN";
            }
            else if (nationality == "SI")
            {
                return "SLOVENIAN";
            }
            else if (nationality == "IT")
            {
                return "ITALIAN";
            }
            else if (nationality == "NO")
            {
                return "NORWEGIAN";
            }
            else
            {
                return nationality;
            }
        }
    }
}
