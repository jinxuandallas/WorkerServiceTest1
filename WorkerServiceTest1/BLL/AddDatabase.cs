using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data;
using MySql.Data.MySqlClient;
using WorkerServiceTest1.Model;

namespace WorkerServiceTest1.BLL
{
    public interface IAddDatabase
    {
        void dbOpen();
        void dbClose();
        void AddOneRecord();
        bool AddProduct(Product product);
        int AddKeyword(string[] keywords);
    }
    public class AddDatabase : IAddDatabase
    {
        private static string connectionString = @"Server=localhost;Port=3306;Database=shoppingcrawler;Uid=root;Pwd=123456;";
        private IDbConnection db;

        public AddDatabase()
        {
            //db = new MySqlConnection(connectionString);
            //db.Open();
            //db.Close();
        }


        public void dbOpen()
        {
            db.Open();
        }

        public void dbClose()
        {
            db.Close();
        }

        public void AddOneRecord()
        {
            using (IDbConnection db2 = new MySqlConnection(connectionString))
            {
                Guid g = Guid.NewGuid();
                db2.Execute("insert into testws values (@id,@time)", new { id = g, time = DateTimeOffset.Now.DateTime });
            }
        }

        public bool AddProduct(Product product)
        {
            product.id = Guid.NewGuid();
            using (db=new MySqlConnection(connectionString))
            {
                db.Execute("insert into crawlresult values(@id,@Keyword,@GlobalText)", product);
            }
            return true;
        }

        public int AddKeyword(string[] keywords)
        {
            string keyword = string.Join(' ', keywords);
            using (db = new MySqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@keyword", keyword);
                param.Add("@keywordid", 0, DbType.Int32, ParameterDirection.Output);
                var res = db.Execute("Add_keyword", param, null, null, CommandType.StoredProcedure);
                return int.Parse(param.Get<object>("@keywordid").ToString());
            }
        }
    }
}
