using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Cache;
using HtmlAgilityPack;
using System.Web;

namespace WorkerServiceTest1.BLL
{
    public interface IWebCrawler
    {
        void SaveHtmlTxt(string url);
        string CrawlSteepandCheap(string html);
    }

    public class WebCrawler : IWebCrawler
    {
        public void SaveHtmlTxt(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            myWebClient.Headers.Set("User-Agent", "Microsoft Internet Explorer");
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, Encoding.Default);//注意编码
            strHTML = sr.ReadToEnd();
            myStream.Close();

            File.WriteAllText(@"Example\example.txt", strHTML);
            //return strHTML;
        }

        public string CrawlSteepandCheap(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection liNodes = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'ui-product-listing-grid')]/li");

            if (liNodes == null)//如果关键词搜索不到任何商品
                return "We couldn’t find any results";

            string[] words = new string[] { "alpha", "sv", "men" };
            HtmlNode targetNode = liNodes.FirstOrDefault(ln => ContainWords(words, ln.InnerText));

            if (targetNode == null)//如果找不到符合条件的产品
                return "No qualified products";

            HtmlNode globalText = doc.DocumentNode.SelectSingleNode("//button[contains(@class,'global-text')]");
            if (globalText == null)
                return "No globaltext";

            //HtmlNode s = globalText.SelectSingleNode("./span");
            //return s.InnerText;
            //System.Web.HttpUtility.HtmlDecode
            return HttpUtility.HtmlDecode(globalText.InnerText);

            //return targetNode.InnerText;
        }

        private bool ContainWords(string[] words, string s)
        {
            foreach (string w in words)
                if (!s.ToLower().Contains(w.ToLower()))
                    return false;
            return true;
        }
    }
}
