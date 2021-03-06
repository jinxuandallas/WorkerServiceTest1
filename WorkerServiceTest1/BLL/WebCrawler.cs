﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Cache;
using HtmlAgilityPack;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using WorkerServiceTest1.Model;

namespace WorkerServiceTest1.BLL
{
    public interface IWebCrawler
    {
        void SaveHtmlTxt(string url);
        string CrawlSteepandCheap(string html);
    }

    public class WebCrawler : IWebCrawler
    {
        //public static string connectionString = @"Server=localhost;Port=3306;Database=shoppingcrawler;Uid=root;Pwd=123456;";
        //public IDbConnection db;

        public IAddDatabase _iad;

        public WebCrawler(IAddDatabase iad)
        {
            _iad = iad;
        }

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

            Product product = new Product();
            product.Keyword= _iad.AddKeyword(words);

            HtmlNode globalText = doc.DocumentNode.SelectSingleNode("//button[contains(@class,'global-text')]");
            if (globalText != null)
                product.GlobalText = HttpUtility.HtmlDecode(globalText.InnerText).Trim();

            _iad.AddProduct(product);
            //    return "No globaltext";

                //HtmlNode s = globalText.SelectSingleNode("./span");
                //return s.InnerText;
                //System.Web.HttpUtility.HtmlDecode
                //return HttpUtility.HtmlDecode(globalText.InnerText);

                //return targetNode.InnerText;
            return product.Keyword.ToString();
        }

        //private int AddKeyword(string[] keywords)
        //{
        //    string keyword = string.Join(' ', keywords);
        //    using (db = new MySqlConnection(connectionString))
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("@keyword", keyword);
        //        param.Add("@keywordid", 0, DbType.Int32, ParameterDirection.Output);
        //        var res = db.Execute("Add_keyword", param, null, null, CommandType.StoredProcedure);
        //        return int.Parse(param.Get<object>("@keywordid").ToString());
        //    }
        //}

        private bool ContainWords(string[] words, string s)
        {
            foreach (string w in words)
                if (!s.ToLower().Contains(w.ToLower()))
                    return false;
            return true;
        }
    }
}
