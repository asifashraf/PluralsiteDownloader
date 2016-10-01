using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//http://blog.tigrangasparian.com/2012/02/09/getting-started-with-sqlite-in-c-part-one/
public class Sqlite
{
    SQLiteConnection m_dbConnection;


    public Sqlite()
    {
        SQLiteConnection.CreateFile("db.sqlite");
        m_dbConnection =
            new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
    }
}