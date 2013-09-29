using System.Text.RegularExpressions;

namespace NBoilerpipe.Util
{
    public static class UtilExtention
    {
        public static string WrapAtNoDots(this string target, int index)
        {
            if (string.IsNullOrEmpty(target)) return target;
            
            return (target.Length < index) ? target : target.Substring(0, index);
        }

        public static string RemoveDoubleSpaces(this string text)
        {
            return ReplaceDoubleCharacters(text, ' ', " ");
        }
        public static string ReplaceDoubleCharacters(this string text, char c, string replace)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var regex = new Regex(@"[" + c + @"]{2,}", RegexOptions.None);
            return regex.Replace(text, replace);
        }
    }
}
