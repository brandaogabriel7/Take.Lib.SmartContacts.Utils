using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Take.SmartContacts.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveEmojis(this string source)
        {
            return Regex.Replace(source, @"([^\u0000-\u010F]|\n|\t|\r)+", string.Empty);
        }

        public static string RemoveAccentuation(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var normalizedChars = normalizedString.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);

            return new string(normalizedChars.ToArray()).Normalize(NormalizationForm.FormC);
        }

        public static string TrimBetween(this string text)
        {
            return Regex.Replace(text, @"\s+", " ");
        }
    }
}