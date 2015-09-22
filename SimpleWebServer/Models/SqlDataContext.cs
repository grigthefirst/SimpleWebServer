using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer.Models
{
    public class SqlPostDataContext : IDataContext<PostDataItem>
    {
        private string connectionString;
        public SqlPostDataContext(string connectionString)
        {
            this.connectionString = connectionString;
            using (SQLiteConnection con = new SQLiteConnection(this.connectionString))
            {
                string initQuery =
                    @"
                     CREATE TABLE IF NOT EXISTS Users (
                      Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                      Name TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Messages (
                      Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                      Message TEXT,
                      DateCreated datetime,
                      UserId INTEGER,
                      FOREIGN KEY(UserId) REFERENCES Users(Id)
                    );";
                con.Open();
                using(SQLiteCommand com = new SQLiteCommand(initQuery, con))
                {
                    com.ExecuteNonQuery();
                }
            }

            
        }

        public PostDataItem GetById(int id)
        {
            PostDataItem item = null;
            using (SQLiteConnection con = new SQLiteConnection(this.connectionString))
            {
                string query = @"SELECT u.Name as User, m.Message as Message, m.DateCreated as DateCreated FROM Messages m inner join Users u on u.Id = m.UserId where m.Id = @id";
                con.Open();
                using (SQLiteCommand com = new SQLiteCommand(query, con))
                {
                    com.Parameters.Add("@id", System.Data.DbType.Int32);
                    com.Parameters["@id"].Value = id;
                    using(SQLiteDataReader r = com.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            string user = r["User"] != null ? r["User"].ToString() : "";
                            string message = r["Message"] != null ? r["Message"].ToString() : "";
                            item = new PostDataItem(user, message);

                            DateTime dateCreated = DateTime.MinValue;
                            if (r["DateCreated"] != null && DateTime.TryParse(r["DateCreated"].ToString(), out dateCreated))
                                item.DateCreated = dateCreated;
                        }
                    }
                }
            }
            return item;
        }

        public IEnumerable<PostDataItem> GetAll()
        {
            List<PostDataItem> items = new List<PostDataItem>();
            using (SQLiteConnection con = new SQLiteConnection(this.connectionString))
            {
                string query = @"SELECT u.Name as User, m.Message as Message, m.DateCreated as DateCreated FROM Messages m inner join Users u on u.Id = m.UserId";
                con.Open();
                using (SQLiteCommand com = new SQLiteCommand(query, con))
                {
                    using (SQLiteDataReader r = com.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            string user = r["User"] != null ? r["User"].ToString() : "";
                            string message = r["Message"] != null ? r["Message"].ToString() : "";

                            PostDataItem item = new PostDataItem(user, message);

                            DateTime dateCreated = DateTime.MinValue;
                            if (r["DateCreated"] != null && DateTime.TryParse(r["DateCreated"].ToString(), out dateCreated))
                                item.DateCreated = dateCreated;

                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public void Add(PostDataItem item)
        {
            using (SQLiteConnection con = new SQLiteConnection(this.connectionString))
            {
                string query =
                    @"
                    insert into Users (Name) values (@name);
                    insert into Messages (UserId, Message, DateCreated) values (last_insert_rowid(), @message, @dateCreated);
                    ";
                con.Open();
                using (SQLiteCommand com = new SQLiteCommand(query, con))
                {
                    com.Parameters.Add("@name", System.Data.DbType.AnsiString);
                    com.Parameters["@name"].Value = item.User;

                    com.Parameters.Add("@message", System.Data.DbType.AnsiString);
                    com.Parameters["@message"].Value = item.Message;

                    com.Parameters.Add("@dateCreated", System.Data.DbType.DateTime);
                    com.Parameters["@dateCreated"].Value = DateTime.Now;

                    com.ExecuteNonQuery();
                }
            }
        }
    }
}
