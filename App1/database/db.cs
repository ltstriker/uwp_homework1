using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.database
{
    class Db
    {
        static Db dbInstance;
        private SQLiteConnection conn;

        Db()
        {
            LoadDatabase();
        }

        static public Db GetInstance()
        {
            if (dbInstance==null)
            {
                dbInstance = new Db();
            }
            return dbInstance;
        }

        private void LoadDatabase()
        {     // Get a reference to the SQLite database    
            conn = new SQLiteConnection("Todo.db");
            string sql = @"CREATE TABLE IF NOT EXISTS item (
                    id    VARCHAR( 140 ) PRIMARY KEY NOT NULL,
                    title   VARCHAR( 140 ),
                    content VARCHAR( 140 ),
                    date    VARCHAR( 140 ),
                    image   VARCHAR( 140 ),
                    finish  INTEGER
                    );";
            using (var statement = conn.Prepare(sql))
            { statement.Step(); }
        }

        public ObservableCollection<Models.ListItem> GetAll()
        {
            var db = this.conn;
            var view = new ObservableCollection<Models.ListItem>();
            try
            {//UPDATE Customer SET Name = ?, City = ?, Contact = ? WHERE Id=?
                using (var statement = db.Prepare("SELECT * FROM item"))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        var temp = ((string)statement[3]).Split('/');
                        var date1 = new DateTime(int.Parse(temp[2]), int.Parse(temp[0]), int.Parse(temp[1]));
                        var date = new DateTimeOffset(date1);
                        var titem = new Models.ListItem((string)statement[1], (string)statement[2], date, 
                            (string)statement[4], (string)statement[0], ((Int64)statement[5]) == Int64.Parse("1") ? true : false);
                        view.Add(titem);
                    }
                }
                return view;
            }
            catch (Exception ex)
            {
                // TODO: Handle error
                return view;
            }
        }

        public void Insert(string id, string title, string content, string date, string imageString)
        {
            // SqlConnection was opened in App.xaml.cs and exposed through property conn
            var db = this.conn;
            try{
                using (
                    var sql = db.Prepare("INSERT INTO item (id, title, content, date, image, finish) VALUES (?, ?, ?, ?, ?, ?)")){
                    sql.Bind(1, id);
                    sql.Bind(2, title);
                    sql.Bind(3, content);
                    sql.Bind(4, date);
                    sql.Bind(5, imageString);
                    sql.Bind(6, -1);
                    sql.Step();
                }
            }catch (Exception ex){
                // TODO: Handle error
            }
        }

        public void Remove(string id)
        {
            // SqlConnection was opened in App.xaml.cs and exposed through property conn
            var db = this.conn;
            try
            {
                using (var statement = db.Prepare("DELETE FROM item WHERE id = ?;"))
                {
                    statement.Bind(1, id);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle error
            }
        }

        public void Update(string id, string title, string content, string date, string imageString)
        {
            // SqlConnection was opened in App.xaml.cs and exposed through property conn
            var db = this.conn;
            try
            {//UPDATE Customer SET Name = ?, City = ?, Contact = ? WHERE Id=?
                using (var statement = db.Prepare("UPDATE item SET title = ?, content = ?, date = ?, image = ? WHERE id = ?"))
                {
                    statement.Bind(1, title);
                    statement.Bind(2, content);
                    statement.Bind(3, date);
                    statement.Bind(4, imageString);
                    statement.Bind(5, id);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle error
            }
        }

        public void Complete(string id, bool finish)
        {
            // SqlConnection was opened in App.xaml.cs and exposed through property conn
            var db = this.conn;
            try
            {//UPDATE Customer SET Name = ?, City = ?, Contact = ? WHERE Id=?
                using (var statement = db.Prepare("UPDATE item SET finish = ? WHERE id = ?"))
                {
                    statement.Bind(1, finish?1:0);
                    statement.Bind(2, id);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle error
            }
        }

        public string Search(string str)
        {
            var db = this.conn;
            StringBuilder msg = new StringBuilder();

            try
            {//UPDATE Customer SET Name = ?, City = ?, Contact = ? WHERE  name like ‘%values%
                int count = 0;
                using (var statement = db.Prepare("SELECT * FROM item WHERE title LIKE ? OR content LIKE ? OR date LIKE ?"))
                {
                    statement.Bind(1, "%"+str+"%");
                    statement.Bind(2, "%" + str + "%");
                    statement.Bind(3, "%" + str + "%");
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        count++;
                        msg.Append("第" + count + "项：\ntitle: " + (string)statement[1] + "\ncontent: " + (string)statement[2] + 
                            "\ndate: " + (string)statement[3] + "\n");
                    }
                    msg.Insert(0,"共" + count + "项：\n");
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle error
                msg.Append("无");
            }
            return msg.ToString();
        }
    }
}
