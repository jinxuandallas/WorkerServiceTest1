using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace WorkerServiceTest1.BLL
{
    public interface IWebCrawler
    {
        void SaveHtmlTxt(string url);
    }

    public class WebCrawler : IWebCrawler
    {
        public void SaveHtmlTxt(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, Encoding.Default);//注意编码
            strHTML = sr.ReadToEnd();
            myStream.Close();

            File.WriteAllText(@"c:\example.txt", strHTML);
            //return strHTML;
        }
    }
}
