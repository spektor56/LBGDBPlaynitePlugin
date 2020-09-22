using System.Text.RegularExpressions;

namespace LBGDBMetadata.Extensions
{
    public static class StringExtensions
    {

        private static readonly Regex SanitizeRegex = new Regex(@"((^the\s+)|([^A-Za-z\s]+\s*the(?![A-Za-z])))|((^an\s+)|([^A-Za-z\s]+\s*an(?![A-Za-z])))|((^a\s+)|([^A-Za-z\s]+\s*a(?![A-Za-z])))|[^A-Za-z0-9]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
            return SanitizeRegex.Replace(text, "").ToLower();
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
