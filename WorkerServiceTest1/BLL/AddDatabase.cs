using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WorkerServiceTest1.BLL
{
    public interface IAddDatabase
    {
        void dbOpen();
        void dbClose();
        void AddOneRecord();
    }
    public class AddDatabase : IAddDatabase
    {
        public static string connectionString = @"Server=localhost;Port=3306;Database=test;Uid=root;Pwd=123456;";
        public IDbConnection db;

        public AddDatabase()
        {
            db = new MySqlConnection(connectionString);
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
    }
}
