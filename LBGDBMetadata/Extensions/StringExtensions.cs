using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LBGDBMetadata.Extensions
{
    public static class StringExtensions
    {
        public static string Sanitize(this string text)
        {
            /*
            text = Regex.Replace(text, @"(^|\s+){1}II($|\s+){1}", "2", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}III($|\s+){1}", "3", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}IV($|\s+){1}", "4", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}V($|\s+){1}", "5", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}VI($|\s+){1}", "6", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}VII($|\s+){1}", "7", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}VIII($|\s+){1}", "8", RegexOptions.IgnoreCase);
            */

            return Regex.Replace(text, @"(((^|\s+){1}|(,\s+){1})the($|\s+){1})|(((^|\s+){1}|(,\s+){1})an($|\s+){1})|(((^|\s+){1}|(,\s+){1})a($|\s+){1})|[^A-Za-z0-9]", "", RegexOptions.IgnoreCase).ToLower();
        }

        public static string ConvertRomans(this string text)
        {
            text = Regex.Replace(text, @"(^|\s+){1}II($|\s+){1}", "2", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}III($|\s+){1}", "3", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}IV($|\s+){1}", "4", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}V($|\s+){1}", "5", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}VI($|\s+){1}", "6", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"(^|\s+){1}VII($|\s+){1}", "7", RegexOptions.IgnoreCase);
            return Regex.Replace(text, @"(^|\s+){1}VIII($|\s+){1}", "8", RegexOptions.IgnoreCase);
        }

    }
}
