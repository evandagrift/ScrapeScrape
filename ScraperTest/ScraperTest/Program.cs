using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScraperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter String");
                string x = Console.ReadLine();
                var task = GetHtmlAsync(x);
                task.Wait();
                var result =
                task.Result;
                foreach (HtmlNode n in result) Console.WriteLine(n.InnerText);
                Console.WriteLine();
            }
        }

        private static async Task<List<HtmlNode>> GetHtmlAsync(string searchTerm)
        {
            var url = "https://www.merriam-webster.com/dictionary/" + searchTerm;
            var httpClient = new HttpClient();
            List<HtmlNode> flList = new List<HtmlNode>();

            //get searches here only
            //move everything else
            try
            {
                var html = await httpClient.GetStringAsync(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                var definitionList = htmlDocument.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("id", "").Contains("dictionary-entry-")).ToList();

                var WordSearchedList = htmlDocument.DocumentNode.Descendants("h1").Where(node => node.GetAttributeValue("class", "").Contains("hword")).ToList();
                flList = htmlDocument.DocumentNode.Descendants("span").Where(node => node.GetAttributeValue("class", "").Contains("fl")).ToList();
                Console.WriteLine("It's a real word!");
            }
            catch(HttpRequestException x) {
                Console.WriteLine("not a real word!");
            }

            return (flList);


        }
    }
}
