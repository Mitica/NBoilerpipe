using System;
using System.Text;
using NBoilerpipe.Extractors;

namespace NBoilerpipe.Util
{
    public static class ArticleUtility
    {
        /// <summary>
        /// Detects article inner text from an uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public static string ClearArticle(Uri uri, string head)
        {
            if (string.IsNullOrEmpty(head)) return null;



            string article;

            var extractor = new WebPageExtractor();

            string html;
            try
            {
                html = extractor.GetHtml(uri.ToString()).Result;
            }
            catch
            {
                return null;
            }

            string content = null;

            try
            {
                article = ArticleExtractor.INSTANCE.GetText(html);
            }
            catch
            {
                return null;
            }

            if (!string.IsNullOrEmpty(article))
            {
                content = ClearArticle(article, head);
                if (!string.IsNullOrEmpty(content)) return content;
            }

            try
            {
                article = LargestContentExtractor.INSTANCE.GetText(html);
            }
            catch
            {
                return null;
            }

            if (!string.IsNullOrEmpty(article))
            {
                content = ClearArticle(article, head);
            }
            return content;
        }

        /// <summary>
        /// Detects inner article text, without start and end unuseful text
        /// </summary>
        /// <param name="article"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public static string ClearArticle(string article, string head)
        {
            if (string.IsNullOrEmpty(article) || string.IsNullOrEmpty(head)) return null;
            string content = article;

            if (string.IsNullOrEmpty(content) || content.Length < head.Length) return null;

            var contentLines = GetLines(content);

            var summaryLines = GetLines(head);

            var summaryStart = summaryLines[0].WrapAtNoDots(100);

            var startContentLineIndex = -1;

            for (int i = 0; i < contentLines.Length; i++)
            {
                var contentLine = contentLines[i];
                if (contentLine.StartsWith(summaryStart) || summaryStart.StartsWith(contentLine.WrapAtNoDots(90)))
                {
                    startContentLineIndex = i;
                    break;
                }
            }
            if (startContentLineIndex == -1) return null;

            var contentBuilder = new StringBuilder();

            for (int i = startContentLineIndex; i < contentLines.Length; i++)
            {
                var line = contentLines[i];
                if (i > contentLines.Length - 3)
                {
                    if (line.Length < 50 || line != line.RemoveDoubleSpaces()) break;
                }
                contentBuilder.AppendLine(line);
            }

            return contentBuilder.ToString();
        }

        static string[] GetLines(string text)
        {
            var lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }

            return lines;
        }
    }
}
