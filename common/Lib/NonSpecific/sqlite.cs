using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//http://blog.tigrangasparian.com/2012/02/09/getting-started-with-sqlite-in-c-part-one/
public class Sqlite
{
    SQLiteConnection m_dbConnection;


    public Sqlite(string filePath)
    {

        if (!File.Exists(filePath))
        {
            SQLiteConnection.CreateFile(filePath);
        }

        
        m_dbConnection =
            new SQLiteConnection(String.Format("Data Source={0};Version=3;", filePath));


    }
}