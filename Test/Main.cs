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

            var url = "http://unimedia.info/stiri/video-flashmob-in-pman-sus-Tinem-vinul-moldovenesc-65465.html";

		    var page = (new NBoilerpipe.Util.WebPageExtractor()).GetHtml(url).Result;
			
			String text = ArticleExtractor.INSTANCE.GetText (page);
			Console.WriteLine ("Text: \n" + text);
		}
	}
}
