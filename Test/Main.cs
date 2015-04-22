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

            var url = "http://www.realitatea.net/cele-mai-mari-reduceri-la-laptopuri-i-calculatoare-second-hand_1681165.html";

		    var page = NBoilerpipe.Util.ArticleUtility.ClearArticleByUrl(url, null);
			
			String text = ArticleExtractor.INSTANCE.GetText (page);
			Console.WriteLine ("Text: \n" + text);
		}
	}
}
