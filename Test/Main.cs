using System;
using System.IO;
using System.Net;
using System.Text;
using NBoilerpipe.Extractors;

namespace Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
		    String url = "http://www.l3s.de/web/page11g.do?sp=page11g&link=ln104g&stu1g.LanguageISOCtxParam=en";
//			String url = "http://www.dn.se/nyheter/vetenskap/annu-godare-choklad-med-hjalp-av-dna-teknik";

		    var page = new NBoilerpipe.Util.WebPageExtractor().GetHtml(url);
			
			String text = ArticleExtractor.INSTANCE.GetText (page);
			Console.WriteLine ("Text: \n" + text);
		}
	}
}
