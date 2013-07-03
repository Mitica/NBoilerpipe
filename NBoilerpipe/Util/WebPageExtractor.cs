using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NBoilerpipe.Util
{
    /// <summary>
    /// Decoded HTML code extractor from URL
    /// </summary>
    public class WebPageExtractor
    {
        #region Public

        /// <summary>
        /// Extracts (decoded) HTML code from URL
        /// </summary>
        /// <param name="url">Web page URL</param>
        /// <returns></returns>
        public string GetHtml(string url)
        {
            var request = WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            return DecodeData(response);
        }

        #endregion


        static string DecodeData(WebResponse w, Encoding defaultEncoding = null)
        {
            defaultEncoding = defaultEncoding ?? Encoding.UTF8;

            string charset = null;
            var ctype = w.Headers["content-type"];
            if (ctype != null)
            {
                int ind = ctype.IndexOf("charset=", StringComparison.OrdinalIgnoreCase);
                if (ind != -1)
                {
                    charset = ctype.Substring(ind + 8);
                }
            }
            Encoding e = null;

            if (!string.IsNullOrEmpty(charset))
            {
                try
                {
                    e = Encoding.GetEncoding(charset);
                }
                catch
                {
                    charset = null;
                }
            }
            MemoryStream rawdata = null;
            if (e == null)
            {

                // save data to a memorystream
                rawdata = new MemoryStream();
                var buffer = new byte[1024];
                var rs = w.GetResponseStream();
                int read = rs.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    rawdata.Write(buffer, 0, read);
                    read = rs.Read(buffer, 0, buffer.Length);
                }

                rs.Close();


                var ms = rawdata;
                ms.Seek(0, SeekOrigin.Begin);

                var srr = new StreamReader(ms, Encoding.ASCII);
                var meta = srr.ReadToEnd();

                if (!string.IsNullOrEmpty(meta))
                {
                    int startInd = meta.IndexOf("charset=", StringComparison.OrdinalIgnoreCase);
                    if (startInd != -1)
                    {
                        int endInd = meta.IndexOf("\"", startInd, System.StringComparison.Ordinal);
                        if (endInd != -1)
                        {
                            var start = startInd + 8;
                            charset = meta.Substring(start, endInd - start + 1);
                            charset = charset.TrimEnd(new Char[] { '>', '"' });
                        }
                    }
                }

                if (charset == null)
                {
                    e = defaultEncoding;
                }
                else
                {
                    try
                    {
                        e = Encoding.GetEncoding(charset);
                    }
                    catch
                    {
                        e = defaultEncoding;
                    }
                }
            }

            Stream data = null;
            if (rawdata != null)
            {
                data = rawdata;
                data.Seek(0, SeekOrigin.Begin);
            }
            else data = w.GetResponseStream();

            var sr = new StreamReader(data, e);

            return sr.ReadToEnd();
        }
    }
}
