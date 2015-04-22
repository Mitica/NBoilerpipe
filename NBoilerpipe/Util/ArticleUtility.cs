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
        /// <param name="url"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public static string ClearArticleByUrl(string url, string head)
        {
            var extractor = new WebPageExtractor();

            string html;
            try
            {
                html = extractor.GetHtml(url).Result;
            }
            catch
            {
                return null;
            }

            return ClearArticleByHtml(html, head);
        }

        /// <summary>
        /// Detects article inner text from a HTML text
        /// </summary>
        /// <param name="html"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public static string ClearArticleByHtml(string html, string head)
        {
            string article;


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
            if (string.IsNullOrEmpty(article)) return null;
            var content = article;

            if (string.IsNullOrEmpty(content) || head!=null && content.Length < head.Length) return null;

            var contentLines = GetLines(content);

            var startContentLineIndex = 0;

            if (!string.IsNullOrEmpty(head))
            {
                var summaryLines = GetLines(head);

                var summaryStart = summaryLines[0].WrapAtNoDots(100);

                for (int i = 0; i < contentLines.Length; i++)
                {
                    var contentLine = contentLines[i];
                    if (contentLine.StartsWith(summaryStart) || summaryStart.StartsWith(contentLine.WrapAtNoDots(90)))
                    {
                        startContentLineIndex = i;
                        break;
                    }
                }
            }
            //if (startContentLineIndex == -1) return null;

            var contentBuilder = new StringBuilder();

            for (int i = startContentLineIndex; i < contentLines.Length; i++)
            {
                var line = contentLines[i];
                if (i > contentLines.Length - 3)
                {
                    if (line.Length < 50 || line.RemoveDoubleWihteSpaces().Length < 50) break;
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
